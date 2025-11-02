using ProceduralGeneration;
using UnityEngine;

namespace UI
{
    public class SpeedIndicatorUI : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Transform of the needle sprite (RectTransform or Transform) that should rotate around Z.")]
        [SerializeField]
        Transform needleTransform;

        [Tooltip("Reference to the active LevelGenerator that controls chunk move speed.")] [SerializeField]
        LevelGenerator levelGenerator;

        [Header("Needle Rotation Settings")]
        [Tooltip("Minimum angle (degrees) when at min speed. For a half-circle starting at left, use 0.")]
        [SerializeField]
        float minAngle = 0f;

        [Tooltip("Maximum angle (degrees) when at max speed. For a half-circle ending at right, use 180.")]
        [SerializeField]
        float maxAngle = 180f;

        [Tooltip("Smoothing time for the needle rotation in seconds.")] [SerializeField, Range(0f, 0.5f)]
        float smoothTime = 0.1f;

        float currentAngle;
        float angularVelocity;
        
        void Update()
        {
            if (!needleTransform || !levelGenerator) return;
            
            var t = Mathf.InverseLerp(levelGenerator.MinMoveSpeed, levelGenerator.MaxMoveSpeed, levelGenerator.MoveSpeed);
            var targetAngle = Mathf.Lerp(minAngle, maxAngle, t);

            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angularVelocity, smoothTime);

            var euler = needleTransform.eulerAngles;
            euler.z = currentAngle;
            needleTransform.eulerAngles = euler;
        }
    }
}
