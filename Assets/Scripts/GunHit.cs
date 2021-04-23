
using UnityEngine;

public class GunHit : MonoBehaviour
{
   public float health = 50f;

   public void TakeDamage(float amount)
   {
      transform.GetComponent<MyPlayer>().Hit();
      health -= amount;
      if (health <= 0f)
      {
         Die();
      }
   }

   void Die()
   { 
      transform.GetComponent<EthanControls>().StopPulling();
     transform.position = new Vector3 (1,1,96);
     health = 50f;
     transform.GetComponent<MyPlayer>().health = 1f;
     
   }
}
