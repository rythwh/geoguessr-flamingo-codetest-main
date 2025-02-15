using System;
using RyUI;
using UnityEngine;

namespace NBoardEditor.UI.Elements
{
	public class UITileTypeButton : UIElement<UITileTypeButtonComponent>
	{
		private readonly TileType tileType;

		public event Action<TileType> OnButtonClicked;

		public UITileTypeButton(TileType tileType) {
			this.tileType = tileType;
		}

		protected override void OnCreate() {
			base.OnCreate();

			Component.SetImage(tileType.tileImage);
			Component.SetText(tileType.tileType.ToString());

			Component.OnButtonClicked += ButtonClicked;
		}

		protected override void OnClose() {
			base.OnClose();

			Component.OnButtonClicked -= ButtonClicked;
		}

		private void ButtonClicked() {
			OnButtonClicked?.Invoke(tileType);
		}
	}
}