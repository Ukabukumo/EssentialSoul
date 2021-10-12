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

    // Инициализация меню
    public void ItemsMenuInit()
    {
        WindowInit();
    }

    // Инициализация окна меню
    private void WindowInit()
    {
        // Активация окна
        itemsUI.SetActive(true);

        // Подсветка первого предмета в меню
        eventSystem.SetSelectedGameObject(null);
        Transform _items = itemsUI.transform.GetChild(0).transform.GetChild(0);
        eventSystem.SetSelectedGameObject(_items.GetChild(0).gameObject);

        StartCoroutine(Act());
    }

    // Действия в меню
    private IEnumerator Act()
    {
        while (!Input.GetKey(KeyCode.Escape))
        {
            yield return new WaitForFixedUpdate();
        }

        // Деактивация окна предметов
        itemsUI.SetActive(false);

        // Активация окна боя
        GetComponent<BattleManager>().WindowInit();
    }
}
