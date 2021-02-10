using UnityEngine;

/*
 * Key Controller
 * Author: Johannes Bluhm
 * Assigned to cubes which serve a key function. Only a key with a group equaling the group of the button activates it.
 */

namespace LevelObjects {
    public class KeyController : MonoBehaviour {
        public int group = 0;
    }
}