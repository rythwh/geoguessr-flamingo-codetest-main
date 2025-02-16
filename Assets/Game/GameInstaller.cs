using NGame;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	[SerializeField] private string boardFileName;

	public override void InstallBindings() {

		Container.BindInstance(boardFileName);

		Container.Bind<BoardManager>().AsSingle().NonLazy();
	}
}