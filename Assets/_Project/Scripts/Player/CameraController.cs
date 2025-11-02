using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraController : MonoBehaviour
    {
        
        [SerializeField] float minFOV = 20f;
        [SerializeField] float maxFOV = 120f;
        [SerializeField] float zoomDuration = 1f;
        [SerializeField] float zoomSpeedModifer = 5f;

        CinemachineCamera cinemachineCamera;

        void Awake()
        {
            cinemachineCamera = GetComponent<CinemachineCamera>();
        }

        public void ChangeCameraFOV(float speedAmount)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFOVRoutine(speedAmount));
        }

        IEnumerator ChangeFOVRoutine(float speedAmount)
        {
            var startFOV = cinemachineCamera.Lens.FieldOfView;
            var targetFOV = Mathf.Clamp(startFOV + speedAmount * zoomSpeedModifer, minFOV, maxFOV);

            var elapsedTime = 0f;

            while (elapsedTime < zoomDuration)
            {
                var t = elapsedTime / zoomDuration;
                elapsedTime += Time.deltaTime;

                cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
                yield return null;
            }

            cinemachineCamera.Lens.FieldOfView = targetFOV;
        }
    }
}