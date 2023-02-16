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

//    public enum CurrentState { idle, trace, attack, patrol, p_dead }; //�ǰ�?����?�߰��ؾ��ϳ�
//    public CurrentState curState = CurrentState.idle;

//    private Transform monsterTransform;

//    private Vector3 playerPrePos;

//    private NavMeshAgent m_nvAgent;
//    private Animator m_animator;

//     ���� �����Ÿ�
//    public float traceDist = 15.0f;
//     ���� �����Ÿ�
//    public float attackDist = 3.2f;
//     ���� ���� �Ÿ�, �� �Ÿ� �ȿ� ������ �÷��̾� ���
//    public float dangerDist = 0.5f;

//     ��� ����
//    private bool p_isDead = false;

//    timeline
//    public bool startMonster = false;
//    public VideoPlayer video;


//    ����
//    [SerializeField] Transform[] m_ptPoints = null; // ���� ��ġ���� ���� �迭
//    private int m_ptPointsCnt = 0;

//    bool doPatrol = false;
//    float idleingTime = 3f;

//    void Start()
//    {
//        monsterTransform = this.gameObject.GetComponent<Transform>();

//        m_nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
//        m_animator = this.gameObject.GetComponent<Animator>();

//         ���� ����� ��ġ�� �����ϸ� �ٷ� ���� ����
//         nvAgent.destination = playerTransform.position;
//        StartCoroutine(this.CheckState());
//        StartCoroutine(this.CheckStateForAction());
//    }
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.white;
//        Gizmos.DrawWireSphere(transform.position, traceDist);
//        switch (curState)
//        {
//            case CurrentState.idle:
//                break;
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

//    IEnumerator CheckState()
//    {
//        while (!p_isDead)
//        {
//            yield return new WaitForSeconds(0.5f);
//            playerPrePos = GameObject.FindWithTag("Player").GetComponent<PlayerPreviousTransform>().ptransform;

            
//            float playerMonsterDist = Vector3.Distance(playerPrePos, monsterTransform.position); // �÷��̾�� ���� �Ÿ�

//            if (playerMonsterDist <= attackDist)
//            {
//                curState = CurrentState.attack;
//            }
//            else if (playerMonsterDist <= traceDist) // �÷��̾ Ž�� ���� �ȿ� ���� �߰�
//            {
//                curState = CurrentState.trace;


//                if (!startMonster) //���� ù���� ����
//                {
//                    video.gameObject.SetActive(true);
//                    video.Play();
//                    video.loopPointReached += CheckOver;
//                }
//                else
//                {
//                    curState = CurrentState.trace;
//                }
//                Debug.Log(playerPrePos);
//            }
//            else //����
//            {
//                curState = CurrentState.patrol;
//                idleingTime -= Time.deltaTime;
//                Debug.Log("idle");
//                if (idleingTime <= 0)
//                {
//                    idleingTime = 3f;
//                    Debug.Log("patrol");
//                    curState = CurrentState.patrol;
//                }
//            }
//            else
//            {
//                curState = CurrentState.idle;
//            }
//        }
//    }

   
//    void CheckOver(UnityEngine.Video.VideoPlayer vp)
//    {
//        Debug.Log("video end");
//        video.gameObject.SetActive(false);
//        curState = CurrentState.trace;
//        startMonster = true;
//    }

//    IEnumerator CheckStateForAction()
//    {
//        while (!p_isDead)
//        {
//            switch (curState)
//            {
//                case CurrentState.idle:
//                    m_animator.SetBool("isWalk", false);
//                    m_animator.SetBool("isRun", false);
//                    break;
//                case CurrentState.trace:
//                    Debug.Log("trace");
//                    m_nvAgent.destination = playerPrePos;
//                    Traceing();
//                    m_animator.SetBool("isWalk", false);
//                    m_animator.SetBool("isRun", true);
//                    break;
//                case CurrentState.patrol:
//                    StartCoroutine("Idleing");
//                    Patroling();
//                    m_animator.SetBool("isWalk", true);
//                    break;
//                case CurrentState.attack:
//                    m_animator.SetBool("isAttack", true); // attack �Ǹ� �÷��̾� ���???
//                    break;
//            }

//            yield return null; // �������Ӹ��� ����
//        }
//    }
//    IEnumerable Idleing()
//    {
//        float delay = 3f;
//        m_animator.SetBool("isWalk", false);
//        m_animator.SetBool("isRun", false);
//        while (delay > 0f)
//        {
//            Debug.Log("Idle " + delay);
//            yield return null;
//            delay -= Time.deltaTime;
//        }
//    }
//    void Patroling()
//    {
//        Debug.Log("point cnt " + m_ptPointsCnt);
//        if (m_nvAgent.velocity == Vector3.zero) // ai�� �ӵ��� 0�� �Ǹ� ���� ����Ʈ�� �̵�
//        {
//            m_nvAgent.SetDestination(m_ptPoints[m_ptPointsCnt++].position);

//            if (m_ptPointsCnt >= m_ptPoints.Length) //����Ʈ�� ������ ���� �ٽ� 0���� �ʱ�ȭ
//                m_ptPointsCnt = 0;
//        }
//    }
//    void Traceing()
//    {
//    }
//    void Attacking() { }
//}