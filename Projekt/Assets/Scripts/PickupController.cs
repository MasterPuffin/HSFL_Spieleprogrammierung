using UnityEngine;

/*
 * Pickup Controller
 * Author: Johannes Bluhm
 * Controls the different pickups
 */

public class PickupController : MonoBehaviour {
    public enum Type {
        Jump,
        Points,
        Regen
    }

    public Type type;
    private PlayerController player;

    private GameManager gameManager;

    //Sound played on pickup
    public AudioClip pickupSound;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider trigger) {
        //Only interact with the player
        if (trigger.gameObject.tag != "Player") {
            return;
        }


        switch (type) {
            case Type.Jump:
                //Enable double jump
                player.canDoubleJump = true;
                break;

            case Type.Points:
                //Reduce the players score by 20 seconds
                gameManager.score = gameManager.score - 20 < 0 ? 0 : gameManager.score - 20;
                break;

            case Type.Regen:
                //Adds 50 HP to the player
                gameManager.hp = gameManager.hp + 50 > 100 ? 100 : gameManager.hp + 50;
                break;
        }

        //Play pickup sound
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        //Destroy pickup object
        Destroy(gameObject);
    }
}