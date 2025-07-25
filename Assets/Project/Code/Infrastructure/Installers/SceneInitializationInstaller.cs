using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Infrastructure.Installers
{
    public class SceneInitializationInstaller : MonoInstaller
    {
        public List<MonoBehaviour> Initializers;

        public override void InstallBindings()
        {
            foreach (MonoBehaviour initializer in Initializers)
                Container.BindInterfacesAndSelfTo(initializer.GetType()).FromInstance(initializer).AsSingle();
        }
    }
}
