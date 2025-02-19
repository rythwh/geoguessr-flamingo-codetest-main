using JetBrains.Annotations;
using NGame.Player;
using RyUI;

namespace NGame.UI
{

	[UsedImplicitly]
	public class UITopBarPresenter : UIPresenter<UITopBarView>
	{
		private readonly PlayerProfile playerProfile;
		private readonly UIHandler uiHandler;

		public UITopBarPresenter(UITopBarView view, UITopBarParameters parameters) : base(view) {
			playerProfile = parameters.PlayerProfile;
			uiHandler = parameters.UIHandler;
		}

		private void OnCoinsAdded(int amount, int coins) {
			View.SetCoinAmount(coins);
		}

		public override void OnCreate() {

			uiHandler.OnPlayerRolled += OnPlayerRolled;
			OnPlayerRolled(0);

			playerProfile.OnCoinsAdded += OnCoinsAdded;
			OnCoinsAdded(0, playerProfile.Coins);
		}

		private void OnPlayerRolled(int roll) {
			View.SetRollAmount(roll);
		}

		public override void OnClose() {
			uiHandler.OnPlayerRolled -= OnPlayerRolled;
			playerProfile.OnCoinsAdded -= OnCoinsAdded;
		}
	}
}