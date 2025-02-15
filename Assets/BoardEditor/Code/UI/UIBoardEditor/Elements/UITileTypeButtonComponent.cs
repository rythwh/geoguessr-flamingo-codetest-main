using System;
using RyUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NBoardEditor.UI.Elements
{
	public class UITileTypeButtonComponent : UIElementComponent
	{
		[SerializeField] private Image image;
		[SerializeField] private TMP_Text tileTypeNameText;

		[SerializeField] private Button button;

		public event Action OnButtonClicked;

		public override void OnCreate() {
			base.OnCreate();

			button.onClick.AddListener(() => OnButtonClicked?.Invoke());
		}

		protected override void OnClose() {
			base.OnClose();

			button.onClick.RemoveListener(() => OnButtonClicked?.Invoke());
		}

		public void SetImage(Sprite sprite) {
			image.sprite = sprite;
		}

		public void SetText(string tileTypeName) {
			tileTypeNameText.SetText(tileTypeName);
		}
	}
}