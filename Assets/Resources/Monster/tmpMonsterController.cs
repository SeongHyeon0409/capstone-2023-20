//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.Playables;
//using UnityEngine.Video;

//public class tmpMonsterController : MonoBehaviour
//{

//    public enum CurrentState { patrol, trace, attack, p_dead }; //�ǰ�?����?�߰��ؾ��ϳ�
//    public CurrentState curState = CurrentState.patrol;

//    private Transform monsterTransform;

//    private Vector3 playerPrePos;

//    private NavMeshAgent m_nvAgent;
//    private Animator m_animator;

//    // ���� �����Ÿ�
//    public float traceDist = 15.0f;
//    // ���� �����Ÿ�
//    public float attackDist = 3.2f;
//    // ���� ���� �Ÿ�, �� �Ÿ� �ȿ� ������ �÷��̾� ���
//    public float dangerDist = 0.5f;

//    // ��� ����
//    private bool p_isDead = false;

//    //timeline
//    //public bool startMonster = false;
//    //public VideoPlayer video;


//    //����
//    [SerializeField] Transform[] m_ptPoints = null; // ���� ��ġ���� ���� �迭
//    private int m_ptPointsCnt = 0;

//    float idleingTime = 3f;
//    bool isTrace = false;
//    bool isPatrolIdle = false;

//    void Start()
//    {
//        monsterTransform = this.gameObject.GetComponent<Transform>();

//        m_nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
//        m_animator = this.gameObject.GetComponent<Animator>();

//        // ���� ����� ��ġ�� �����ϸ� �ٷ� ���� ����
//        // nvAgent.destination = playerTransform.position;
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.white;
//        Gizmos.DrawWireSphere(transform.position, traceDist);
//        switch (curState)
//        {
//            case CurrentState.trace:
//                Gizmos.color = Color.red;
//                Gizmos.DrawWireSphere(transform.position, 1f);
//                break;
//            case CurrentState.patrol:
//                Gizmos.color = Color.green;
//                Gizmos.DrawWireSphere(transform.position, 3f);
//                break;
//        }
//    }
//    int ChkDist()
//    {
//        playerPrePos = GameObject.FindWithTag("Player").GetComponent<PlayerPreviousTransform>().ptransform;

//        //
//        float playerMonsterDist = Vector3.Distance(playerPrePos, monsterTransform.position); // �÷��̾�� ���� �Ÿ�

//        if (playerMonsterDist <= attackDist)
//        {
//            curState = CurrentState.attack;
//            return 2;
//        }
//        else if (playerMonsterDist <= traceDist) // �÷��̾ Ž�� ���� �ȿ� ���� �߰�
//        {
//            curState = CurrentState.trace;
//            return 1;

//        }
//        else 
//        {
//            curState = CurrentState.patrol;
//            return 0;
//        }; //����
//    }
//    enum state { patrol, attack, trace };
//    void Update()
//    {
//        while (!p_isDead)
//        {
//            int state = ChkDist();
//        }
//    }

   
//    //void CheckOver(UnityEngine.Video.VideoPlayer vp)
//    //{
//    //    Debug.Log("video end");
//    //    video.gameObject.SetActive(false);
//    //    curState = CurrentState.trace;
//    //    startMonster = true;
//    //}

//    IEnumerator CheckStateForAction()
//    {
//        Debug.Log("chk action");
//        while (!p_isDead)
//        {
//            switch (curState)
//            {
//                case CurrentState.trace:
//                    Debug.Log("trace");
//                    m_nvAgent.destination = playerPrePos;
//                    m_animator.SetBool("isWalk", false);
//                    m_animator.SetBool("isRun", true);
//                    break;
//                case CurrentState.patrol:
//                    m_animator.SetBool("isWalk", false);
//                    m_animator.SetBool("isRun", false);
//                    while (ChkDist() == 0)
//                    {
//                        m_animator.SetBool("isWalk", false);
//                        m_animator.SetBool("isRun", false);
//                        float delay = 3f;
//                        while (delay > 0f)
//                        {
//                            yield return null;
//                            delay -= Time.deltaTime;
//                        }
//                        Debug.Log("Idle end");
//                        m_animator.SetBool("isWalk", true);
//                        m_nvAgent.SetDestination(m_ptPoints[m_ptPointsCnt++].position);
//                        bool isArrived = false;
//                        while (!isArrived)
//                        {
//                            if (!m_nvAgent.pathPending)
//                            {
//                                if (m_nvAgent.remainingDistance <= m_nvAgent.stoppingDistance)
//                                {
//                                    if (!m_nvAgent.hasPath || m_nvAgent.velocity.sqrMagnitude == 0f)
//                                    {
//                                        isArrived = true;
//                                        m_ptPointsCnt++;
//                                        if (m_ptPointsCnt >= m_ptPoints.Length) //����Ʈ�� ������ ���� �ٽ� 0���� �ʱ�ȭ
//                                            m_ptPointsCnt = 0;
//                                    }
//                                }
//                            }
//                        }
//                        Debug.Log("end patrol");
//                    }
//                    break;
//                case CurrentState.attack:
//                    //m_animator.SetBool("isAttack", true); // attack �Ǹ� �÷��̾� ���???
//                    break;
//            }

//            yield return null; // �������Ӹ��� ����
//        }
//    }


//}