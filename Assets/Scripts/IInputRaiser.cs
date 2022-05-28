using System;
using UnityEngine;

public interface IInputRaiser
{
    event Action<Vector2> FingerDown;
    event Action<float> Swerve;
}
