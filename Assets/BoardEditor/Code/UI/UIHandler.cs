using System;
using Cysharp.Threading.Tasks;
using RyUI;
using UnityEngine;
using Zenject;

namespace NBoardEditor.UI
{
	public class UIHandler
	{
		public Action<TileType> OnTileTypeSelected;
		public Action<bool, string> OnBoardValidationUpdated;

		[Inject]
		public UIHandler(UIManager uiManager, Canvas canvas, TileTypeList tileTypeList) {
			uiManager.canvas = canvas.transform;

			uiManager.OpenViewAsync<UIBoardEditor>(null, new UIBoardEditorParameters(tileTypeList, this), true, false).Forget();
		}
	}
}