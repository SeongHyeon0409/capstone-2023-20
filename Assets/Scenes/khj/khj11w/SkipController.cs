using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SkipController : MonoBehaviourPunCallbacks
{
    public string NextSceneName;


    public void OnLevelWasLoaded(int level)
    {
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.Instantiate("SkipBtn",new Vector3(683,384,0),new Quaternion(0f,0f,0f,0f));
    }

    public void SkipButtonClicked()
    {
        // ���� �÷��� ��ŵ ���� �ۼ�
        // ��: ���� ������ ��ȯ
        if (PhotonNetwork.IsMasterClient) LoadingSceneController.LoadScene();
        else Debug.Log("������ �ƴմϴ�. ��ŵ�ȵ˴ϴ�.213����������������");
    }
}
