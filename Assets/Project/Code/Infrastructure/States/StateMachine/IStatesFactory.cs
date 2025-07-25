namespace Assets.Code.Scripts.Runtime.State_Machine
{
    public interface IStatesFactory
    {
        T Create<T>() where T : State;
    }
}