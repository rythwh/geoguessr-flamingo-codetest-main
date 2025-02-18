using System;
using Cysharp.Threading.Tasks;
using RyUI;
using UnityEngine;
using Zenject;

namespace NGame.UI
{
	public class UIHandler
	{
		public Action OnTravelButtonClicked;
		public Action<bool> OnInputBlockChanged;

		[Inject]
		public UIHandler(UIManager uiManager, Canvas canvas) {
			uiManager.canvas = canvas.transform;

			uiManager.OpenViewAsync<UITravelButton>(null, new UITravelButtonParameters(this), true, false).Forget();
		}
	}
}