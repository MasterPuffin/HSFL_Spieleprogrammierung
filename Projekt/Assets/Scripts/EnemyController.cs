using UnityEngine;

/*
 * Enemy Controller
 * Author: Johannes Bluhm
 * Sets the default damage for an enemy
 */

public class EnemyController : MonoBehaviour {
    public enum Type {
        Spikes,
        Ball,
        Tower,
        Bullet,
        Lava
    }

    public Type type;
    //Sound played at contact with player
    public AudioClip hitSound;
    
    //If set to 0, default damage will be used
    public int damage = 0;

    // Start is called before the first frame update
    void Start() {
        //Set default damage if no custom damage is set
        if (damage == 0) {
            switch (type) {
                case Type.Spikes:
                    damage = 50;
                    break;
                case Type.Ball:
                    damage = 50;
                    break;
                case Type.Tower:
                    damage = 50;
                    break;
                case Type.Bullet:
                    damage = 50;
                    break;
                case Type.Lava:
                    damage = 1000;
                    break;
            }
        }
    }
}