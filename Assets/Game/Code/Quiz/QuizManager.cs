using System;
using System.Collections.Generic;
using System.IO;
using NGame.Player;
using NGame.UI;
using NShared.Board;
using RyUI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace NGame.Quiz
{
	public class QuizManager : IDisposable
	{
		private readonly PlayerController playerController;
		private readonly UIManager uiManager;
		private readonly PlayerProfile playerProfile;
		private readonly UIHandler uiHandler;

		private Dictionary<QuestionType, List<QuizData>> quizzes = new();
		private Dictionary<QuestionType, List<QuizData>> usedQuizzes = new();

		[Inject]
		public QuizManager(PlayerController playerController, UIManager uiManager, PlayerProfile playerProfile, UIHandler uiHandler) {
			this.playerController = playerController;
			this.uiManager = uiManager;
			this.playerProfile = playerProfile;
			this.uiHandler = uiHandler;

			DeserializeQuizzes();

			playerController.OnPlayerMovedToTile += OnPlayerMovedToTile;

			uiHandler.OnQuizCompleted += playerProfile.AddCoins;
		}

		private void DeserializeQuizzes() {
			foreach (QuestionType questionType in Enum.GetValues(typeof(QuestionType))) {
				string quizFile = Path.Combine(QuizData.QuizFolderPath, $"{questionType}QuizData.json");
				string json = File.ReadAllText(quizFile);
				QuizData quizData = JsonUtility.FromJson<QuizData>(json);
				if (!quizzes.TryAdd(questionType, new List<QuizData>() { quizData })) {
					quizzes[questionType].Add(quizData);
				}
			}
		}

		// Get a random quiz without using a previously used quiz (until all quizzes have been used)
		private QuizData GetQuiz(QuestionType questionType) {
			List<QuizData> quizzesForQuestionType = quizzes[questionType];
			if (quizzesForQuestionType.Count == 0) {
				quizzesForQuestionType.AddRange(usedQuizzes[questionType]);
				usedQuizzes[questionType].Clear();
			}
			int quizIndex = Random.Range(0, quizzesForQuestionType.Count);
			QuizData selectedQuiz = quizzesForQuestionType[quizIndex];
			if (!usedQuizzes.TryAdd(questionType, new List<QuizData>() { selectedQuiz })) {
				usedQuizzes[questionType].Add(selectedQuiz);
			}
			quizzesForQuestionType.RemoveAt(quizIndex);
			return selectedQuiz;
		}

		private void OnPlayerMovedToTile(Tile tile) {
			QuestionType? questionTypeNullable = QuizData.EquateTileTypeToQuestionType(tile.TileType);
			if (!questionTypeNullable.HasValue) {
				return;
			}
			OpenQuiz(questionTypeNullable.Value);
		}

		private void OpenQuiz(QuestionType questionType) {
			QuizData quiz = GetQuiz(questionType);
			if (quiz == null) {
				Debug.LogError($"Quiz is null. This should never happen unless there are no quizzes for questionType: {questionType}");
				return;
			}

			UIQuizParameters parameters = new UIQuizParameters(quiz, uiManager, uiHandler);
			switch (questionType) {
				case QuestionType.Text:
					uiManager.OpenViewAsync<UIQuizText>(null, parameters, true, false);
					break;
				case QuestionType.Flag:
					uiManager.OpenViewAsync<UIQuizFlag>(null, parameters, true, false);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(questionType), questionType, null);
			}
		}

		public void Dispose() {
			playerController.OnPlayerMovedToTile -= OnPlayerMovedToTile;
		}
	}
}