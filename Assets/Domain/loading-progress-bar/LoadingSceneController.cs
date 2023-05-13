using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class LoadingSceneController : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private TMP_Text tip;
    [SerializeField]
    private TMP_Text explain;
    [SerializeField]
    private GameObject[] BGI;


    static int nextLevel;
    //Ÿ��Ʋ, ����, ü����, ������, ���ӿ�����, �ε��� ������ ����
    static string[] levels = { "MainTitle","PhotonTest-KKB" ,"MainBuilding", "Stage2", "Stage3","" };
    static string[] tips = { 
        "2������ �÷��̰� �����մϴ�.", 
        "����� �̷��� �б��� ����Ǿ� �ִ� �ǰ�..?", 
        "�� �̷��� ü������ ���� �κη��� ����?..", 
        "������ ���� �� ��ߵɱ�...", 
         };
    static string[] explains = { 
        "Ÿ��Ʋ�� �̵� ��...", 
        "�������� ���� ��..", 
        "ü�������� ���� ��..", 
        "�ǹ��� �����Ƿ� ���� ��.."
    };

    public static void LoadScene()
    {
        PhotonNetwork.LoadLevel("LoadingScene");

    }

    void Start()
    {
        nextLevel = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentLevel"];
        Debug.Log("nextlevel == " + nextLevel);
        tip.text = "Tips : " + tips[nextLevel];
        explain.text = explains[nextLevel];
        BGI[nextLevel - 1].SetActive(true);
        
        if (PhotonNetwork.IsMasterClient)
        {
            LoadStage();
        }
    }

    void LoadStage()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {

        PhotonNetwork.LoadLevel(levels[nextLevel]);
        while (PhotonNetwork.LevelLoadingProgress < 1f)
        {
            progressBar.fillAmount = PhotonNetwork.LevelLoadingProgress;
            yield return null;
        }
        BGI[nextLevel - 1].SetActive(false);
    }
}
