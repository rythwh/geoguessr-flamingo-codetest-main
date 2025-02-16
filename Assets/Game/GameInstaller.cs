using NBoardEditor;
using NGame;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	[SerializeField] private string boardFileName;
	[SerializeField] private TileTypeList tileTypeListSO;
	[SerializeField] private Transform boardParent;

	public override void InstallBindings() {

		Container.BindInstance(boardFileName);
		Container.BindInstance(tileTypeListSO);
		Container.BindInstance(boardParent);

		Container.Bind<BoardManager>().AsSingle().NonLazy();
	}
}