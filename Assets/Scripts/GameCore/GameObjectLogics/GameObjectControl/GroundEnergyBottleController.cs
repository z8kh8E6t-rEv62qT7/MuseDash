﻿using System;
using GameLogic;
using System.Collections;
using UnityEngine;
using FormulaBase;

public class GroundEnergyBottleController: BaseEnemyObjectController {

	private bool isBeAttacked = false;
	private static GameObject recoveryEffect = null;
	void Start () {
		if (recoveryEffect == null) {
			int recoverItemIdx = 4000;
			string path = "Prefabs/skill/Skill_hp";
			recoveryEffect = StageBattleComponent.Instance.AddObjWithControllerInit (ref path, recoverItemIdx);
			recoveryEffect.SetActive (false);
		}
	}

	public override void OnControllerStart() {
//		host.SetDynamicData(FormulaBase.SignKeys.COMBO, (float)true);
		base.OnControllerStart ();
		//set bottle size flag here
		FormulaHost host = BattleEnemyManager.Instance.Enemies [this.idx];
		if (host != null) {
			host.SetDynamicData ("BOTTLE_TYPE", 1);
		}
	}

	private void RecoveryEffects() {
		var myHost = BattleRoleAttributeComponent.Instance.GetBattleRole ();
		int recoverValue = (int)myHost.Result (FormulaKeys.FORMULA_47);
		BattleRoleAttributeComponent.Instance.AddHp (recoverValue);

		BattleEnemyManager.Instance.SetPlayResult (this.idx, GameMusic.PERFECT);
		BattleRoleAttributeComponent.Instance.FireSkill (SkillComponent.ON_EAT_ITEM);

		recoveryEffect.SetActive (true);
		SpineActionController.Play (ACTION_KEYS.COMEIN, recoveryEffect);
		string audioName = BattleEnemyManager.Instance.GetNodeAudioByIdx (this.idx);
		AudioManager.Instance.PlayGirlHitByName (audioName);
	}

	public override void OnControllerAttacked (int result, bool isDeaded) {
		if (!isBeAttacked && result != (int)GameMusic.JUMPOVER) {
			RecoveryEffects ();
			SpineActionController.Play (ACTION_KEYS.COMEOUT, this.gameObject);
			Animator ani = this.gameObject.GetComponent<Animator> ();
			if (ani != null) {
				ani.speed = 0;
			}

			isBeAttacked = true;
			TaskStageTarget.Instance.AddEnergyItemCount (1);
		}
	}

	public override bool OnControllerMiss (int idx) {
		return true;
	}
}
