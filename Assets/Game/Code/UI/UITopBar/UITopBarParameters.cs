using NGame.Player;
using RyUI;

namespace NGame.UI
{
	public class UITopBarParameters : IUIParameters
	{
		public readonly PlayerProfile PlayerProfile;
		public readonly UIHandler UIHandler;

		public UITopBarParameters(PlayerProfile playerProfile, UIHandler uiHandler) {
			PlayerProfile = playerProfile;
			UIHandler = uiHandler;
		}
	}
}