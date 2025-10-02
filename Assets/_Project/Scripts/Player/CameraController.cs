using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] ParticleSystem speedupParticleSystem;
        [SerializeField] float minFOV = 20f;
        [SerializeField] float maxFOV = 120f;
        [SerializeField] float zoomDuration = 1f;
        [SerializeField] float zoomSpeedModifer = 5f;

        CinemachineCamera _cinemachineCamera;

        void Awake()
        {
            _cinemachineCamera = GetComponent<CinemachineCamera>();
        }

        public void ChangeCameraFOV(float speedAmount)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFOVRoutine(speedAmount));

            if (speedAmount > 0)
            {
                speedupParticleSystem.Play();
            }
        }

        IEnumerator ChangeFOVRoutine(float speedAmount)
        {
            var startFOV = _cinemachineCamera.Lens.FieldOfView;
            var targetFOV = Mathf.Clamp(startFOV + speedAmount * zoomSpeedModifer, minFOV, maxFOV);

            var elapsedTime = 0f;

            while (elapsedTime < zoomDuration)
            {
                var t = elapsedTime / zoomDuration;
                elapsedTime += Time.deltaTime;

                _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
                yield return null;
            }

            _cinemachineCamera.Lens.FieldOfView = targetFOV;
        }
    }
}