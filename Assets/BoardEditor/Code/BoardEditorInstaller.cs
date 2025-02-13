using System;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
    public class BoardEditorInstaller : MonoInstaller
    {
        [SerializeField] private GameObject grid;

        public override void InstallBindings() {

            Container.BindInstance(grid);

            Container.Bind<InputHandler>().AsSingle();

            Container.Bind<GridManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlacementHandler>().AsSingle();
        }
    }
}