using UnityEngine;

/*
 * Key Controller
 * Author: Johannes Bluhm
 * Controls the tower enemy
 */

public class TowerController : MonoBehaviour {
    private GameObject player;

    //Prefabs for the spawned bullet
    public GameObject bulletPrefab;
    public GameObject impactPrefab;

    //Max distance for the player to fire the bullet
    public float maxDistance = 10f;

    public float timeOffset = 0;

    //Time passed since last shot
    private float timePassed;

    //Timeout between shots
    public float timeOut = 3.0f;

    //Offset for spawning the bullet
    public Vector3 spawnOffset = new Vector3(0, 0, 0);

    //Sound for when the bullet is fired
    public AudioClip soundFire;

    //Audio source for the turn sound of the turret
    private AudioSource audioSource;

    void Start() {
        timePassed = timeOffset;
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    void Update() {
        timePassed += Time.deltaTime;

        //Measure the distance to the player
        if (Vector3.Distance(transform.position, player.transform.position) < maxDistance) {
            
            //Track the player
            transform.LookAt(player.transform);
            
            if (timePassed >= timeOut) {
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }

                if (soundFire) {
                    AudioSource.PlayClipAtPoint(soundFire, transform.position);
                }

                timePassed = 0;

                //Spawn bullet prefab
                GameObject bullet =
                    Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.identity);
                Vector3 force = new Vector3(
                    player.transform.position.x - transform.position.x,
                    //Add 2 to simulate a ballistic trajectory
                    player.transform.position.y - transform.position.y + 2,
                    player.transform.position.z - transform.position.z
                );

                //Propel the bullet
                bullet.GetComponent<Rigidbody>().AddForce(force * 100);

                //Set parent object, otherwise the bullet will despawn immediately  
                bullet.GetComponent<BulletController>().parent = gameObject;
            }
        } else {
            //Mute the audio source when the player is to far away
            if (audioSource.isPlaying) {
                StartCoroutine(AudioFade.StartFade(audioSource, 1f, 0f, audioSource.volume));
            }
        }
    }
}