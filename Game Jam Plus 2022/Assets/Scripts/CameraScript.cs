using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Vector2 cameraRange = new(75, 100);
    [SerializeField] float cameraDistance;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cam;
    void Start()
    {
        cameraDistance = cameraRange.x;
        cam.m_Lens.FieldOfView = cameraDistance;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Minus))
        {
            ChangeValue(2);
        }
        if (Input.GetKey(KeyCode.Equals))
        {
            ChangeValue(-2);
        }
    }

    void ChangeValue(float _newValue)
    {
        cameraDistance += _newValue;     
        if (cameraDistance > cameraRange.y)
        {
            cameraDistance = cameraRange.y;
        }
        if (cameraDistance < cameraRange.x)
        {
            cameraDistance = cameraRange.x;
        }
        cam.m_Lens.FieldOfView = cameraDistance;
    }
}
