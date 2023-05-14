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

    static int nextLevel;

    static string[] levels = { "MainTitle", "scifi_ysh", "Stage2", "Stage3" };
    static string[] tips = { "tip1", "tip2", "tip3", "tip4", "tip5" };
    public static void LoadScene(int level)
    {
        nextLevel = level;
        PhotonNetwork.LoadLevel("LoadingScene");

    }

    void Start()
    {
        tip.text = "Tips : " + tips[nextLevel + 1];
        
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
        PhotonNetwork.LoadLevel(levels[nextLevel]);
    }

    /*IEnumerator LoadSceneProcess()
    {
        AsyncOperation op;
        //op.allowSceneActivation = false; //�ε� �ð��� �ʹ� ª���� ����(�Ƹ� ������� Ŀ�� �ʿ���� ���� ����
        while (!op.isDone)
        {
            progressBar.fillAmount = op.progress;
            yield return null;
        }
    }*/
}
