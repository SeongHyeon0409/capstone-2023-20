using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    private int stage;
    private PhotonView pv;
    [SerializeField]
    private TMP_Text stateText;
    private void Awake()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties
            (new Hashtable { { "InGame", false } });
        PhotonNetwork.LocalPlayer.SetCustomProperties
            (new Hashtable { { "GameReady", false } });
    }
    private void Update()
    {
        
    }

    void OnClickRestart()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties
    (new Hashtable { { "GameReady", true } });

        bool gameStart = true;
        foreach (int id in PhotonNetwork.CurrentRoom.Players.Keys)
        {
            if (!(bool)PhotonNetwork.CurrentRoom.Players[id].CustomProperties["GameReady"]) gameStart = false;
        }
        if (gameStart)
        {
            Debug.Log("���ӽ�ŸƮ");
            pv.RPC("SetState", RpcTarget.All, "<color=yellow>�������� �絵��</color>");
            Hashtable rp = PhotonNetwork.CurrentRoom.CustomProperties;
            rp["InGame"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(rp);
            pv.RPC("Load", RpcTarget.All);
            StartCoroutine(Count());
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.NickName.Equals("Latifa")) SetStateText("�����̸� ��ٸ��� ��..");
            else SetStateText("�¿��̸� ��ٸ��� ��..");
        }
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(3.0f);
    }

    private void Load()
    {
        LoadingSceneController.LoadScene(stage);
    }

    [PunRPC]
    void SetStateText(string arg)
    {
        stateText.text = arg;
    }

    [PunRPC]
    void SetStage(int idx)
    {
        stage = idx;
    }

}
