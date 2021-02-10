using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Player Controller
 * Author: Johannes Bluhm
 * Controls the movement of the player
 * Based on work by: Unity
 * https://learn.unity.com/tutorial/moving-the-player
 */

public class PlayerController : MonoBehaviour {
    //Acceleration 
    public float speed = 500;

    //Height of the jump
    public float jumpheight = 30.0f;

    //Max speed
    public float maxspeed = 5.0f;

    //If the player is currently jumping
    private bool jumping = false;

    //Able to double jump?
    public bool canDoubleJump = false;

    private Rigidbody rb;

    private float movementX;
    private float movementY;
    private float movementZ;

    private GameManager gameManager;

    //Has the camera been initialized?
    private bool initializedCamera = false;
    private Camera cam;
    private CameraController cameraController;

    //Detector if the player is on the ground
    private GroundDetector groundDetector;

    //Sound played on death
    public AudioClip deathSound;


    // Start is called before the first frame update
    void Start() {
        movementZ = 0.0f;
        rb = GetComponent<Rigidbody>();

        //Init the GM
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.Init();

        groundDetector = gameObject.AddComponent<GroundDetector>();
    }

    //Movement by WASD keys
    private void OnMove(InputValue movementValue) {
        //Disable movement changes during jumping
        if (groundDetector.grounded) {
            Vector2 movementVector = movementValue.Get<Vector2>();

            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    //Jump on space key
    public void OnJump() {
        //Only jump if the player is grounded and may jump count hasn't been exceded
        if (groundDetector.grounded || (canDoubleJump && groundDetector.jumpCount < 2)) {
            //Set jumping to true to process it later in the update
            jumping = true;
            groundDetector.jumpCount++;
        }
    }

    private void FixedUpdate() {
        //Searching for the camera is performance expensive. Skip this search if the camera has been found
        if (!initializedCamera) {
            cam = Camera.current;

            //Check if camera exists, if not skip update
            if (cam == null) {
                return;
            }

            initializedCamera = true;
            cameraController = cam.GetComponent<CameraController>();
            
            //Set freecam state
            gameManager.SetFreecamIndicator(!cameraController.freecam);
        }


        //Set the Z movement to the jumpheight if the player is jumping
        if (jumping) {
            movementZ = jumpheight;
            jumping = false;
        } else {
            movementZ = 0.0f;
        }

        Vector3 movement;

        //Check if the camera is in freecam mode
        if (cameraController.freecam) {
            /*
            * Source for the next two loc:
            * Dmitrij Berg @ HSFL
            */
            Vector3 moveVecX = Vector3.ProjectOnPlane(cam.transform.right, new Vector3(0, 1, 0)).normalized * movementX;
            Vector3 moveVecZ = Vector3.ProjectOnPlane(cam.transform.forward, new Vector3(0, 1, 0)).normalized *
                               movementY;

            movement = moveVecX + moveVecZ + new Vector3(0, movementZ, 0);
        } else {
            movement = new Vector3(movementX, movementZ, movementY);
        }

        //Only apply additional force if player is below max speed
        if (Mathf.Abs(rb.velocity.x) < maxspeed && Mathf.Abs(rb.velocity.z) < maxspeed) {
            rb.AddForce(movement * (speed * Time.deltaTime));
        }
    }

    //Detect collisions with triggers
    private void OnTriggerEnter(Collider trigger) {
        //End the level when the player touches the finish tag
        if (trigger.gameObject.CompareTag("Finish")) {
            gameManager.Finish();
        }

        //End the level when the player falls out of the world
        if (trigger.gameObject.CompareTag("Respawn")) {
            if (deathSound) {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            gameManager.Crash();
        }
    }

    //Detect collisions with enemies
    private void OnCollisionEnter(Collision hit) {
        if (hit.gameObject.CompareTag("Enemy")) {
            //Get the damage from the enemy
            EnemyController enemyController = hit.gameObject.GetComponent<EnemyController>();
            gameManager.hp -= enemyController.damage;
            //Play hit sound
            AudioSource.PlayClipAtPoint(enemyController.hitSound, transform.position);
            //End the level if the player has no more hp
            if (gameManager.hp <= 0) {
                gameManager.Crash();
            }
        }
    }

    //Removes the force from the player
    public void RemoveForce() {
        rb.velocity = new Vector3(0, 0, 0);
    }
}