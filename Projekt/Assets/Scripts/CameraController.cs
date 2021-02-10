using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Camera Movement
 * Author: Johannes Bluhm
 * Controls all aspects of the camera movement
 * Based on work by: Unity
 * https://learn.unity.com/tutorial/moving-the-camera
 */

public class CameraController : MonoBehaviour {
    public GameObject player;
    //Offset between the player and the camera
    private Vector3 offset;
    
    private GameManager gameManager;
    //Disables the camera movement eg. for use in menus
    public bool disableMovement = false;

    //Angle of the camera, initial value is starting value
    public float angle = 30;
    //If the player should move independent from the cameras current rotation
    public bool freecam = true;

    //Rotation of the camera around the player
    //This has to be a float somehow...
    private float rotatingDir = 0;

    //Changes the camera angle
    private void OnScroll(InputValue scrollValue) {
        Vector2 scrollVector = scrollValue.Get<Vector2>();

        //Ignore if the value is 0, as the method is always called twice with 120/-120 and 0
        //Set upper scroll limit
        if (scrollVector.y > 0 && offset.y + 1.0f < 20) {
            offset.y += 1.0f;
            //Check if value will be greater 0 to prevent the camera from clipping into the ground
        } else if (scrollVector.y < 0 && offset.y - 1.0f > 0) {
            offset.y -= 1.0f;
        }
    }

    //Changes the rotation of the camera on keypress
    private void OnRotate(InputValue inputValue) {
        rotatingDir = (float) inputValue.Get();
    }

    //Toggles between freecam and normal mode
    private void OnToggle() {
        freecam = !freecam;
        gameManager.ToggleFreecamIndicator();
    }

    // Start is called before the first frame update
    private void Start() {
        //Get the current offset
        offset = transform.position - player.transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void LateUpdate() {
        //Skip update if movement is disabled
        if (disableMovement) {
            return;
        }
        //Get the angle for rotation
        angle += rotatingDir * Time.deltaTime * 100;
        Vector3 tOffset = Quaternion.AngleAxis(angle, Vector3.up) * offset;
        transform.position = player.transform.position + tOffset;
        transform.LookAt(player.transform);
    }
}