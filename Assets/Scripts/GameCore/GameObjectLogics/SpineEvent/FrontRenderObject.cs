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

namespace GameLogic {
	public class FrontRenderObject : DoNothing {
		public override void Do (Spine.AnimationState state, int trackIndex, int loopCount) {
			MeshRenderer render = this.gameObject.GetComponent<MeshRenderer> ();
			if (render == null) {
				return;
			}
			
			render.sortingOrder = GameGlobal.LIMITE_INT;
		}
	}
}
