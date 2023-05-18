using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using StarterAssets;

public class GameOverManager : MonoBehaviour
{
    static int stage;
    private PhotonView pv;
    [SerializeField]
    private TMP_Text stateText;

    private void Awake()
    { 
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        if (cp.ContainsKey("InGame")) cp.Remove("InGame"); //�浹 ���� Ȯ���ϰ� ������ ������Ʈ �ϱ� ����;
        if (cp.ContainsKey("GameOver")) cp.Remove("GameOver");
        cp.Add("InGame", false);
        cp.Add("GameOver", true); //���ӿ����������� �ƴ���
        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
        cp = PhotonNetwork.LocalPlayer.CustomProperties;
        if (cp.ContainsKey("GameReady")) cp.Remove("GameReady");
        cp.Add("GameReady", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
        pv = gameObject.GetPhotonView();

        //�÷��̾� ĳ���� �ı�
        PhotonNetwork.Destroy(GameObject.Find("Player" + PhotonNetwork.LocalPlayer.NickName));

    }
    private void Update()
    {
        
    }

    public void OnClickRestart()
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
            pv.RPC("SetStateText", RpcTarget.All, "<color=yellow>�������� �絵�� �غ���</color>");
            Hashtable rp = PhotonNetwork.CurrentRoom.CustomProperties;
            rp["InGame"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(rp);
            pv.RPC("Load", RpcTarget.All);
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.NickName.Equals("Latifa")) SetStateText("�����̸� ��ٸ��� ��..");
            else SetStateText("�¿��̸� ��ٸ��� ��..");
        }
    }

    [PunRPC]
    void Load()
    {
        Debug.Log("currentLevel at GameOver : " + PhotonNetwork.CurrentRoom.CustomProperties["CurrentLevel"]);
        if (PhotonNetwork.IsMasterClient) LoadingSceneController.LoadScene();
    }

    public static void LoadGameOver()
    {
        stage = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentLevel"];
        PhotonNetwork.LoadLevel("GameOver");

    }

    [PunRPC]
    void SetStateText(string arg)
    {
        stateText.text = arg;
    }


}
