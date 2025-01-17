/// UI分析工具自动生成代码
/// PnlAchievementUI主模块
/// 
using System;
using UnityEngine;
using System.Collections.Generic;
using FormulaBase;
using System.Collections;
using GameLogic;


namespace PnlAchievement {
	public class PnlAchievement : UIPhaseBase {
		private static PnlAchievement instance = null;
		public static PnlAchievement Instance {
			get {
					return instance;
			}
		}

		public int sliderWidth;
		public UISprite nextTrophyShow;
		public List<GameObject> trophys;

		public TweenWidth slideCombo;
		public TweenWidth slidePerfect;
		public TweenWidth slideStars;
		public TweenWidth slideClear;

		void Start() {
			instance = this;

			this.slideCombo.enabled = false;
			this.slidePerfect.enabled = false;
			this.slideStars.enabled = false;
			this.slideClear.enabled = false;
		}

		public override void OnShow () {
			foreach (GameObject t in this.trophys) {
				t.SetActive (false);
			}

			this.StartCoroutine (this.__OnShow (0.1f));
		}

		public override void OnHide () {
		}

		private IEnumerator __OnShow(float sec) {
			yield return new WaitForSeconds (sec);

			StageBattleComponent.Instance.Host.SetAsUINotifyInstance ();
			TaskStageTarget.Instance.Host.SetAsUINotifyInstance ();
			int rank = TaskStageTarget.Instance.GetStageEvluateMax ();
			for (int i = 0; i < rank; i++) {
				GameObject t = this.trophys [i];
				t.SetActive (i < rank);
			}

			this.ShowRankProgress (TaskStageTarget.TASK_SIGNKEY_MAX_COMBO, "Combo_", this.slideCombo);
			this.ShowRankProgress (TaskStageTarget.TASK_SIGNKEY_EVLUATE_HEAD + GameMusic.PERFECT, "Perfect_", this.slidePerfect);
			this.ShowRankProgress (TaskStageTarget.TASK_SIGNKEY_HIDE_NODE_COUNT, "Star_", this.slideStars);
			this.ShowRankProgress (TaskStageTarget.TASK_SIGNKEY_STAGE_CLEAR_COUNT, "Clear_", this.slideClear);
		}

		private void ShowRankProgress(string taskKey, string cfgKey, TweenWidth slider) {
			float rateBase = 0.2f;
			float rank = 0;
			int sid = StageBattleComponent.Instance.GetId ();
			string strSid = sid.ToString ();
			int vmax = TaskStageTarget.Instance.GetXMax (taskKey);
			foreach (string strRank in GameGlobal.STAGE_EVLUATE_MAP) {
				string _cfgKey = cfgKey + strRank;
				int _cfgValue = ConfigPool.Instance.GetConfigIntValue (StageRewardComponent.REWARD_CONFIG_NAME, strSid, _cfgKey);
				if (vmax <= _cfgValue) {
					break;
				}

				rank += 1;
			}

			int rate = (int)(rateBase * rank * this.sliderWidth);
			slider.enabled = true;
			slider.from = 0;
			slider.to = rate;
			slider.ResetToBeginning ();
			slider.PlayForward ();
		}
	}
}