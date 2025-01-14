///自定义模块，可定制模块具体行为
using System;
using UnityEngine;
namespace FormulaBase {
	public class AccountCrystalManagerComponent : CustomComponentBase {
		private static AccountCrystalManagerComponent instance = null;
		private const int HOST_IDX = 1;
		public static AccountCrystalManagerComponent Instance {
			get {
				if(instance == null) {
					instance = new AccountCrystalManagerComponent();
				}
			return instance;
			}
		}
		//----------------------------------------
		public void SetDiamond(int diamond) {
			FormulaHost account = AccountManagerComponent.Instance.GetAccount ();
			if (account == null) {
				return;
			}

			account.SetDynamicData (SignKeys.DIAMOND, diamond);
		}

		public int GetDiamond() {
			FormulaHost account = AccountManagerComponent.Instance.GetAccount ();
			if (account == null) {
				return 0;
			}

			return (int)account.GetDynamicDataByKey (SignKeys.DIAMOND);
		}

		// TODO : add diamond here
		/// <summary>
		/// Changes the diamond.
		/// 
		/// Sample :
		/// AccountCrystalManagerComponent.Instance.ChangeDiamond (-1000, true, new HttpResponseDelegate (((bool result) => {
		/// 	Debug.Log ("On Change Diamond : " + result);
		/// })));
		/// 
		/// </summary>
		/// <returns><c>true</c>, if diamond was changed, <c>false</c> otherwise.</returns>
		/// <param name="diamond">Diamond.</param>
		/// <param name="isave">If set to <c>true</c> isave.</param>
		/// <param name="rsp">Rsp.</param>
		public bool ChangeDiamond(int diamond, bool isave = true, HttpResponseDelegate rsp = null) {
			FormulaHost account = AccountManagerComponent.Instance.GetAccount ();
			if (account == null) {
				return false;
			}

			CommonPanel.GetInstance ().ShowWaittingPanel (true);
			bool result = account.AddDynamicValueRemote (SignKeys.DIAMOND, diamond, isave, new HttpResponseDelegate ((bool _result) => {
				this.ChangeDiamondCallback (_result);
				if (rsp != null) {
					rsp (_result);
				}
			}), true);

			if (!result) {
				CommonPanel.GetInstance ().ShowWaittingPanel (false);
			}

			return result;
		}

		private void ChangeDiamondCallback(bool _success) {
			if (!_success) {
				CommonPanel.GetInstance ().ShowText ("连接失败");
				return;
			}

			Messenger.Broadcast (MainMenuPanel.BroadcastChangeDiamond);
			Messenger.Broadcast (MainMenuPanel.Broadcast_MainMenuChangeDiamond);
			CommonPanel.GetInstance ().ShowWaittingPanel (false);
			Debug.Log ("接受改变钻石的反馈");
		}
	}
}