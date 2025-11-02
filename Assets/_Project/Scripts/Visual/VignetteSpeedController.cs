using ProceduralGeneration;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Visual
{
    
    [RequireComponent(typeof(Volume))]
    public sealed class VignetteSpeedController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the LevelGenerator that drives chunk speed.")]
        [SerializeField]
        LevelGenerator levelGenerator;

        [Header("Vignette Settings")]
        [Tooltip("Maximum vignette intensity when LevelGenerator speed reaches MinMoveSpeed.")]
        [Range(0f, 1f)]
        [SerializeField]
        float maxVignetteIntensity = 0.6f;

        Volume volume;
        Vignette vignette;

        float baselineSpeed;
        float lastAppliedIntensity = -1f;
        float lastSeenSpeed = float.NaN;

        void Start()
        {
            if (!levelGenerator) return;
            
            baselineSpeed = levelGenerator.MoveSpeed;
            ApplyForCurrentSpeed();
        }

        void Update()
        {
            if (!levelGenerator || !vignette) return;

            float current = levelGenerator.MoveSpeed;
            if (Mathf.Approximately(current, lastSeenSpeed)) return;

            lastSeenSpeed = current;
            ApplyForCurrentSpeed();
        }

        void ApplyForCurrentSpeed()
        {
            if (!vignette || !levelGenerator) return;

            float current = levelGenerator.MoveSpeed;
            float min = levelGenerator.MinMoveSpeed;
            float baseSpd = baselineSpeed;

            float intensity;
            if (current >= baseSpd)
            {
                intensity = 0f;
            }
            else
            {
                if (Mathf.Approximately(baseSpd, min))
                {
                    intensity = maxVignetteIntensity;
                }
                else
                {
                    float t = Mathf.InverseLerp(min, baseSpd, current);
                    intensity = Mathf.Lerp(maxVignetteIntensity, 0f, t);
                }
            }

            intensity = Mathf.Clamp(intensity, 0f, maxVignetteIntensity);

            if (!Mathf.Approximately(intensity, lastAppliedIntensity))
            {
                vignette.intensity.Override(intensity);
                lastAppliedIntensity = intensity;
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            maxVignetteIntensity = Mathf.Clamp01(maxVignetteIntensity);
        }
#endif
    }
}
