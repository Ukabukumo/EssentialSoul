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
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite redFlower;
    [SerializeField] private Sprite blueFlower;
    private EventSystem eventSystem;
    private int[] inventory;

    private void Start()
    {
        eventSystem = EventSystem.current;

        // ���������� ���������� �� ������ ���������
        for (int i = 0; i < 16; i++)
        {
            Button _item = items.transform.GetChild(i).GetComponent<Button>();
            _item.onClick.AddListener(UseItem);
        }
    }

    // ������������� ����
    public void ItemsMenuInit()
    {
        inventory = player.GetComponent<Player>().GetInventory();

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

    // ������������� ��������
    private void UseItem()
    {
        int _i = Convert.ToInt32(eventSystem.currentSelectedGameObject.name) - 1;

        switch (inventory[_i])
        {
            // ������� ������
            case 1:
                GetComponent<BattleManager>().SetHealth(3);

                // ������� ������� �� ���������
                inventory[_i] = 0;
                player.GetComponent<Player>().SetInventory(inventory);
                eventSystem.currentSelectedGameObject.GetComponent<Image>().sprite = null;

                // ��������� ������� �� ��������
                int _battleStage = GetComponent<BattleManager>().GetBattleStage();

                // ���� ���� �����
                if (_battleStage == 1)
                {
                    GetComponent<BattleManager>().ChangeBattleTime(-5f);
                }
                
                // ���� ���� ������
                else if (_battleStage == 2)
                {
                    GetComponent<BattleManager>().ChangeBattleTime(5f);
                }

                break;

            // ����� ������
            case 2:
                int _health = player.GetComponent<Player>().GetMaxHealth();
                GetComponent<BattleManager>().SetHealth(_health);

                // ������� ������� �� ���������
                inventory[_i] = 0;
                player.GetComponent<Player>().SetInventory(inventory);
                eventSystem.currentSelectedGameObject.GetComponent<Image>().sprite = null;

                // ��������� ������� �� ��������
                GetComponent<BattleManager>().ChangeBattleTime(-5f);
                break;

            // �����
            default:
                break;
        }
    }
}
