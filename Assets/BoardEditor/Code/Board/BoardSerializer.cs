using System.IO;
using NBoardEditor.UI;
using NShared.Board;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
	public class BoardSerializer
	{
		private readonly EditorBoardManager editorBoardManager;
		private readonly BoardValidator boardValidator;
		private readonly UIHandler uiHandler;

		[Inject]
		public BoardSerializer(EditorBoardManager editorBoardManager, BoardValidator boardValidator, UIHandler uiHandler) {
			this.editorBoardManager = editorBoardManager;
			this.boardValidator = boardValidator;
			this.uiHandler = uiHandler;

			uiHandler.OnLoadButtonClicked += OnLoadButtonClicked;
			uiHandler.OnSaveButtonClicked += OnSaveButtonClicked;
		}

		private void OnLoadButtonClicked() {
			DeserializeBoard();
		}

		private void OnSaveButtonClicked(string boardName) {
			SerializeBoard(editorBoardManager.BoardData, boardName);
		}

		private void SerializeBoard(BoardData boardData, string boardName) {

			if (!boardValidator.ValidateBoard()) {
				return;
			}

			if (!Directory.Exists(BoardData.BoardsFolderPath)) {
				Directory.CreateDirectory(BoardData.BoardsFolderPath);
			}

			string boardPath = Path.Combine(BoardData.BoardsFolderPath, $"{boardName}.json");
			boardData.PrepareForSerialization();
			string boardJson = JsonUtility.ToJson(boardData, true);
			File.WriteAllText(boardPath, boardJson);

			Debug.Log($"Board serialized at: {boardPath}");
		}

		private void DeserializeBoard() {
			string boardFile = EditorUtility.OpenFilePanel("Select Board File", BoardData.BoardsFolderPath, "json");

			if (string.IsNullOrEmpty(boardFile)) {
				Debug.LogWarning("No file selected");
				return;
			}

			string json = File.ReadAllText(boardFile);
			BoardData boardData = JsonUtility.FromJson<BoardData>(json);
			boardData.RestoreAfterDeserialization();

			editorBoardManager.RecreateBoard(boardData);

			uiHandler.OnBoardLoaded?.Invoke(Path.GetFileNameWithoutExtension(boardFile));

			Debug.Log($"Board loaded from: {boardFile}");
		}
	}
}