using Cysharp.Threading.Tasks;
using NBoardEditor.UI.Elements;
using RyUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NBoardEditor.UI
{
	public class UIBoardEditorView : UIView
	{
		public LayoutGroup tileTypeButtonsLayoutGroup;
		public TMP_Text boardValidationStatusText;

		public override void OnOpen() {

		}

		public override void OnClose() {

		}

		public async UniTask<UITileTypeButton> CreateTileTypeButton(TileType tileType) {
			UITileTypeButton tileTypeButton = new UITileTypeButton(tileType);
			await tileTypeButton.Open(tileTypeButtonsLayoutGroup.transform);
			return tileTypeButton;
		}

		public void SetBoardValidationStatus(bool passed, string message) {
			boardValidationStatusText.SetText(message);
			boardValidationStatusText.color = passed ? Color.green : Color.red;
		}
	}
}