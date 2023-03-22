using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class MonsterController : MonoBehaviour
{

    public enum CurrentState { idle, trace, attack, patrol, dead };
    public CurrentState curState = CurrentState.idle;

    private Transform _transform;
    private Transform playerTransform;
    private NavMeshAgent nvAgent;
    private Animator _animator;

    // ���� �����Ÿ�
    public float traceDist = 15.0f;
    // ���� �����Ÿ�
    public float attackDist = 3.2f;

    // ��� ����
    private bool isDead = false;
    private bool isIdle = true;
    private float IdleTIme = 3f;
    private float chkTime = 0f;

    //���� �þ�
    public AiSensor Sensor;
    bool inSight = false;

    [SerializeField] Transform[] m_ptPoints = null; // ���� ��ġ���� ���� �迭
    int m_ptPointsCnt = 0;

    //view
    MonsterView view;

    //debug
    public GameObject alarm;

    void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        _animator = this.gameObject.GetComponent<Animator>();
        view = GameObject.Find("Monster").GetComponent<MonsterView>();
        Sensor = GameObject.Find("MonsterSensor").GetComponent<AiSensor>();

        // ���� ����� ��ġ�� �����ϸ� �ٷ� ���� ����
        // nvAgent.destination = playerTransform.position;

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, traceDist);
        switch (curState)
        {
            case CurrentState.trace:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, 1f);
                break;
            case CurrentState.patrol:
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, 3f);
                break;
        }
    }
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.5f);

            //float dist = Vector3.Distance(playerTransform.position, _transform.position);
            inSight = Sensor.isInSight;

            if (inSight)
            {
                Debug.Log("trace");
                curState = CurrentState.trace;
            }
            else
            {
                curState = CurrentState.patrol;
            }

        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case CurrentState.idle:
                    _animator.SetBool("isIdle", true);
                    break;
                case CurrentState.trace:
                    nvAgent.speed = 10f;
                    nvAgent.destination = playerTransform.position;
                    _animator.SetBool("isRun", true);
                    break;
                case CurrentState.patrol:
                    nvAgent.speed = 1f; // �ִ� �̵� �ӵ�
                    Patroling();
                    break;
            }

            yield return null;
        }
    }
    void Patroling()
    {
        if (nvAgent.remainingDistance < 2f)
        {
            alarm.SetActive(true);
            //Debug.Log("patrol_idle");
            _animator.SetBool("isIdle", true);
            chkTime += Time.deltaTime;
        }
        if (chkTime > IdleTIme)
        {
            alarm.SetActive(false);
            Debug.Log(chkTime);
            chkTime = 0;
            _animator.SetBool("isWalk", true);
            if (nvAgent.remainingDistance < 2f)
            {
                nvAgent.SetDestination(m_ptPoints[m_ptPointsCnt].position);
                Debug.Log(m_ptPointsCnt);
                m_ptPointsCnt++;

                if (m_ptPointsCnt >= m_ptPoints.Length) //����Ʈ�� ������ ���� �ٽ� 0���� �ʱ�ȭ
                    m_ptPointsCnt = 0;
                if (!nvAgent.pathPending)
                {
                    if (nvAgent.remainingDistance <= nvAgent.stoppingDistance)
                    {
                        if (!nvAgent.hasPath || nvAgent.velocity.sqrMagnitude == 0f)
                        {
                            curState = CurrentState.idle;
                        }
                    }
                }
            }
        }
    }
    //void MakeWorldBounds()
    //{
    //    WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
    //    Vector3 min = worldBounds.min.position;
    //    Vector3 max = worldBounds.max.position;

    //    Vector3 randomPosition = new Vector3{
    //Random.Range(min.x, max.x),
    //Random.Range(min.y, max.y),
    //Random.Range(min.z, max.z) // �ٵ� �츮���� ���� �������� ���� �𸣰ڴ�
    //}
        
}