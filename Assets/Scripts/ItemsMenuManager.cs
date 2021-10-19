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

        // Добавление слушателей на кнопки предметов
        for (int i = 0; i < 16; i++)
        {
            Button _item = items.transform.GetChild(i).GetComponent<Button>();
            _item.onClick.AddListener(UseItem);
        }
    }

    // Инициализация меню
    public void ItemsMenuInit()
    {
        inventory = player.GetComponent<Player>().GetInventory();

        WindowInit();
    }

    // Инициализация окна меню
    private void WindowInit()
    {
        // Активация окна
        itemsUI.SetActive(true);

        // Подсветка первого предмета в меню
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(items.transform.GetChild(0).gameObject);

        for (int i = 0; i < 16; i++)
        {
            GameObject _item = items.transform.GetChild(i).gameObject;

            // Красный цветок
            if (inventory[i] == 1)
            {
                _item.GetComponent<Image>().sprite = redFlower;
            }

            // Синий цветок
            else if (inventory[i] == 2)
            {
                _item.GetComponent<Image>().sprite = blueFlower;
            }

            // Пустая ячейка
            else
            {
                _item.GetComponent<Image>().sprite = null;
            }
        }

        StartCoroutine(Act());
    }

    // Действия в меню
    private IEnumerator Act()
    {
        while (!Input.GetKey(KeyCode.Escape))
        {
            yield return new WaitForFixedUpdate();
            
            // Вывод информации о выбранном предмете
            ItemInfo(Convert.ToInt32(eventSystem.currentSelectedGameObject.name) - 1);
        }

        // Деактивация окна предметов
        itemsUI.SetActive(false);

        // Активация окна боя
        GetComponent<BattleManager>().WindowInit();
    }

    // Информация о предмете
    private void ItemInfo(int _i)
    {
        // Красный цветок
        if (inventory[_i] == 1)
        {
            textField.GetComponent<TextMeshProUGUI>().text = "Red Flower";
        }

        // Синий цветок
        else if (inventory[_i] == 2)
        {
            textField.GetComponent<TextMeshProUGUI>().text = "Blue Flower";
        }

        else
        {
            textField.GetComponent<TextMeshProUGUI>().text = "EMPTY";
        }
    }

    // Использование предмета
    private void UseItem()
    {
        int _i = Convert.ToInt32(eventSystem.currentSelectedGameObject.name) - 1;

        switch (inventory[_i])
        {
            // Красный цветок
            case 1:
                GetComponent<BattleManager>().SetHealth(3);

                // Удаляем предмет из инвентаря
                inventory[_i] = 0;
                player.GetComponent<Player>().SetInventory(inventory);
                eventSystem.currentSelectedGameObject.GetComponent<Image>().sprite = null;

                // Изменение времени на миниигру
                int _battleStage = GetComponent<BattleManager>().GetBattleStage();

                // Если этап атаки
                if (_battleStage == 1)
                {
                    GetComponent<BattleManager>().ChangeBattleTime(-5f);
                }
                
                // Если этап защиты
                else if (_battleStage == 2)
                {
                    GetComponent<BattleManager>().ChangeBattleTime(5f);
                }

                break;

            // Синий цветок
            case 2:
                int _health = player.GetComponent<Player>().GetMaxHealth();
                GetComponent<BattleManager>().SetHealth(_health);

                // Удаляем предмет из инвентаря
                inventory[_i] = 0;
                player.GetComponent<Player>().SetInventory(inventory);
                eventSystem.currentSelectedGameObject.GetComponent<Image>().sprite = null;

                // Изменение времени на миниигру
                GetComponent<BattleManager>().ChangeBattleTime(-5f);
                break;

            // Пусто
            default:
                break;
        }
    }
}
