using System;
using Cysharp.Threading.Tasks;
using NGame.Player;
using RyUI;
using UnityEngine;
using Zenject;

namespace NGame.UI
{
	public class UIHandler
	{
		public Action OnTravelButtonClicked;
		public Action<bool> OnInputBlockChanged;
		public Action<int> OnQuizCompleted;
		public Action<int> OnPlayerRolled;

		[Inject]
		public UIHandler(
			UIManager uiManager,
			Canvas canvas,
			PlayerProfile playerProfile
		) {
			uiManager.canvas = canvas.transform;

			uiManager.OpenViewAsync<UITopBar>(null, new UITopBarParameters(playerProfile, this), true, false).Forget();
			uiManager.OpenViewAsync<UITravelButton>(null, new UITravelButtonParameters(this), true, false).Forget();
		}
	}
}