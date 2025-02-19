using System;
using System.IO;
using NShared.Board;
using UnityEngine;

namespace NGame.Quiz
{
	[Serializable]
	public class QuizData
	{
		public static readonly string QuizFolderPath = Path.Combine(Application.streamingAssetsPath, "QuizData");

		public string ID;
		public QuestionType QuestionType;
		public string Question;
		public string CustomImageID;
		public Answer[] Answers;
		public int CorrectAnswerIndex;

		public static QuestionType? EquateTileTypeToQuestionType(TileTypeEnum tileType) {
			return tileType switch {
				TileTypeEnum.Quiz => QuestionType.Text,
				TileTypeEnum.QuizFlag => QuestionType.Flag,
				_ => null
			};
		}

		public Answer GetCorrectAnswer() {
			return Answers[CorrectAnswerIndex];
		}

		public bool CheckAnswer(Answer answer) {
			return answer == GetCorrectAnswer();
		}

		public override string ToString() {
			return $"{nameof(ID)}: {ID}, {nameof(QuestionType)}: {QuestionType}, {nameof(Question)}: {Question}, {nameof(CustomImageID)}: {CustomImageID}, {nameof(Answers)}: {Answers}, {nameof(CorrectAnswerIndex)}: {CorrectAnswerIndex}";
		}
	}
}