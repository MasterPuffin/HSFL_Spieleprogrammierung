using UnityEngine;

/*
 * Bullet Controller
 * Author: Johannes Bluhm
 * Responsible for collision detection of the bullet. Damage of the bullet is handled by the enemy controller as the
 * bullet is technically an enemy
 */

public class BulletController : MonoBehaviour {
    public GameObject parent;
    public GameObject impactPrefab;

    private void OnCollisionEnter(Collision hit) {
        //Check if the collision is with the parent element. This can happen at spawn.
        if (hit.gameObject != parent) {
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}