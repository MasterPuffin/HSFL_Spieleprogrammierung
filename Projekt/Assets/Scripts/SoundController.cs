using UnityEngine;

/*
 * Sound Controller
 * Author: Johannes Bluhm
 * Plays the dragging sound when the player pushes a block
 */

public class SoundController : MonoBehaviour {
    //Audio source of the block
    private AudioSource audioSource;

    //Ground detector to check if the block is touching the ground to prevent it from playing the sound while being mid-air
    private GroundDetector groundDetector;
    private Rigidbody rb;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        groundDetector = gameObject.AddComponent<GroundDetector>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        //Only play the sound if the speed is above 0.5 (Sometimes objects in Unity move by its own a tiny bit)
        if (rb.velocity.magnitude > 0.5) {
            if (!audioSource.isPlaying && groundDetector.grounded) {
                //Play the dragging sound
                audioSource.Play();
            } else if (audioSource.isPlaying && !groundDetector.grounded) {
                //The block isn't grounded anymore, fade out the sound
                StartCoroutine(AudioFade.StartFade(audioSource, 0.1f, 0f));
            }
        } else {
            if (audioSource.isPlaying) {
                //The block isn't moving anymore, fade out the sound
                StartCoroutine(AudioFade.StartFade(audioSource, 0.1f, 0f));
            }
        }
    }
}