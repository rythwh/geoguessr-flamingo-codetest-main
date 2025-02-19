using NGame.Quiz;
using RyUI;

namespace NGame.UI
{
	public class UIQuizParameters : IUIParameters
	{
		public readonly QuizData QuizData;
		public readonly UIManager UIManager;
		public readonly UIHandler UIHandler;

		public UIQuizParameters(
			QuizData quizData,
			UIManager uiManager,
			UIHandler uiHandler
		) {
			QuizData = quizData;
			UIManager = uiManager;
			UIHandler = uiHandler;
		}
	}
}