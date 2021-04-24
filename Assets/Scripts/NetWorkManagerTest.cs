using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetWorkManagerTest : MonoBehaviourPunCallbacks
{
    //instance
    public static NetWorkManagerTest instance;

    void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            //set the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void CreatRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
