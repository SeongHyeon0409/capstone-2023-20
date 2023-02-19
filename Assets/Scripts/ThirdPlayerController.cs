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
    //����� - �κ��丮 GameObject �߰�
    [SerializeField] private GameObject Inventory;
    //����� - �κ��丮 ������ InventoryManager �߰�
    [SerializeField] private InventoryManager InventoryManager;
    //����� - �̴ϸ� GameObject �߰�
    [SerializeField] private GameObject Minimap;
    //����� - ���� ĳ���Ͱ� ��ġ�� �� ����
    [SerializeField] private GameObject CurrentMap;


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
        //����� - �κ��丮 ���½� �κ��丮 UI Ȱ��ȭ
        //����� - �ߺ� UI ���� ���� �̴ϸ� UI ��Ȱ�� �ڵ� �߰�
        if (playerInputs.inventory)
        {
            Inventory.SetActive(true);
            playerInputs.minimap = false;
        }
        else
        {
            Inventory.SetActive(false);
        }

        //����� - �̴ϸ� ���½� �̴ϸ� UI Ȱ��ȭ
        //����� - �ߺ� UI ���� ���� �κ��丮 UI ��Ȱ�� �ڵ� �߰�
        //����� - ���� UI�� �ߺ����� ������ Update �Լ� Ư���� �� ������ ���� Map -> Inventory��
        //MapUI���� InventoryUI�� UI�� ��ȯ�ǳ� Inventory -> Map���δ� ��ȯ���� ����. 
        //������ �ʿ�� Inventory -> Map ��ȯ��� �߰� ���� Ȥ�� ��ȯ�� �ƿ� ���� �������� ����.
        if (playerInputs.minimap)
        {
            Minimap.SetActive(true);
            playerInputs.inventory = false;
        }
        else
        {
            Minimap.SetActive(false);
        }

    }

    //����� - �̴ϸ� ��ȯ ���� ���Խ� �̴ϸ� ��ȯ.
    //����� - CurrentMap�� Null ������ ��� ���� ������ ��ȯ������ TransMap�� CurrentMap���� ����.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Maps")
        {
            Debug.Log("Entering : " + other);
            if (CurrentMap == null)
            {
                GameObject TransMap = other.GetComponent<MinimapTransition>().getMap();
                TransMap.SetActive(true);
                CurrentMap = TransMap;
            }
            if(other.name == "Stair2")
            {
                if (CurrentMap.name == "Floor1")
                {
                    CurrentMap.SetActive(false);
                    GameObject TransMap = other.GetComponent<MinimapTransition>().getMap();
                    TransMap.SetActive(true);
                    CurrentMap = TransMap;
                }
                
            }
            if(other.name == "Stair1")
            {
                if(CurrentMap.name == "Floor2")
                {
                    CurrentMap.SetActive(false);
                    GameObject TransMap = other.GetComponent<MinimapTransition>().getMap();
                    TransMap.SetActive(true);
                    CurrentMap = TransMap;
                }
            }
        }
    }

    //����� - �ɵ��� ������ ȹ���� ���� OnTriggerEnter -> OnTriggerStay�� ��ȯ
    //       - cf) Enter�� �� �� ���������� �����̹Ƿ� ������ ȹ���� ���� ��ȣ�ۿ� ��ư�� ������ ȹ����� �ʴ� ��찡 ����. 
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Items")
        {
            if (playerInputs.investigate)
            {
                Debug.Log("Investigating");
                other.GetComponent<EventObject>().getEventUI().SetActive(true);
            }

            //����� - ������ ��ȣ�ۿ�� ����
            if (playerInputs.interaction)
            {
                //����� - ��ȣ�ۿ�� ���ִ� EventUI ���� ����.
                other.GetComponent<EventObject>().getText().SetActive(false);
                other.GetComponent<EventObject>().getEventUI().SetActive(false);
                other.GetComponent<GetItem>().Get();
                InventoryManager.Instance.addItem(other.GetComponent<ItemController>().Item);
                playerInputs.interaction = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Items")
        {
            other.GetComponent<EventObject>().getEventUI().SetActive(false);
        }
        if (other.tag == "Maps")
        {
            Debug.Log("Exiting : " + other);
            if (other.name == "Stair2")
            {
                if (CurrentMap.name == "Floor1")
                {
                    CurrentMap.SetActive(false);
                }

            }
        }
        if (other.tag == "Maps")
        {
            if (other.name == "Stair1")
            {
                if (CurrentMap.name == "Floor2")
                {
                    CurrentMap.SetActive(false);
                }

            }
        }
    }


}

