using NShared;
using RyUI;

namespace NBoardEditor.UI
{
	public class UIBoardEditorParameters : IUIParameters
	{
		public TileTypeList TileTypeList;
		public EditorUIHandler EditorUIHandler;

		public UIBoardEditorParameters(TileTypeList tileTypeList, EditorUIHandler editorUIHandler) {
			TileTypeList = tileTypeList;
			EditorUIHandler = editorUIHandler;
		}
	}
}