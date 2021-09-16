using UnityEngine;

public class Defense : MonoBehaviour
{
    [SerializeField] private GameObject defenseBGPref;
    private GameObject storage;

    // ������������� �������� ������
    public void DefenceInit()
    {
        // ��������� ��� �������� �����
        storage = new GameObject("Storage");

        // �������� ����
        Instantiate(defenseBGPref, new Vector3(0f, 0f, -1f), Quaternion.identity, storage.transform);
    }
}
