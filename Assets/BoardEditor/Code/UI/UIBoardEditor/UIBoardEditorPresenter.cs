using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NBoardEditor.UI.Elements;
using RyUI;

namespace NBoardEditor.UI
{

	[UsedImplicitly]
	public class UIBoardEditorPresenter : UIPresenter<UIBoardEditorView>
	{
		private readonly TileTypeList tileTypeList;
		private readonly UIHandler uiHandler;

		public UIBoardEditorPresenter(UIBoardEditorView view, UIBoardEditorParameters parameters) : base(view) {
			tileTypeList = parameters.TileTypeList;
			uiHandler = parameters.UIHandler;

			CreateTileTypeButtons().Forget();
			uiHandler.OnBoardValidationUpdated += UpdateBoardValidationStatus;
			View.OnLoadButtonClicked += LoadButtonClicked;
			View.OnSaveButtonClicked += SaveButtonClicked;
			uiHandler.OnBoardLoaded += OnBoardLoaded;

			View.SetBoardName($"board{DateTime.Now:yyyyMMddHHmmss}");
		}

		private async UniTaskVoid CreateTileTypeButtons() {
			foreach (TileType tileType in tileTypeList.tileTypes) {
				UITileTypeButton tileTypeButton = await View.CreateTileTypeButton(tileType);
				tileTypeButton.OnButtonClicked += TileTypeButtonClicked;
			}
			TileTypeButtonClicked(tileTypeList.tileTypes.First());
		}

		private void TileTypeButtonClicked(TileType tileType) {
			uiHandler.OnTileTypeSelected?.Invoke(tileType);
		}

		private void UpdateBoardValidationStatus(bool passed, string message) {
			View.SetBoardValidationStatus(passed, message);
		}

		private void LoadButtonClicked() {
			uiHandler.OnLoadButtonClicked?.Invoke();
		}

		private void SaveButtonClicked(string boardName) {
			uiHandler.OnSaveButtonClicked?.Invoke(boardName);
		}

		private void OnBoardLoaded(string boardName) {
			View.SetBoardName(boardName);
		}

		public override void OnClose() {
			base.OnClose();

			uiHandler.OnBoardValidationUpdated -= UpdateBoardValidationStatus;
			View.OnLoadButtonClicked -= LoadButtonClicked;
			View.OnSaveButtonClicked -= SaveButtonClicked;
		}
	}
}