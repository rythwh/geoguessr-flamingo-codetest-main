using NGame;
using NGame.Camera;
using NGame.UI;
using NPlayer;
using NShared;
using RyUI;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	[Header("Data")]
	[SerializeField] private string boardFileName;
	[SerializeField] private TileTypeList tileTypeListSO;

	[Header("Existing Objects")]
	[SerializeField] private Transform boardParent;
	[SerializeField] private Canvas canvas;
	[SerializeField] private PlayerObject playerObject;
	[SerializeField] private CameraAnchor cameraAnchor;

	public override void InstallBindings() {

		Container.BindInstance(boardFileName);
		Container.BindInstance(tileTypeListSO);
		Container.BindInstance(boardParent);
		Container.BindInstance(canvas);
		Container.BindInstance(playerObject);
		Container.BindInstance(cameraAnchor);

		Container.Bind<BoardManager>().AsSingle().NonLazy();

		Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
		Container.Bind<UIHandler>().AsSingle().NonLazy();

		Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();
		Container.Bind<PlayerProfile>().AsSingle().NonLazy();
	}
}