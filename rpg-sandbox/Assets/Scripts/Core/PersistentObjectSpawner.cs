using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentobjectPrefab;
        static bool hasSpawned = false;

        void Awake() {
            if(hasSpawned) return;
            SpawnPersistentObjects();
            hasSpawned = true;
        }

        void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentobjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
