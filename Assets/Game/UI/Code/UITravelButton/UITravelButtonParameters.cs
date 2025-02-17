using RyUI;

namespace NGame.UI
{
	public class UITravelButtonParameters : IUIParameters
	{
		public UIHandler UIHandler;

		public UITravelButtonParameters(UIHandler uiHandler) {
			UIHandler = uiHandler;
		}
	}
}