using System;
using NGame.Quiz;
using RyUI;
using UnityEngine;

namespace NGame.UI.FlagAnswerButton
{
	public class UIQuizFlagAnswerButton : UIElement<UIQuizFlagAnswerButtonComponent>, IUIQuizAnswerButton
	{
		private readonly Answer answer;
		public Answer Answer => answer;

		public event Action<Answer> OnAnswerButtonClicked;

		public UIQuizFlagAnswerButton(Answer answer) {
			this.answer = answer;
		}

		protected override void OnCreate() {
			base.OnCreate();

			Component.Button.onClick.AddListener(() => OnAnswerButtonClicked?.Invoke(answer));
		}

		public void SetFlagImage(Sprite flag) {
			Component.SetFlagImage(flag);
		}

		public void SetCorrectState(Answer chosenAnswer, Answer correctAnswer, bool correct) {
			if (answer == chosenAnswer) {
				Component.SetCorrectState(false);
			}
			if (answer == correctAnswer) {
				Component.SetCorrectState(true);
			}
		}
	}
}