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

    // Инициализация меню
    public void ItemsMenuInit(int[] _inventory)
    {
        inventory = _inventory;

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
}
