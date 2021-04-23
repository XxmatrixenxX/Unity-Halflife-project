using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MyNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(1,1,96),Quaternion.identity);
    }
}

