using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NGame.Quiz;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NGame.UI
{

	[UsedImplicitly]
	public class UIQuizTextPresenter : UIQuizPresenter<UIQuizTextView>
	{
		public UIQuizTextPresenter(UIQuizTextView view, UIQuizParameters parameters) : base(view, parameters) {
		}

		public override void OnCreate() {

			View.SetTitle("Europe");

			LoadQuestionImage(quizData).Forget();
			CreateAnswerButtons<UIQuizTextAnswerButton>(quizData).Forget();
		}

		private async UniTask LoadQuestionImage(QuizData quizData) {
			AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(quizData.CustomImageID);
			handle.ReleaseHandleOnCompletion();
			View.SetImage(await handle);
		}
	}
}