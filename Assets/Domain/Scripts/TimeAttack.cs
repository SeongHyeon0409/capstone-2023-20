using UnityEngine;
using TMPro;
using System.Collections;



public class TimeAttack : MonoBehaviour
{


    private float done = 10.0F;

    public TMP_Text gui_text;


    // ��ü ���� �ð��� �������ش�. ���⼭�� 180��.
    float setTime = 180;

    // �д����� �ʴ����� ����� ������ ������ش�.
    int min;
    float sec;

    // Use this for initialization

    void Start()
    {


    }


    // Update is called once per frame

    void Update()
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
