using System;
using NGame.Quiz;
using RyUI;

namespace NGame.UI
{
	public class UIQuizTextAnswerButton : UIElement<UIQuizTextAnswerButtonComponent>, IUIQuizAnswerButton
	{
		private readonly Answer answer;

		public event Action<Answer> OnAnswerButtonClicked;

		public UIQuizTextAnswerButton(Answer answer) {
			this.answer = answer;
		}

		protected override void OnCreate() {
			base.OnCreate();

			Component.Button.onClick.AddListener(() => OnAnswerButtonClicked?.Invoke(answer));

			Component.SetAnswerText(answer.Text);
		}

		public void SetCorrectState(Answer chosenAnswer, Answer correctAnswer, bool correct) {
			if (answer == chosenAnswer || answer == correctAnswer) {
				Component.SetCorrectState(correct);
			}
		}
	}
}