using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//���� - Asuna�뵵 ��Ʈ�ѷ� Ŭ���� (player Ȯ��� Ŭ����)
public class ThirdPlayerController : MonoBehaviour
{
    //����� ��ü
    private PhotonView pv;
    //������� �ó׸ӽ� ī�޶�
    private CinemachineVirtualCamera virtualCamera;
    //���ӻ����϶� �ó׸ӽ�ī�޶�
    private CinemachineVirtualCamera aimVirtualCamera;
    //ī�޶� root
    private GameObject cameraRoot;
    //ī�޶� 
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();


    private StarterAssetsInputs playerInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;


    float hitDistance = 2f;
    public bool InvestigateValue;

    Material outline;

    Renderer renderers;
    List<Material> materialList = new List<Material>();

    Vector3 mouseWorldPosition = Vector3.zero;

    private void Start()
    {
        outline = new Material(Shader.Find("Custom/OutLine"));
    }

    private void Awake()
    {

        pv = GetComponent<PhotonView>();

        //CinemachineVirtualCamera[] cameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        //foreach (CinemachineVirtualCamera camera in cameras)
        //{
        //    if (camera.Name == "AsunaFollowCamera") virtualCamera = camera;
        //    else if (camera.Name == "AsunaAimCamera") aimVirtualCamera = camera;
        //}
        //cameraRoot = GameObject.FindGameObjectWithTag("CinemachineTarget");
        //// �ڽ��� ���� ĳ������ ��� �ó׸ӽ� ī�޶� ����
        //if (pv.IsMine)
        //{
        //    virtualCamera.Follow = cameraRoot.transform;
        //    aimVirtualCamera.Follow = cameraRoot.transform;
        //}
        playerInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        //ȭ�� �߾� 2���� ���Ͱ�
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        //ray������Ʈ ī�޶󿡼� ���콺�� ����Ű�� ȭ������Ʈ�� ray��ü�� �Ҵ�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {

            // raycast �Ÿ��� 2f �̳�, ȭ�鿡 UI�����ÿ��� Ȱ��ȭ
            // 
            if (hit.distance < hitDistance && EventSystem.current.IsPointerOverGameObject() == false)
            {
                //Debug.Log("�浹��ü:" + hit.collider.name);
                // �̺�Ʈ ������Ʈ �Ͻ�
                if (hit.collider.tag == "EventObj")
                {
                    // ��ȣ�ۿ� ��ư Ȱ��ȭ
                    InvestigateValue = true;

                    // ��ȣ�ۿ� �޼��� Ȱ��ȭ
                    Popup.instance.OpenPopUp();

                    // ��ȣ�ۿ�� ���� Ȱ��ȭ
                    if (playerInputs.investigate == true)
                    {
                        GameObject.Find(hit.collider.name).GetComponent<Puzzle>().Activate();

                    } 
                    // Ű���� RŰ �Է� ��
                    //if (Input.GetKeyDown(KeyCode.R))
                    //{
                    //    Puzzle.target1();
                    //    //GameObject.Find("Puzzle2").GetComponent<Activate1>().Activate();
                    //}
                }
                else
                {
                    InvestigateValue = false;
                    Popup.instance.ClosePopUp();

                }
            }
            // raycast�� ��ü�� ���� �� 
            else
            {
                InvestigateValue = false;
                Popup.instance.ClosePopUp();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EventObj")
        {
            if (playerInputs.investigate)
            {
                other.GetComponent<EventObject>().getEventUI().SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "EventObj")
        {
            other.GetComponent<EventObject>().getEventUI().SetActive(false);
        }
    }


}

