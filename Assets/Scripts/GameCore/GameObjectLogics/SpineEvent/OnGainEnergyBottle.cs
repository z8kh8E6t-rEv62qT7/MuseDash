﻿using FormulaBase;
using System.Collections;
using UnityEngine;
using Spine;

namespace GameLogic
{
    public class OnGainEnergyBottle : DoNothing
    {
        private static GameObject recoveryEffect = null;

        private void Start()
        {
            if (recoveryEffect == null)
            {
                int recoverItemIdx = 4000;
                string path = "Prefabs/skill/Skill_hp";
                recoveryEffect = StageBattleComponent.Instance.AddObjWithControllerInit(ref path, recoverItemIdx);
                recoveryEffect.SetActive(false);
            }
        }

        private void PlayRecoveryEffect()
        {
            recoveryEffect.SetActive(true);
            SpineActionController.Play(ACTION_KEYS.COMEIN, recoveryEffect);
        }

		public override void Do(TrackEntry entry)
        {
            PlayRecoveryEffect();

            //destory it self
            SpineMountController smc = this.gameObject.GetComponent<SpineMountController>();
            if (smc != null)
            {
                smc.DestoryDynamicObjects();
            }

            this.gameObject.SetActive(false);
            string name = this.gameObject.name.Remove(this.gameObject.name.Length - 7);

            Debug.Log("name to be destroy is: " + name);

            SceneObjectController.Instance.SceneObjectPool[name] = null;
            this.gameObject.SetActive(false);
            //GameObject.Destroy(this.gameObject);
        }
    }
}