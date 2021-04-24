using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GunSwitch : MonoBehaviourPunCallbacks
{
   private PhotonView myPV;
   [SerializeField] private Item[] items;

   private int itemIndex = 0;
   private int previousItemIndex = -1;

   void Start()
   {
      myPV = transform.parent.parent.gameObject.GetComponent<PhotonView>();
      EquipItem(itemIndex);
   }
   void Update()
   {
      if (myPV.IsMine)
      {
         if (Input.GetAxis("Mouse ScrollWheel") > 0f)
         {
            if (itemIndex >= transform.childCount - 1)
               itemIndex = 0;
            else
               itemIndex++;
         }

         if (Input.GetAxis("Mouse ScrollWheel") < 0f)
         {
            if (itemIndex <= 0)
               itemIndex = transform.childCount - 1;
            else
               itemIndex--;
         }

         if (Input.GetKeyDown(KeyCode.Alpha1))
         {
            itemIndex = 0;
         }

         if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
         {
            itemIndex = 1;
         }

         if (previousItemIndex != itemIndex)
         {
            EquipItem(itemIndex);
         }
      }
   }
   
   public void EquipItem(int _index)
   {
      if (_index == previousItemIndex)
         return;
      itemIndex = _index;
      items[itemIndex].itemGameObject.SetActive(true);

      if (previousItemIndex != -1)
      {
         items[previousItemIndex].itemGameObject.SetActive(false);
      }

      previousItemIndex = itemIndex;

      if (myPV.IsMine)
      {
         Hashtable hash = new Hashtable();
         hash.Add("itemIndex", itemIndex);
         PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
      }
   }

   public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
   {
      if (!myPV.IsMine && targetPlayer == myPV.Owner)
      {
         EquipItem((int)changedProps["itemIndex"]);
      }
      
   }
}
