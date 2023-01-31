using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System.Cam
{
    public class CameraScript : MonoBehaviour
    {
        [SerializeField] Vector2 cameraRange = new(75, 100);
        [SerializeField] float cameraDistance;
        [SerializeField] Cinemachine.CinemachineVirtualCamera cam;
        Cinemachine.CinemachineFramingTransposer transposer;
        [SerializeField] Vector2 camRangeX;
        [SerializeField] Vector2 camRangeY;
        void Start()
        {
            cameraDistance = cameraRange.x;
            cam.m_Lens.FieldOfView = cameraDistance;
            transposer = cam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();
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
                StartCoroutine(CamShake(0.15f, .5f));
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
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(camRangeX.x, camRangeX.y) * magnitude;
                float y = Random.Range(camRangeY.x, camRangeY.y) * magnitude;

                SetScreenPos(transposer, x, y);
                elapsed += Time.deltaTime;
                yield return 0;
            }
            SetScreenPos(transposer, 0, 0);
        }

        void SetScreenPos(Cinemachine.CinemachineFramingTransposer _transposer, float _x, float _y)
        {
            _transposer.m_TrackedObjectOffset = new(_x, _y, 0);
        }

    }
}