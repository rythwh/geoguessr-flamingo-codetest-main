using RyUI;

namespace NBoardEditor.UI
{
	public class UIBoardEditorParameters : IUIParameters
	{
		public TileTypeList TileTypeList;
		public UIHandler UIHandler;

		public UIBoardEditorParameters(TileTypeList tileTypeList, UIHandler uiHandler) {
			TileTypeList = tileTypeList;
			UIHandler = uiHandler;
		}
	}
}