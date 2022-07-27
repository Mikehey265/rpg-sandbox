using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float arrowSpeed = 1;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
 
        [SerializeField] UnityEvent onHit;

        Health target = null;
        GameObject instagator = null;
        float damage = 0;

        void Start(){
            transform.LookAt(GetAimLocation());
        }
        
        void Update()
        {
            if(target == null) return;
            if(isHoming && !target.IsDead()){
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * arrowSpeed);
        }

        public void SetTarget(Health target, GameObject instagator, float damage){
            this.target = target;
            this.damage = damage;
            this.instagator = instagator;

            Destroy(gameObject, maxLifeTime);
        }

        Vector3 GetAimLocation(){
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null){
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Health>() != target) return;
            if(target.IsDead()) return;
            target.TakeDamage(instagator, damage);
            arrowSpeed = 0;
            onHit.Invoke();
            if(hitEffect != null){
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach(GameObject toDestroy in destroyOnHit){
                Destroy(toDestroy); 
            }
            Destroy(gameObject, lifeAfterImpact); 
            //transform.SetParent(target.transform);  //Attach the arrow to the enemy
            //target = null;  //clear the target so the arrow no longer moves
        }
    }
}
