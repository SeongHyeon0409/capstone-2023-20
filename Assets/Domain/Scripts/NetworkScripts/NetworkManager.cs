using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit.Forms;

public class NetworkManager : MonoBehaviourPunCallbacks { 
    public PhotonView pv;


    private void Awake()
    {
        //������ Ŭ���̾�Ʈ�� ������ �ڵ� ����ȭ
        //PhotonNetwork.AutomaticallySyncScene = true;
        CreatePlayer(PhotonNetwork.LocalPlayer);
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties.ToString());
    }
    private void CreatePlayer(Player player)
    {
        Debug.Log(GameObject.Find("Player" + player.NickName).ToString());
        //GameObject.Find("Player" + player.NickName).GetComponent<PhotonView>().RequestOwnership();
        GameObject.Find("Player" + player.NickName).GetComponent<ThirdPlayerController>().virtualCamera.Priority += 10;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreatePlayer(newPlayer);
    }
}
