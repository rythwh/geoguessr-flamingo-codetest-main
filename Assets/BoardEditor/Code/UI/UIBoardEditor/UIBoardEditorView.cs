using System;
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

		public Button loadButton;
		public TMP_InputField boardNameInputField;
		public Button saveButton;

		public event Action OnLoadButtonClicked;
		public event Action<string> OnSaveButtonClicked;

		public override void OnOpen() {
			loadButton.onClick.AddListener(() => OnLoadButtonClicked?.Invoke());
			saveButton.onClick.AddListener(() => OnSaveButtonClicked?.Invoke(boardNameInputField.text));
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

		public void SetBoardName(string boardName) {
			boardNameInputField.SetTextWithoutNotify(boardName);
		}
	}
}