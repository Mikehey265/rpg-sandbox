using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        void Awake() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        void Update() {
            GetComponent<Text>().text = ("Health: " + (int)health.GetHealthPoints() + "/" + (int)health.GetMaxHealthPoints());
        }
    }
}
