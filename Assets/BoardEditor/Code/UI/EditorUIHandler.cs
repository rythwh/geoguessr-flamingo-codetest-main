using System;
using Cysharp.Threading.Tasks;
using NShared;
using RyUI;
using UnityEngine;
using Zenject;

namespace NBoardEditor.UI
{
	public class EditorUIHandler
	{
		// This is mostly needed because my UI framework is not setup for DI, so I can't inject to Presenters, so I need a bridge between it and the non-UI code for events
		public Action<TileType> OnTileTypeSelected;
		public Action<bool, string> OnBoardValidationUpdated;

		public Action OnLoadButtonClicked;
		public Action<string> OnSaveButtonClicked;
		public Action<string> OnBoardLoaded;

		[Inject]
		public EditorUIHandler(UIManager uiManager, Canvas canvas, TileTypeList tileTypeList) {
			uiManager.canvas = canvas.transform;

			uiManager.OpenViewAsync<UIBoardEditor>(null, new UIBoardEditorParameters(tileTypeList, this), true, false).Forget();
		}
	}
}