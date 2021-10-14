using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class ItemsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject itemsUI;
    [SerializeField] private GameObject items;
    [SerializeField] private GameObject textField;
    [SerializeField] private Sprite redFlower;
    [SerializeField] private Sprite blueFlower;
    private EventSystem eventSystem;
    private int[] inventory;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    // ������������� ����
    public void ItemsMenuInit(int[] _inventory)
    {
        inventory = _inventory;

        WindowInit();
    }

    // ������������� ���� ����
    private void WindowInit()
    {
        // ��������� ����
        itemsUI.SetActive(true);

        // ��������� ������� �������� � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(items.transform.GetChild(0).gameObject);

        for (int i = 0; i < 16; i++)
        {
            GameObject _item = items.transform.GetChild(i).gameObject;

            // ������� ������
            if (inventory[i] == 1)
            {
                _item.GetComponent<Image>().sprite = redFlower;
            }

            // ����� ������
            else if (inventory[i] == 2)
            {
                _item.GetComponent<Image>().sprite = blueFlower;
            }

            // ������ ������
            else
            {
                _item.GetComponent<Image>().sprite = null;
            }
        }

        StartCoroutine(Act());
    }

    // �������� � ����
    private IEnumerator Act()
    {
        while (!Input.GetKey(KeyCode.Escape))
        {
            yield return new WaitForFixedUpdate();
            
            // ����� ���������� � ��������� ��������
            ItemInfo(Convert.ToInt32(eventSystem.currentSelectedGameObject.name) - 1);
        }

        // ����������� ���� ���������
        itemsUI.SetActive(false);

        // ��������� ���� ���
        GetComponent<BattleManager>().WindowInit();
    }

    // ���������� � ��������
    private void ItemInfo(int _i)
    {
        // ������� ������
        if (inventory[_i] == 1)
        {
            textField.GetComponent<TextMeshProUGUI>().text = "Red Flower";
        }

        // ����� ������
        else if (inventory[_i] == 2)
        {
            textField.GetComponent<TextMeshProUGUI>().text = "Blue Flower";
        }

        else
        {
            textField.GetComponent<TextMeshProUGUI>().text = "EMPTY";
        }
    }
}
