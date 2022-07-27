using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool wasTriggered = false;
        void OnTriggerEnter(Collider other) {
            if(!wasTriggered && other.gameObject.tag == "Player"){
                GetComponent<PlayableDirector>().Play();
                wasTriggered = true;
            }
        }

    }
}
