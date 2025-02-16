using UnityEngine;

namespace NShared.Board
{
	public interface ITileObject
	{
		GameObject gameObject { get; }
		void SetImage(Sprite sprite);
	}
}