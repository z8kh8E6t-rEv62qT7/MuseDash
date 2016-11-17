using Assets.Scripts.Common.Manager;
using GameLogic;

///自定义模块，可定制模块具体行为
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBase
{
    public class StageRewardComponent : CustomComponentBase
    {
        private static StageRewardComponent instance = null;
        private const int HOST_IDX = 3;

        public static StageRewardComponent Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StageRewardComponent();
                }
                return instance;
            }
        }

        // ------------------------------------------------------------------------------------
        public const string REWARD_CONFIG_NAME = "stage_value";

        public const string ACHIEVEMENT_CONFIG_NAME = "achievement";

        public static string[] ACHIEVEMENT_GOAL_MAP = new string[] { "", "c_goal", "b_goal", "a_goal", "s_goal" };
        public static string[] ACHIEVEMENT_REWARD_MAP = new string[] { "", "c_award", "b_award", "a_award", "s_award" };

        public FormulaHost stage
        {
            private set;
            get;
        }

        public void SetStage(FormulaHost host)
        {
            this.stage = host;
        }

        /// <summary>
        /// Stages the reward.
        ///
        /// 关卡奖励
        /// </summary>
        /// <param name="rank">Rank.</param>
        /// <param name="rankStr">Rank string.</param>
        /// <param name="rechargeData">Recharge data.</param>
        public void StageReward(FormulaHost stageHost, bool isNewRank)
        {
            this.SetStage(stageHost);

            string id = this.stage.GetDynamicStrByKey(SignKeys.ID);

            // 通关基础奖励
            float rwRate = TaskStageTarget.Instance.GetStageRewardRank();
            int rwExp = (int)(ConfigPool.Instance.GetConfigIntValue(REWARD_CONFIG_NAME, id, "exp") * rwRate);
            int rwGold = (int)(ConfigPool.Instance.GetConfigIntValue(REWARD_CONFIG_NAME, id, "coin") * rwRate * RoleManageComponent.Instance.GetGoldAdd());
            int rwCharm = (int)(ConfigPool.Instance.GetConfigIntValue(REWARD_CONFIG_NAME, id, "charm") * rwRate * RoleManageComponent.Instance.GetCharmAdd());
            // reward gold
            stageHost.SetDynamicData(SignKeys.GOLD, rwGold);
            // reward exp
            stageHost.SetDynamicData(SignKeys.EXP, rwExp);
            // reward diamond
            stageHost.SetDynamicData(SignKeys.CHARM, rwCharm);

            AccountGoldManagerComponent.Instance.ChangeMoney(rwGold, false);
            AccountLevelManagerComponent.Instance.ChangeExp(rwExp, false);
            AccountCharmComponent.Instance.ChangeCharm(rwCharm, false);

            // 通关成就奖励
            StageBattleComponent.Instance.Host = AchievementManager.instance.ReceieveAchievement(stageHost, false);
            AccountManagerComponent.Instance.GetAccount().Save();
        }

        // ------------------------------------------------------------------------------------------------------------
    }
}