using JetBrains.Annotations;
using RyUI;

namespace NGame.UI
{

	[UsedImplicitly]
	public class UITravelButtonPresenter : UIPresenter<UITravelButtonView>
	{
		private readonly UIHandler uiHandler;

		public UITravelButtonPresenter(UITravelButtonView view, UITravelButtonParameters parameters) : base(view) {
			uiHandler = parameters.UIHandler;
		}

		public override void OnCreate() {
			View.OnTravelButtonClicked += OnTravelButtonClicked;
			uiHandler.OnInputBlockChanged += OnInputBlockChanged;
		}

		public override void OnClose() {
			View.OnTravelButtonClicked -= OnTravelButtonClicked;
			uiHandler.OnInputBlockChanged -= OnInputBlockChanged;
		}

		private void OnTravelButtonClicked() {
			uiHandler.OnTravelButtonClicked?.Invoke();
		}

		private void OnInputBlockChanged(bool inputBlock) {
			View.SetTravelButtonInteractable(!inputBlock);
		}
	}
}