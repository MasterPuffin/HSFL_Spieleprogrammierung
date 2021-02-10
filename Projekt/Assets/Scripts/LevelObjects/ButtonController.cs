using System.Collections.Generic;
using UnityEngine;

/*
 * Button Controller
 * Author: Johannes Bluhm
 * Controls the level element button and what happens on interaction with an object or player.
 */

namespace LevelObjects {
    public class ButtonController : MonoBehaviour {
        //Material for when the button if off
        public Material materialOn;
        //Material for when the button is on
        public Material materialOff;
        //The GameObject on top of the button which indicates its state
        private Renderer indicator;
        //The group for the button. If != 0 the button needs a key with the same group to activate
        public int group = 0;
        //List of objects touching the button. Used to keep track if the button should show as activated or not
        private List<Collider> TriggerList = new List<Collider>();
        //GO with the button destroys / hides
        public GameObject target;
        //Sound played when the GO is destroyed. Only used when Action is destroy
        public AudioClip destroySound;
        //As above but the particle system spawned
        public GameObject destroyParticlePrefab;
        //What should happen with the target object?
        public enum Action {
            Destroy,
            Disable,
            Nothing
        }

        public Action action;

        private bool destroyed = false;

        // Start is called before the first frame update
        void Start() {
            //Set indicator to deactivated
            indicator = gameObject.transform.GetChild(0).GetComponent<Renderer>();
            indicator.sharedMaterial = materialOff;
        }

        private void OnTriggerEnter(Collider trigger) {
            //Check if key is valid for this button
            KeyController kc = trigger.GetComponent<KeyController>();

            //Check if kc is a keycontroller if not and the button needs a key return
            //Check if kc is valid for this button
            if (group != 0 && (kc == null || kc.group != group)) {
                return;
            }

            //Add object to list of objects touching the button
            if (!TriggerList.Contains(trigger)) {
                TriggerList.Add(trigger);
            }

            //Activate the on material. 
            if (TriggerList.Count == 1) {
                indicator.sharedMaterial = materialOn;
            }
            
            //Do the action
            switch (action) {
                case Action.Destroy:
                    if (destroyed) {
                        break;
                    }
                    Destroy(target);
                    destroyed = true;
                    //Spawn destroy prefab
                    if (destroyParticlePrefab) {
                        Instantiate(destroyParticlePrefab,
                            new Vector3(target.transform.position.x, target.transform.position.y + 1,
                                target.transform.position.z), Quaternion.identity);
                    }

                    //Play destroy sound
                    if (destroySound) {
                        AudioSource.PlayClipAtPoint(destroySound, transform.position);
                    }

                    break;
                case Action.Disable:
                    //Animate the door sliding down
                    target.GetComponent<DoorAnimator>().Lower();
                    break;
            }
        }

        private void OnTriggerExit(Collider trigger) {
            //Remove object from list
            if (TriggerList.Contains(trigger)) {
                TriggerList.Remove(trigger);
            }

            //Check if nothing is touching the button anymore
            if (TriggerList.Count == 0) {
                indicator.sharedMaterial = materialOff;

                switch (action) {
                    case Action.Disable:
                        //Raise door
                        target.GetComponent<DoorAnimator>().Raise();
                        break;
                }
            }
        }
    }
}