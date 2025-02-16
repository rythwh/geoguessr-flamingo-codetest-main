using NBoardEditor.UI;
using RyUI;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
    public class BoardEditorInstaller : MonoInstaller
    {
        [SerializeField] private GameObject grid;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TileTypeList tileTypeListSO;

        public override void InstallBindings() {

            Container.BindInstance(grid);
            Container.BindInstance(canvas);
            Container.BindInstance(tileTypeListSO);

            Container.Bind<EditorBoardManager>().AsSingle().NonLazy();
            Container.Bind<BoardSerializer>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
            Container.Bind<UIHandler>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();
            Container.Bind<InputHandler>().AsSingle();
            Container.Bind<GridManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlacementHandler>().AsSingle();
            Container.Bind<BoardValidator>().AsSingle().NonLazy();
        }
    }
}