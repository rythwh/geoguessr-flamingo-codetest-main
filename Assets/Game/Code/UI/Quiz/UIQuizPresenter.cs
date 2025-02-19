using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NGame.Quiz;
using RyUI;
using UnityEngine;

namespace NGame.UI
{

	[UsedImplicitly]
	public class UIQuizPresenter<TView> : UIPresenter<TView> where TView : UIQuizView
	{
		protected readonly UIQuizParameters parameters;
		protected readonly QuizData quizData;

		protected readonly List<IUIQuizAnswerButton> answerButtons = new();

		private const int CorrectReward = 10000;
		private const int IncorrectReward = 1000;

		public UIQuizPresenter(TView view, UIQuizParameters parameters) : base(view) {
			this.parameters = parameters;
			quizData = parameters.QuizData;
		}

		public override void OnCreate() {
			ApplyQuizData();
		}

		private void ApplyQuizData() {
			View.SetQuestion(quizData.Question);
		}

		protected async UniTask CreateAnswerButtons<TButton>(QuizData quizData) where TButton : class, IUIQuizAnswerButton {
			foreach (Answer answer in quizData.Answers) {
				if (Activator.CreateInstance(typeof(TButton), answer) is not TButton answerButton) {
					Debug.LogError("Answer button is null or invalid.");
					continue;
				}
				answerButton.OnAnswerButtonClicked += OnAnswerSelected;
				answerButtons.Add(answerButton);
				await answerButton.Open(View.AnswerButtonsLayoutGroup.transform);
			}
		}

		private async void OnAnswerSelected(Answer selectedAnswer) {
			bool correct = quizData.CheckAnswer(selectedAnswer);
			foreach (IUIQuizAnswerButton answerButton in answerButtons) {
				answerButton.SetCorrectState(selectedAnswer, quizData.GetCorrectAnswer(), correct);
			}
			await UniTask.WaitForSeconds(1);
			TransitionToPostAnswer(selectedAnswer, correct);
		}

		private void TransitionToPostAnswer(Answer answer, bool correct) {

			int reward = correct ? CorrectReward : IncorrectReward;

			View.SetAnswerResultText(correct ? "Well Done!" : "You'll get it right next time!", correct);
			View.SetCorrectAnswerText(quizData.GetCorrectAnswer().Text);
			View.SetCoinsAmountText(reward);

			View.CloseButton.onClick.AddListener(() => OnCloseButtonClicked(reward));

			View.SetPostAnswerActive(true);
		}

		private void OnCloseButtonClicked(int reward) {
			parameters.UIHandler.OnQuizCompleted?.Invoke(reward);
			parameters.UIManager.CloseView(this);
		}
	}
}