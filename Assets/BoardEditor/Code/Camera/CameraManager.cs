using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace NBoardEditor
{
	public class CameraManager
	{
		[Inject]
		public CameraManager() {

			SetInitialCameraSettings();
		}

		private void SetInitialCameraSettings() {

			if (!Camera.main) {
				return;
			}

			Camera.main.transform.position = new Vector3(1, 0, 1) * GridManager.GridRadius;
			Camera.main.orthographicSize = GridManager.GridRadius * 1.2f;
		}
	}
}