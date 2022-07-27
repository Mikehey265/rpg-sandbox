using System;
using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier{
            A, B, C, D, E
        }
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        void OnTriggerEnter(Collider other) {
            if(other.tag == "Player"){
                StartCoroutine(Transition());
            }
        }

        //Coroutine that runs between scenes
        IEnumerator Transition(){
            if(sceneToLoad < 0){
                Debug.LogError("Scene to load is not set");
                yield break;
            }

            //before the scene load
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            //Remove Control from player
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            //save current level
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            //remove control from player
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            //load current level
            savingWrapper.Load();

            //after the scene load
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            //restore control to player
            newPlayerController.enabled = true;

            Destroy(gameObject);
        }

        void UpdatePlayer(Portal otherPortal)
        {
            Transform player = GameObject.FindWithTag("Player").transform;
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.position = otherPortal.spawnPoint.position;
            player.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>()){  //Finding every portal in scene
                if(portal == this) continue;
                if(portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }
}
