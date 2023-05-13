using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.Cockpit.Forms;
using StarterAssets;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks { 
    public  PhotonView pv;
    private GameObject LocalPlayer;
    [SerializeField]
    private GameObject CloseGameMsg;

 
    
    private void Awake()
    {
        
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameOver();
        }
    }

    //���ӿ��������� �̵��ϴ� �ۺ��Լ�(�װų�, �ð��ʰ����� �� �Լ� ȣ��)
    public void GameOver()
    {
        pv.RPC("MoveGameOverScene", RpcTarget.All);
    }
    [PunRPC]
    private void MoveGameOverScene()
    {
        LocalPlayer.GetComponent<StarterAssetsInputs>().PlayerMoveLock(); //���콺 Ŀ�� �ǵ���.
        PhotonNetwork.Destroy(LocalPlayer);
        if(PhotonNetwork.IsMasterClient) GameOverManager.LoadGameOver();
    }
    public void OnLevelWasLoaded(int level)
    {
        if(level == 1 || (bool)PhotonNetwork.CurrentRoom.CustomProperties["GameOver"] ) 
        {
            CreatePlayer(PhotonNetwork.LocalPlayer);
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            if (cp.ContainsKey("GameOver")) cp.Remove("GameOver");
            cp.Add("GameOver", false);
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
            
        }
    }
    private void CreatePlayer(Player player)
    {
        Transform pos = GameObject.Find("SpwanPoint" + player.NickName).transform;
        LocalPlayer = PhotonNetwork.Instantiate("Player" + player.NickName, pos.position, pos.rotation);
        LocalPlayer.GetComponent<ThirdPlayerController>().virtualCamera.Priority += 10;
        DontDestroyOnLoad(LocalPlayer);
        Debug.Log(LocalPlayer.name + " �����Ϸ� ũ����Ʈ�÷��̾�");
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CloseGameMsg.SetActive(true);
        Invoke("LeaveRoom",3f);
    }
    
    public void LeaveRoom()
    {
        LocalPlayer.GetComponent<StarterAssetsInputs>().PlayerMoveLock(); //���콺 Ŀ�� �ǵ���.
        Debug.Log(LocalPlayer.name + " �ı��ϰ� �� ������");
        PhotonNetwork.LeaveRoom();
        Destroy(LocalPlayer);
        SceneManager.LoadScene(0);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("cause : " + cause.ToString());
    }


    //���� �ε��Ҷ� ���Ǵ� �Լ� ==> ���� ������ �������� ������ �����Ǹ� �Ʒ� �Լ��� ȣ�����ָ� �˴ϴ� �����ؼ� ������ּ���
    /*  
    void func(){
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            int nextLevel = (int)cp["CurrentLevel"] + 1;
            if (cp.ContainsKey("CurrentLevel")) cp.Remove("CurrentLevel"); //�浹 ���� Ȯ���ϰ� ������ ������Ʈ �ϱ� ����;
            cp.Add("CurrentLevel", nextLevel);
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("CurrentLevel"))
            {
                LoadingSceneController.LoadScene();
            }
    }
    */
}
