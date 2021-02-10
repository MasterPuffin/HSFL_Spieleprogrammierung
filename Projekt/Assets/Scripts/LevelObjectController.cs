using UnityEngine;

/*
 * Level Object Controller
 * Author: Johannes Bluhm
 * Main controller for all level objects, however the button level object has it's own additional class due to its complexity
 */

public class LevelObjectController : MonoBehaviour {
    public enum Type {
        Tar,
        Button
    }
    
    public Type type;

    //Speed reduction when the player enters the tar
    private int tarSpeedModifier = 10;

    private PlayerController player;
    //Sound played on interaction
    public AudioClip sound;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider trigger) {
        switch (type) {
            case Type.Tar:
                //Make the player slow
                player.speed = player.speed / tarSpeedModifier;
                //Remove all current speed
                player.RemoveForce();
                break;
        }

        if (sound) {
            AudioSource.PlayClipAtPoint(sound, transform.position);
        }
    }

    private void OnTriggerExit(Collider trigger) {
        switch (type) {
            case Type.Tar:
                //Reset the player speed
                player.speed = player.speed * tarSpeedModifier;
                break;
        }
    }
}