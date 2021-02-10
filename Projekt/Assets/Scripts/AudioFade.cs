using System.Collections;
using UnityEngine;

/*
* Fade out sound
* Author: John Leonard French
* https://gamedevbeginner.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/#first_method
*/

public class AudioFade : MonoBehaviour {
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, float resetVolume = 0) {
        float currentTime = 0;
        float start = audioSource.volume;
        if (resetVolume == 0) {
            resetVolume = audioSource.volume;
        }

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = resetVolume;

        yield break;
    }
}