using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class NetworkManager : MonoBehaviourPunCallbacks { 

    public  PhotonView pv;
    [SerializeField]
    private GameObject CloseGameMsg;

    [Header("FadeOut")]
    private float fadeSpeed = 2f;
    private Image fadeImage;
    private float delayTime = 1.0f;  // ��ȯ ��� �ð�




    

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

        GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponent<StarterAssetsInputs>().PlayerMoveLock(); //���콺 Ŀ�� �ǵ���.
        Debug.Log("�����÷��̾� �̸� ::" + GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).name);
        fadeImage = GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponentInChildren<FadeOut>().GetComponent<Image>();
        StartCoroutine(FadeOut());
        if(PhotonNetwork.IsMasterClient) GameOverManager.LoadGameOver();
    }
    public void OnLevelWasLoaded(int level)
    {
        if(level == 1 || (bool)PhotonNetwork.CurrentRoom.CustomProperties["GameOver"] ) 
        {
            if (!PhotonNetwork.IsMasterClient) return;
            pv.RPC("CreatePlayer", RpcTarget.All);
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            if (cp.ContainsKey("GameOver")) cp.Remove("GameOver");
            cp.Add("GameOver", false);
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
            
        }
        else if(level == 0)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    [PunRPC]
    private void CreatePlayer()
    {
        Transform pos = GameObject.Find("SpwanPoint" + PhotonNetwork.LocalPlayer.NickName).transform;
        PhotonNetwork.Instantiate("Player" + PhotonNetwork.LocalPlayer.NickName, pos.position, pos.rotation);

        GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName).GetComponent<ThirdPlayerController>().virtualCamera.Priority += 10;
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag(PhotonNetwork.LocalPlayer.NickName));
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
        /*Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        if (cp.ContainsKey("CurrentLevel")) cp.Remove("CurrentLevel"); //�浹 ���� Ȯ���ϰ� ������ ������Ʈ �ϱ� ����;
        cp.Add("CurrentLevel", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);*/
        LoadingSceneController.LoadScene();
        PhotonNetwork.LeaveRoom();
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

    IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        // �г��� ���� ���� ������ ���ҽ��� ���̵�ƿ� ȿ���� ��
        while (fadeImage.color.a < 1.0f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b,
                                           fadeImage.color.a + fadeSpeed * Time.deltaTime);
            yield return null;
        }

        // ��ȯ ��� �ð� ���� ���
        yield return new WaitForSeconds(delayTime);
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
