using System;
using UnityEngine;
using static UnityEngine.Mathf;

public class PlayerInput : MonoBehaviour, IInputRaiser
{
    private Touch touch;
    private float touchLastPosition, mouseLastPosition;

    [SerializeField]
    private float sensitivity = 0.1f;

    public event Action<Vector2> FingerDown = delegate { };
    public event Action<float> Swerve = delegate { };

    private float intendedSwerve, realizedSwerve, accumulatedSwerve;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchLastPosition = touch.position.x;
                FingerDown(touch.position);
            }

            else if(touch.phase == TouchPhase.Moved)
            {
                intendedSwerve = (touch.position.x - touchLastPosition) * sensitivity;

                if (Sign(accumulatedSwerve) == Sign(intendedSwerve))
                {
                    realizedSwerve = Clamp(intendedSwerve + accumulatedSwerve, -1, 1);
                    accumulatedSwerve -= realizedSwerve;
                }
                else
                {
                    realizedSwerve = Clamp(intendedSwerve, -1, 1);
                    accumulatedSwerve = intendedSwerve - realizedSwerve;
                }
                Swerve(realizedSwerve);
                touchLastPosition = touch.position.x;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseLastPosition = Input.mousePosition.x;
                FingerDown(Input.mousePosition);
            }

            else if (Input.GetMouseButton(0))
            {
                //Swerve(Mathf.Clamp((Input.mousePosition.x - mouseLastPosition) * sensitivity, -1, 1));
                intendedSwerve = (Input.mousePosition.x - mouseLastPosition) * sensitivity;

                if (Sign(accumulatedSwerve) == Sign(intendedSwerve))
                {
                    realizedSwerve = Clamp(intendedSwerve + accumulatedSwerve, -1, 1);
                    accumulatedSwerve -= realizedSwerve;
                }
                else
                {
                    realizedSwerve = Clamp(intendedSwerve, -1, 1);
                    accumulatedSwerve = intendedSwerve - realizedSwerve;
                }
                Swerve(realizedSwerve);
                mouseLastPosition = Input.mousePosition.x;
            }
        }

    }
}
