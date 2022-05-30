using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform character;
    [SerializeField]
    private float smoothTime = 0.5f, lookAhead = 20;
    [SerializeField]
    private Vector3 offset, velocity, pos;
    private float charY;

    private void Awake()
    {
        velocity = Vector3.zero;
        charY = character.position.y;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, character.position + offset, ref velocity, smoothTime);

        pos = transform.position;

        transform.LookAt(new Vector3(pos.x, charY, pos.z + lookAhead));
    }
}
