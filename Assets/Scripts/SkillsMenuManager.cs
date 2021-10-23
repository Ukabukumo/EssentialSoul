using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class SkillsMenuManager : MonoBehaviour
{
    [SerializeField] GameObject skillsMenu;
    [SerializeField] GameObject skills;
    [SerializeField] GameObject player;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;

        // Добавление слушателей на кнопки навыков
        for (int i = 0; i < 55; i++)
        {
        }
    }

    // Инициализация меню
    public void SkillsMenuInit()
    {
        WindowInit();
    }

    // Инициализация окна меню
    private void WindowInit()
    {
        // Активация окна
        skillsMenu.SetActive(true);

        // Подсветка первого навыка в меню
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(skills.transform.GetChild(0).gameObject);

        StartCoroutine(Act());
    }

    // Действия в меню
    private IEnumerator Act()
    {
        while (!Input.GetKey(KeyCode.Escape))
        {
            yield return new WaitForFixedUpdate();
        }

        // Деактивация окна навыков
        skillsMenu.SetActive(false);

        // Активация игрока
        player.SetActive(true);
    }
}
