using Cysharp.Threading.Tasks;

namespace Assets.Code.Scripts.Runtime.State_Machine
{
    public interface IPayloadState<TPayload>
    {
        UniTask OnEnterAsync(TPayload payload);
    }
}