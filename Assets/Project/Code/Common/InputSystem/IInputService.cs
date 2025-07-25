using System;
using UnityEngine;

namespace Assets.Code.Scripts.Runtime.InputSystem
{
    public interface IInputService
    {
        event Action<Vector2> LookCanceled;

        void Initialize();
        void ChangeInputMap(InputMapType inputMapType);
    }
}