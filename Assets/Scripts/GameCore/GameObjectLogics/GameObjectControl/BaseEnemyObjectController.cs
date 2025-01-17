﻿using UnityEngine;
using System.Collections;
using GameLogic;
using FormulaBase;

public abstract class BaseEnemyObjectController : BaseSpineObjectController {
	private const string POINT_CENTER = "pc";
	private static string[] listResultMap = new string[]{"", "", ACTION_KEYS.COMEOUT1, ACTION_KEYS.COMEOUT2, ACTION_KEYS.COMEOUT3, ACTION_KEYS.JUMP};

	[SerializeField]
	public bool attackedDoNothing = false;

	public bool IsEmptyNode() {
		SkeletonAnimation sklAni = this.gameObject.GetComponent<SkeletonAnimation> ();
		return sklAni == null;
	}

	public void AttackedSuccessful(uint result, bool isDead = true) {  // (mark) all obeject has been attack here!
		if (this.attackedDoNothing) {
			return;
		}

		this.OnControllerAttacked ((int)result, isDead);
		GameGlobal.gGameMusicScene.OnObjBeAttacked (this.idx);
		this.OnAttackDestory ();
	}

	public virtual bool IsShotPause() {
		return false;
	}
	
	public virtual void SetShotPause(decimal tick) {
	}

	public virtual void OnAttackDestory() {

	}

	public override void SetIdx (int idx) {
		this.idx = idx;
	}

	public override void Init () {
	}

	public override bool ControllerMissCheck (int idx, decimal currentTick) {
		bool isJumpping = GirlManager.Instance.IsJumpingAction ();
		decimal missHardTime = GameGlobal.gGameMissPlay.GetMissHardTime ();
		MusicData md = StageBattleComponent.Instance.GetMusicDataByIdx (idx);
		if (md.nodeData.hit_type == GameMusic.NONE) {
			return false;
		}

		if (BattleEnemyManager.Instance.IsDead (idx)) {
			return false;
		}
		
		//if (md.IsShotPausing ()) {
		//	return false;
		//}

		if (currentTick == GameGlobal.MISS_NO_CHECK_TICK) { // && md.Tick () != currentTick) {
			return false;
		}

		//if ((md.nodeData.isAirunits || md.nodeData.enableJump) && isJumpping) {
		if (md.nodeData.enable_jump && isJumpping) {
			return false;
		}

		if (md.IsPressTemp ()) {
			return true;
		}

		if (missHardTime > 0) {
			BattleEnemyManager.Instance.SetPlayResult (idx, GameMusic.MISS);
			return false;
		}

		return true;
	}

	public override void OnControllerStart () {
		Animator ani = this.gameObject.GetComponent<Animator> ();
		if (ani != null) {
			ani.Stop ();
		}

		SpineActionController.Play (ACTION_KEYS.COMEIN, this.gameObject);
		string aniName = BattleEnemyManager.Instance.GetNodeBossAnimationByIdx(this.idx);
		if (aniName != null) {
			Boss.Instance.Play (aniName);
		}

		StartCoroutine(this.AfterControllerStart());
	}

	public IEnumerator AfterControllerStart () {
		yield return new WaitForSeconds (0.1f);
		SpineMountController smc = this.gameObject.GetComponent<SpineMountController> ();
		if (smc != null) {
			smc.enabled = true;
			smc.OnControllerStart ();
		}
	}

	public override void OnControllerAttacked (int result, bool isDeaded) {
		if (!isDeaded) {
			SpineActionController.Play (ACTION_KEYS.HURT, this.gameObject);
			return;
		}
		
		string actionKey = listResultMap [result];

		Vector3 orgpos = this.gameObject.transform.position;
		Vector3 bonepos = SpineActionController.GetBoneRealPosition (POINT_CENTER, this.gameObject);
		this.gameObject.transform.position = new Vector3 (orgpos.x + bonepos.x, orgpos.y + bonepos.y, orgpos.z + bonepos.z);
		SpineActionController.Play (actionKey, this.gameObject);

		string hitAnimation = BattleEnemyManager.Instance.GetNodeBossHitAnimationByIdx (this.idx);
		Boss.Instance.Play (hitAnimation);
	}

	public override bool OnControllerMiss (int idx) {
		bool isShowMiss = this.__OnControllerMiss (idx);		// (mark) player gain damage here!
		BattleEnemyManager.Instance.SetPlayResult (idx, GameMusic.MISS);
		if (isShowMiss) {
			//CharBehaviour.GirlInstance.ShowLabel (GameMusic.MISS);
			// hurt show
			AttacksController.Instance.BeAttacked ();
		}

		this.OnAttackDestory ();

		return isShowMiss;
	}

	private bool __OnControllerMiss(int idx) {
		ArrayList musicData = StageBattleComponent.Instance.GetMusicData ();
		MusicData md = (MusicData)musicData [idx];
		if (md.nodeData.hit_type == GameMusic.NONE) {
			return false;
		}
		
		if (!md.nodeData.missCombo) {
			return false;
		}
		
		// Fire buff event.
		if (!GameGlobal.IS_DEBUG) {
			int hurtValue = BattleEnemyManager.Instance.GetDamageValueByIndex (idx);
			BattleRoleAttributeComponent.Instance.Hurt (-hurtValue);
		}
		
		GameGlobal.gGameMissPlay.SetMissHardTime (GameMusic.MISS_AVOID_TIME);

		if (!BattleRoleAttributeComponent.Instance.IsComboProtect ()) {
			StageBattleComponent.Instance.SetCombo (0);
			CharPanel.Instance.StopCombo ();
			EffectManager.Instance.StopCombo ();
		}

		GameGlobal.gGameMusicScene.OnObjBeMissed (this.idx);
		
		return true;
	}
}