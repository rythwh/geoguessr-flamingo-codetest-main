using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NBoardEditor.UI.Elements;
using NShared;
using RyUI;

namespace NBoardEditor.UI
{

	[UsedImplicitly]
	public class UIBoardEditorPresenter : UIPresenter<UIBoardEditorView>
	{
		private readonly TileTypeList tileTypeList;
		private readonly EditorUIHandler editorUIHandler;

		public UIBoardEditorPresenter(UIBoardEditorView view, UIBoardEditorParameters parameters) : base(view) {
			tileTypeList = parameters.TileTypeList;
			editorUIHandler = parameters.EditorUIHandler;

			CreateTileTypeButtons().Forget();
			editorUIHandler.OnBoardValidationUpdated += UpdateBoardValidationStatus;
			View.OnLoadButtonClicked += LoadButtonClicked;
			View.OnSaveButtonClicked += SaveButtonClicked;
			editorUIHandler.OnBoardLoaded += OnBoardLoaded;

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
			editorUIHandler.OnTileTypeSelected?.Invoke(tileType);
		}

		private void UpdateBoardValidationStatus(bool passed, string message) {
			View.SetBoardValidationStatus(passed, message);
		}

		private void LoadButtonClicked() {
			editorUIHandler.OnLoadButtonClicked?.Invoke();
		}

		private void SaveButtonClicked(string boardName) {
			editorUIHandler.OnSaveButtonClicked?.Invoke(boardName);
		}

		private void OnBoardLoaded(string boardName) {
			View.SetBoardName(boardName);
		}

		public override void OnClose() {
			base.OnClose();

			editorUIHandler.OnBoardValidationUpdated -= UpdateBoardValidationStatus;
			View.OnLoadButtonClicked -= LoadButtonClicked;
			View.OnSaveButtonClicked -= SaveButtonClicked;
		}
	}
}