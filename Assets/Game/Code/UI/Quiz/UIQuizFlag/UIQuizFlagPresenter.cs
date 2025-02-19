using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using ModestTree;
using NGame.UI.FlagAnswerButton;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NGame.UI
{

	[UsedImplicitly]
	public class UIQuizFlagPresenter : UIQuizPresenter<UIQuizFlagView>
	{
		public UIQuizFlagPresenter(UIQuizFlagView view, UIQuizParameters parameters) : base(view, parameters) {
		}

		public override void OnCreate() {
			string[] questionWords = quizData.Question.Split(' ');
			string questionHeader = questionWords.SkipLast(1).Join(" ");
			string questionTarget = questionWords.TakeLast(1).First();

			View.SetQuestion(questionHeader);
			View.SetQuestionTarget(questionTarget);

			CreateAnswerButtons().Forget();
		}

		private async UniTask CreateAnswerButtons() {
			await CreateAnswerButtons<UIQuizFlagAnswerButton>(quizData);
			SetAnswerFlags();
		}

		private void SetAnswerFlags() {
			foreach (IUIQuizAnswerButton button in answerButtons) {
				SetAnswerFlag(button as UIQuizFlagAnswerButton).Forget();
			}
		}

		private async UniTask SetAnswerFlag(UIQuizFlagAnswerButton button) {
			AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(button.Answer.ImageID);
			handle.ReleaseHandleOnCompletion();
			button.SetFlagImage(await handle);
		}
	}
}