// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;

namespace GameLogic
{
    public class OnJumpEnd : DoNothing
    {
        public override void Do(Spine.AnimationState state, int trackIndex, int loopCount)
        {
            var curAnimName = GirlActionController.instance.curAnimName;
            var isJumping = (curAnimName == ACTION_KEYS.JUMP_ATTACK) || (curAnimName == ACTION_KEYS.JUMP) || (curAnimName == ACTION_KEYS.JUMP_HURT);
            GirlManager.Instance.SetJumpingAction(isJumping);
            SpineActionController.Play(ACTION_KEYS.RUN, this.gameObject);
        }
    }
}