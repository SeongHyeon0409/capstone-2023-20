using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipController : MonoBehaviour
{
    public string NextSceneName;

    private void Awake()
    {
      
    }
    public void SkipButtonClicked()
    {
        // ���� �÷��� ��ŵ ���� �ۼ�
        // ��: ���� ������ ��ȯ
        if (PhotonNetwork.IsMasterClient) LoadingSceneController.LoadScene();
    }
}
