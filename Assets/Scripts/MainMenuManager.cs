using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBG;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject player;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;

        // Добавление слушателей на кнопки
        startButton.onClick.AddListener(StartButtonAct);
        exitButton.onClick.AddListener(ExitButtonAct);
    }

    // Инициализация меню
    public void MainMenuInit()
    {
        WindowInit();
    }

    // Инициализация окна меню
    private void WindowInit()
    {
        // Активация окна
        mainMenuBG.SetActive(true);

        // Подсветка кнопки "START" в меню
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(startButton.gameObject);
    }

    // Действие при нажатии кнопки "START"
    private void StartButtonAct()
    {
        // Создание уровня
        GetComponent<WorldManager>().CreateLevel(player);

        // Активация игрока
        player.SetActive(true);

        // Деактивация окна
        mainMenuBG.SetActive(false);
    }

    // Действие при нажатии кнопки "EXIT"
    private void ExitButtonAct()
    {
        // Выход из игры
        Application.Quit();
    }
}
