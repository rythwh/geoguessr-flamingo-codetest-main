using System;
using RyUI;
using UnityEngine;
using UnityEngine.UI;

namespace NGame.UI
{
	public class UITravelButtonView : UIView
	{

		[SerializeField] private Button travelButton;

		public event Action OnTravelButtonClicked;

		public override void OnOpen() {
			travelButton.onClick.AddListener(() => OnTravelButtonClicked?.Invoke());
		}

		public override void OnClose() {
			travelButton.onClick.RemoveListener(() => OnTravelButtonClicked?.Invoke());
		}

		public void SetTravelButtonInteractable(bool interactable) {
			travelButton.interactable = interactable;
		}
	}
}