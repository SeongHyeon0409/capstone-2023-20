using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void OnDrawGizoms() //������ �׻� ����
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one); //���簢�� �׸���
    }

    void OnDrawGizmosSelected() //������Ʈ�� ���õ��� �� ����
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 3f); //�� �׸���
    }
}
