using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace NBoardEditor
{
	public class CameraManager : IDisposable
	{
		private readonly InputHandler inputHandler;
		private readonly GridManager gridManager;

		[Inject]
		public CameraManager(InputHandler inputHandler, GridManager gridManager) {
			this.inputHandler = inputHandler;
			this.gridManager = gridManager;

			inputHandler.Actions.Editor.Zoom.performed += OnZoomPerformed;

			SetInitialCameraSettings();
		}

		private void SetInitialCameraSettings() {

			if (!Camera.main) {
				return;
			}

			Camera.main.transform.position = new Vector3(1, 0, 1) * GridManager.GridRadius;
			Camera.main.orthographicSize = GridManager.GridRadius * 1.2f;
		}

		private void OnZoomPerformed(InputAction.CallbackContext context) {
			if (!Camera.main) {
				return;
			}

			// Camera.main.orthographicSize += context.ReadValue<float>();
		}

		public void Dispose() {
			inputHandler.Actions.Editor.Zoom.performed -= OnZoomPerformed;
		}
	}
}