using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class NetworkManager : MonoBehaviourPunCallbacks { 

    public  PhotonView pv;
    [SerializeField]
    private GameObject CloseGameMsg;

    
    private void Awake()
    {
        var objs = FindObjectsOfType<NetworkManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
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

        GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponent<StarterAssetsInputs>().PlayerMoveLock(); //���콺 Ŀ�� �ǵ���.
        Debug.Log("�����÷��̾� �̸� ::" + GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).name);
        FadeOut();
    }
    public void OnLevelWasLoaded(int level)
    {
        // ���κ����̰ų�, ���ӿ��� ���¸鼭, ���ӿ������� �ƴ� ��츸 �÷��̾�ĳ���� ����
        if(level == 1 || ((bool)PhotonNetwork.CurrentRoom.CustomProperties["GameOver"] && level != 4) ) 
        {
            pv.RPC("CreatePlayer", RpcTarget.All);
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            if (cp.ContainsKey("GameOver")) cp.Remove("GameOver");
            cp.Add("GameOver", false);
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
            
        }
        else if(level == 0 )
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    [PunRPC]
    private void CreatePlayer()
    {
        GameObject chk = GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName);
        if (chk != null) return;
        Transform pos = GameObject.Find("SpwanPoint" + PhotonNetwork.LocalPlayer.NickName).transform;
        
        PhotonNetwork.Instantiate("Player" + PhotonNetwork.LocalPlayer.NickName, pos.position, pos.rotation);

        GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponent<ThirdPlayerController>().virtualCamera.Priority += 10;
        Debug.Log(GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).name + " �����Ϸ� ũ����Ʈ�÷��̾�");
        
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CloseGameMsg.SetActive(true);
        Invoke("LeaveRoom",3f);
    }
    
    public void LeaveRoom()
    {
        GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponent<StarterAssetsInputs>().PlayerMoveLock(); //���콺 Ŀ�� �ǵ���.
        Debug.Log(GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).name + " �ı��ϰ� �� ������");
        PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName));
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainTitle");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("cause : " + cause.ToString());
    }

    public override void OnLeftRoom()
    {
        Debug.Log("���� �������ϴ�.");
    }
    private void FadeOut()
    {
        if(GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponent<ThirdPlayerController>() != null)
        {
            GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponent<ThirdPlayerController>().FadingStart();
        }
        GameOverManager.LoadGameOver();
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
