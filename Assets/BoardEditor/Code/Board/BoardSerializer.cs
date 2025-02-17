using System;
using System.IO;
using NBoardEditor.UI;
using NShared.Board;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
	public class BoardSerializer : IDisposable
	{
		private readonly EditorBoardManager editorBoardManager;
		private readonly BoardValidator boardValidator;
		private readonly EditorUIHandler editorUIHandler;

		[Inject]
		public BoardSerializer(EditorBoardManager editorBoardManager, BoardValidator boardValidator, EditorUIHandler editorUIHandler) {
			this.editorBoardManager = editorBoardManager;
			this.boardValidator = boardValidator;
			this.editorUIHandler = editorUIHandler;

			editorUIHandler.OnLoadButtonClicked += OnLoadButtonClicked;
			editorUIHandler.OnSaveButtonClicked += OnSaveButtonClicked;
		}

		public void Dispose() {
			editorUIHandler.OnLoadButtonClicked -= OnLoadButtonClicked;
			editorUIHandler.OnSaveButtonClicked -= OnSaveButtonClicked;
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

			if (boardData.OrderedTiles == null || boardData.OrderedTiles.Count == 0) {
				editorBoardManager.DetermineBoardDirection();
			}

			string boardPath = Path.Combine(BoardData.BoardsFolderPath, $"{boardName}.json");
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

			editorUIHandler.OnBoardLoaded?.Invoke(Path.GetFileNameWithoutExtension(boardFile));

			Debug.Log($"Board loaded from: {boardFile}");
		}
	}
}