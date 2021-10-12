using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject itemsUI;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    // ������������� ����
    public void ItemsMenuInit()
    {
        WindowInit();
    }

    // ������������� ���� ����
    private void WindowInit()
    {
        // ��������� ����
        itemsUI.SetActive(true);

        // ��������� ������� �������� � ����
        eventSystem.SetSelectedGameObject(null);
        Transform _items = itemsUI.transform.GetChild(0).transform.GetChild(0);
        eventSystem.SetSelectedGameObject(_items.GetChild(0).gameObject);

        StartCoroutine(Act());
    }

    // �������� � ����
    private IEnumerator Act()
    {
        while (!Input.GetKey(KeyCode.Escape))
        {
            yield return new WaitForFixedUpdate();
        }

        // ����������� ���� ���������
        itemsUI.SetActive(false);

        // ��������� ���� ���
        GetComponent<BattleManager>().WindowInit();
    }
}
