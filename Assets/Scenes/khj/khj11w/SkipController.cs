using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipController : MonoBehaviour
{
    public string NextSceneName;
    public void SkipButtonClicked()
    {
        // ���� �÷��� ��ŵ ���� �ۼ�
        // ��: ���� ������ ��ȯ
        SceneManager.LoadScene(NextSceneName);
    }
}
