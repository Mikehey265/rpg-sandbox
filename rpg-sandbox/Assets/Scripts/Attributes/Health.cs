using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{   
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;

        LazyValue<float> healthPoints;
        bool isDead = false;

        void Awake() {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth(){
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Start() {
            healthPoints.ForceInit();
        }

        void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead(){
            return isDead;
        }
        
        public void TakeDamage(GameObject instagator, float damage){
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if(healthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instagator);
            }else{
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float healthToRestore){
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        public float GetHealthPoints(){
            return healthPoints.value;
        }

        public float GetMaxHealthPoints(){
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage(){
            return 100 * GetFraction();
        }

        public float GetFraction(){
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        void AwardExperience(GameObject instagator)
        {
            Experience experience = instagator.GetComponent<Experience>();
            if(experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if(healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}