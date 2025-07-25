using System;
using UnityEngine;

namespace Assets.Code.Scripts.Runtime.InputSystem
{
    public interface IUIInputReader
    {
        event Action<Vector2> NavigatePerformed;
        event Action SubmitPerformed;
        event Action CancelPerformed;
    }
}