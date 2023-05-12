using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class PasswordController : MonoBehaviour
{
    public TMP_InputField passwordInputField;
    private ObjectManager objectmanager;

    private void Awake()
    {
        objectmanager = GetComponent<ObjectManager>();
    }

    public void CheckPassword()
    {
        string password = passwordInputField.text;

        if (password == "1")
        {
            Debug.Log("��ȣ�� ��ġ�մϴ�!");
            objectmanager.Activate();

        }
        else
        {
            Debug.Log("��ȣ�� ��ġ���� �ʽ��ϴ�.");
        }
    }
}