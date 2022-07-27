using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        void Awake() {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        void Update() {
            GetComponent<Text>().text = ("XP: " + (int)experience.GetPoints());
        }
    }
}