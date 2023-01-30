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

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(CamShake(0.15f, 5f));
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

    public IEnumerator CamShake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }
}
