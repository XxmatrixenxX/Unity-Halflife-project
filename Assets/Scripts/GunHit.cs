using Photon.Pun;
using UnityEngine;

public class GunHit : MonoBehaviour
{
   public float health = 50f;
   private PhotonView myPV;
   
   void Start()
   {
      myPV = transform.gameObject.GetComponent<PhotonView>();
   }
   public void TakeDamage(float amount)
   {
      myPV.RPC("RPC_TakeDamage", RpcTarget.All, amount);
     
   }

   void Die()
   { 
      transform.GetComponent<EthanControls>().StopPulling();
     transform.position = new Vector3 (1,1,96);
     health = 50f;
     transform.GetComponent<MyPlayer>().health = 1f;
     
   }
   
   [PunRPC]
   void RPC_TakeDamage(float damage)
   {
      if (!myPV.IsMine)
      {
         return;
      }
      transform.GetComponent<MyPlayer>().Hit();
      health -= damage;
      if (health <= 0f)
      {
         Die();
      }
   }
}
