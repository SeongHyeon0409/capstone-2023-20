using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.Cockpit.Forms;
using StarterAssets;

public class NetworkManager : MonoBehaviourPunCallbacks { 
    public  PhotonView pv;
    private GameObject LocalPlayer;
    private void Awake()
    {
        
    }

    public void OnLevelWasLoaded(int level)
    {
        if(level ==1 ) 
        {
            CreatePlayer(PhotonNetwork.LocalPlayer);

        }
    }
    private void CreatePlayer(Player player)
    {
        Transform pos = GameObject.Find("SpwanPoint" + player.NickName).transform;
        LocalPlayer = PhotonNetwork.Instantiate("Player" + player.NickName, pos.position, pos.rotation);
        LocalPlayer.transform.GetChild(5).GetChild(0).gameObject.GetComponent<Option>().SetInputSystem(LocalPlayer.GetComponent<StarterAssetsInputs>()); //�����÷��̾��� starterAssetsInputs Option�� ����;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        LeaveRoom();
    }

    public void LeaveRoom()
    {
        Debug.Log("���Ӿ�������");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }


  
}
