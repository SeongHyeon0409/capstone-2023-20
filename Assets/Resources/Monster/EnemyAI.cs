using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    NavMeshAgent m_enemy = null;

    [SerializeField] Transform[] m_ptPoints = null; // ���� ��ġ���� ���� �迭
    int m_ptPointsCnt = 0;

    void Start()
    {
        m_enemy = GetComponent<NavMeshAgent>();
        InvokeRepeating("MovetoNextWayPoint", 0f, 2f); // �����ϸ� 2�ʸ��� �ݺ�
    }
    void MoveToNextWayPoint()
    {
        if (m_enemy.velocity == Vector3.zero) // ai�� �ӵ��� 0�� �Ǹ� ���� ����Ʈ�� �̵�
        {
            m_enemy.SetDestination(m_ptPoints[m_ptPointsCnt++].position);

            if (m_ptPointsCnt >= m_ptPoints.Length) //����Ʈ�� ������ ���� �ٽ� 0���� �ʱ�ȭ
                m_ptPointsCnt = 0;
        }
    }

    void Update()
    {
        
    }
}
