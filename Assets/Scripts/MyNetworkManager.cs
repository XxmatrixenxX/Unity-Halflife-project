using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class MyNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name , new Vector3(1,1,96),Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(4,1,96),Quaternion.identity);
        }
        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(1,1,96),Quaternion.identity);
    }
}

