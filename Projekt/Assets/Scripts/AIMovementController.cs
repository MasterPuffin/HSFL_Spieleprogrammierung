using UnityEngine;

/*
 * AI Movement
 * Author: Johannes Bluhm
 * Controls the movement of the enemy following the player
 */

public class AIMovementController : MonoBehaviour {
    private GameObject player;

    //Speed with which the player is followed
    public float followSpeed = 2;

    //Max distance to follow the player
    public float maxDistance = 10;

    //Sound when acquiring the player
    public AudioClip sound;

    //The sound should only be played on first acquisition. 
    private bool soundPlayed = false;
    private Rigidbody rb;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        //Measure the distance to the player
        if (Vector3.Distance(transform.position, player.transform.position) < maxDistance) {
            //Look at the player
            transform.LookAt(player.transform);
            
            //Play acquisition sound
            if (sound && !soundPlayed) {
                AudioSource.PlayClipAtPoint(sound, transform.position);
                soundPlayed = true;
            }

            //Move towards player
            rb.AddForce((player.transform.position - transform.position) * followSpeed);
        } else {
            soundPlayed = false;
        }
    }
}