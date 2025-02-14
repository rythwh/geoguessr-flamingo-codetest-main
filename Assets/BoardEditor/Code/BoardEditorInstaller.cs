using System;
using RyUI;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
    public class BoardEditorInstaller : MonoInstaller
    {
        [SerializeField] private GameObject grid;

        public override void InstallBindings() {

            Container.BindInstance(grid);

            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();
            Container.Bind<InputHandler>().AsSingle();
            Container.Bind<GridManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlacementHandler>().AsSingle();
        }
    }
}