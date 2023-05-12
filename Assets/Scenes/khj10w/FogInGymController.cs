using UnityEngine;

public class FogInGymController : MonoBehaviour
{
    private int fogLevel = 1;
    private float timePassed = 0f;
    private float fogIncreaseInterval = 5f;
    private float playerDeathDelay = 20f; // 3�ܰ谡 ���� �� �߰��� 5�ʰ� ������ �÷��̾� ��� �Լ��� ȣ��

    public Color fogColor = Color.magenta; // �Ȱ��� �����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Taichi"))
        {
            RenderSettings.fog = true; // �Ȱ��� Ȱ��ȭ�մϴ�.
            float density = 0.1f;
            RenderSettings.fogDensity = density;
            RenderSettings.fogColor = fogColor; // �Ȱ��� ������ �����մϴ�.
            InvokeRepeating("IncreaseFogLevel", fogIncreaseInterval, fogIncreaseInterval);
            Invoke("PlayerDeath", playerDeathDelay);
        }
    }

    private void IncreaseFogLevel()
    {
        fogLevel++;
        Debug.Log("�Ȱ� �ܰ� " + fogLevel + " ����");

        // �Ȱ� �е� ����
        float density = fogLevel * 0.2f; // ���÷� �е��� fogLevel�� ����ϰ� ������ŵ�ϴ�.
        RenderSettings.fogDensity = density;

        if (fogLevel >= 3)
        {
            CancelInvoke("IncreaseFogLevel");
        }
    }

    private void PlayerDeath()
    {
        // �÷��̾� ��� �� ȣ��Ǵ� �Լ��Դϴ�.
        // ���ϴ� ������ �ۼ����ּ���.
        // ���� ���, ���� ���� ȭ���� ǥ���ϰų� �ٸ� ���� ������ ������ �� �ֽ��ϴ�.
    }
}
