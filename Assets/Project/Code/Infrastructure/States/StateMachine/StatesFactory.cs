using Zenject;

namespace Assets.Code.Scripts.Runtime.State_Machine
{
    public sealed class StatesFactory : IStatesFactory
    {
        private readonly DiContainer container;

        [Inject]
        public StatesFactory(DiContainer diContainer) =>
            this.container = diContainer;

        public T Create<T>() where T : State =>
            container.Resolve<T>();
    }
}