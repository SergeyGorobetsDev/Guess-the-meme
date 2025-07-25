using System;
using UnityEngine;

namespace Assets.Code.Scripts.Runtime.InputSystem
{
    public interface IPlayerInputReader
    {
        event Action<Vector2> MovePerformed;
        event Action<Vector2> MoveCanceled;
        event Action<Vector2> LookPerformed;
        event Action<Vector2> LookCanceled;
    }
}