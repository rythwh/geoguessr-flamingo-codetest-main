using System;
using Cysharp.Threading.Tasks;
using NGame.Quiz;
using UnityEngine;

namespace NGame.UI
{
	public interface IUIQuizAnswerButton
	{
		event Action<Answer> OnAnswerButtonClicked;

		UniTask Open(Transform parent);
		void SetCorrectState(Answer chosenAnswer, Answer correctAnswer, bool correct);
	}
}