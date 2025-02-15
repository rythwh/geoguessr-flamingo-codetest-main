using Zenject;

namespace NBoardEditor
{
	public class InputHandler
	{
		private BoardEditorInputActions boardEditorInputActions = new BoardEditorInputActions();
		public BoardEditorInputActions Actions => boardEditorInputActions;

		[Inject]
		public InputHandler() {
			boardEditorInputActions.Enable();
		}
	}
}