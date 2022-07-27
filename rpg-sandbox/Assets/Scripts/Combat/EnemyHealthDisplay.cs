using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        void Awake() {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update() {
            if(fighter.GetTarget() == null){
                GetComponent<Text>().text = "Enemy: N/A";
                return;
            }
            Health health = fighter.GetTarget();
            GetComponent<Text>().text = ("Enemy: " + (int)health.GetHealthPoints() + "/" + (int)health.GetMaxHealthPoints());
            
        }
    }
}
