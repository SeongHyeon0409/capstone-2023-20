using UnityEngine;
using TMPro;
using System.Collections;



public class TimeAttack : MonoBehaviour
{

    private float done = 10.0F;

    public TMP_Text gui_text;


    // ��ü ���� �ð��� �������ش�.
    float setTime = 180;

    int min;
    float sec;

    // Ÿ�̸� ����
    public bool gameActive;

    // Use this for initialization

    void Start()
    {
        gameActive = false;

    }


    // Update is called once per frame

    void Update()
    {
        if (gameActive)
        {
            // ���� �ð��� ���ҽ����ش�.
            setTime -= Time.deltaTime;

            // ��ü �ð��� 60�� ���� Ŭ ��
            if (setTime >= 60f)
            {
                // 60���� ������ ����� ���� �д����� ����
                min = (int)setTime / 60;
                // 60���� ������ ����� �������� �ʴ����� ����
                sec = setTime % 60;
                // UI�� ǥ�����ش�
                gui_text.text = "���� �ð� : " + min + "��" + (int)sec + "��";
            }

            // ��ü�ð��� 60�� �̸��� ��
            if (setTime < 60f)
            {
                // �� ������ �ʿ�������Ƿ� �ʴ����� ������ ����
                gui_text.text = "���� �ð� : " + (int)setTime + "��";
            }

            // ���� �ð��� 0���� �۾��� ��
            if (setTime <= 0)
            {
                // UI �ؽ�Ʈ�� 0�ʷ� ������Ŵ.
                gui_text.text = "���� �ð� : 0��";
            }
        }
    }

    public void StartGame()
    {
        gameActive = true;
    }
}
