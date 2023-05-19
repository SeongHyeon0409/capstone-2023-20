using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string NextSceneName;

    private void Start()
    {
        // ���� �÷��̾� ������Ʈ ��������
        videoPlayer = GetComponent<VideoPlayer>();

        // Loop Point Reached �̺�Ʈ�� �̺�Ʈ �ڵ鷯 ���
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void OnLoopPointReached(VideoPlayer source)
    {
        // ���� ����� ������ ���� ������ ��ȯ
        SceneManager.LoadScene(NextSceneName);
    }
}
