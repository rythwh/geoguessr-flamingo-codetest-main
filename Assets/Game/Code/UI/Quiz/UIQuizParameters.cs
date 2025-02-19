using NGame.Quiz;
using RyUI;

namespace NGame.UI
{
	public class UIQuizParameters : IUIParameters
	{
		public readonly QuizData QuizData;
		public readonly UIManager UIManager;

		public UIQuizParameters(
			QuizData quizData,
			UIManager uiManager
		) {
			QuizData = quizData;
			UIManager = uiManager;
		}
	}
}