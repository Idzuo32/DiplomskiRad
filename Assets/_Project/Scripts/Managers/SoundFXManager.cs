using UnityEngine;
using Utilities;

namespace Managers
{
    public class SoundFXManager : Singleton<SoundFXManager>
    {
        [SerializeField] AudioSource soundFXObject;

        public void PlaySoundFX(AudioClip clip, Transform spawnPoint, float volume)
        {
            var audioSource = Instantiate(soundFXObject, spawnPoint.position, Quaternion.identity);
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            var clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
    }
}