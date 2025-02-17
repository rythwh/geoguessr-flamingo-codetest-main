using JetBrains.Annotations;
using RyUI;

namespace NGame.UI
{

	[UsedImplicitly]
	public class UITravelButtonPresenter : UIPresenter<UITravelButtonView>
	{
		private readonly UITravelButtonParameters parameters;
		private readonly UIHandler uiHandler;

		public UITravelButtonPresenter(UITravelButtonView view, UITravelButtonParameters parameters) : base(view) {
			this.parameters = parameters;
			uiHandler = parameters.UIHandler;
		}

		public override void OnCreate() {
			View.OnTravelButtonClicked += OnTravelButtonClicked;
		}

		public override void OnClose() {
			View.OnTravelButtonClicked -= OnTravelButtonClicked;
		}

		private void OnTravelButtonClicked() {
			uiHandler.OnTravelButtonClicked?.Invoke();
		}
	}
}