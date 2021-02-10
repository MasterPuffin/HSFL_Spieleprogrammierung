using UnityEngine;

/*
 * Door Animator
 * Author: Johannes Bluhm
 * Animates the door when activated by a button
 */

public class DoorAnimator : MonoBehaviour {
    //Relative Vector for the movement of the door
    public Vector3 targetDir = new Vector3(0, -1, 0);

    //Target position, calculated by the current position and the target vector
    private Vector3 targetPos;

    //Original position to revert the animation
    private Vector3 originalPos;

    //Speed for animation. Higher number = faster animation
    public float speed = 2f;

    //Skips the calculation if the door is close enough to its target. This is useful so the calculation can be skipped
    //when there is no change to improve performance.
    private bool enable = false;
    private bool opened = false;
    public AudioClip sound;

    private void Start() {
        //Get the original position
        originalPos = transform.position;
    }

    public void Raise() {
        if (!opened) {
            return;
        }
        
        //Raise the door back to the original position
        if (sound) {
            AudioSource.PlayClipAtPoint(sound, transform.position);
        }

        targetPos = originalPos;
        enable = true;
    }

    public void Lower() {
        if (opened) {
            return;
        }
        
        //Lower the door
        if (sound) {
            AudioSource.PlayClipAtPoint(sound, transform.position);
        }

        targetPos = transform.position + targetDir;
        enable = true;
    }

    void Update() {
        //Skip update when door has reached its target
        if (!enable) {
            return;
        }

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, targetPos) > 0.001f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        } else {
            //Skip the next updates until the door becomes active again
            enable = false;
            opened = !opened;
        }
    }
}