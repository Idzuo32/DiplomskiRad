using System.Collections;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class SoundFXManager : Singleton<SoundFXManager>
    {
        [SerializeField] AudioSource soundFXObject;

        public void PlaySoundFX(AudioClip clip, Transform spawnPoint, float volume)
        {
            if (!clip || !soundFXObject || !spawnPoint) return;

            var go = PoolManager.Get(soundFXObject.gameObject, spawnPoint.position, Quaternion.identity);
            var audioSource = go.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                PoolManager.Release(go);
                return;
            }

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();

            var clipLength = clip.length;
            StartCoroutine(ReturnToPoolAfter(audioSource, clipLength));
        }

        IEnumerator ReturnToPoolAfter(AudioSource source, float seconds)
        {
            if (seconds > 0f)
                yield return new WaitForSeconds(seconds);
            else
                yield return null;

            if (!source) yield break;
            source.Stop();
            PoolManager.Release(source.gameObject);
        }
    }
}