using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBG;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject player;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip changeButtonSound;
    [SerializeField] private AudioClip pressButtonSound;
    
    private EventSystem eventSystem;
    private GameObject lastSelectedObject;

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

        // Назначение предыдущей кнопки
        lastSelectedObject = eventSystem.currentSelectedGameObject;

        StartCoroutine(Act());
    }

    // Действия в меню
    private IEnumerator Act()
    {
        // Пока меню активно
        while (mainMenuBG.activeSelf)
        {
            yield return null;

            ChangeButton();
        }
    }

    // Действие при нажатии кнопки "START"
    private void StartButtonAct()
    {
        // Воспроизведение звука нажатия на кнопку
        soundManager.PlaySound(pressButtonSound);

        // Создание уровня
        GetComponent<WorldManager>().CreateLocation(player);

        // Активация игрока
        player.SetActive(true);

        // Деактивация окна
        mainMenuBG.SetActive(false);

        // Воспроизведение музыки
        soundManager.SetMusic(mainTheme);
        soundManager.PlayMusic();
    }

    // Действие при нажатии кнопки "EXIT"
    private void ExitButtonAct()
    {
        // Воспроизведение звука нажатия на кнопку
        soundManager.PlaySound(pressButtonSound);

        // Выход из игры
        Application.Quit();
    }

    // Действия при смене кнопки
    private void ChangeButton()
    {
        // Если произошла смена кнопки
        if (eventSystem.currentSelectedGameObject != lastSelectedObject)
        {
            // Воспроизведение звука смены кнопки
            soundManager.PlaySound(changeButtonSound);

            // Назначение предыдущей кнопки
            lastSelectedObject = eventSystem.currentSelectedGameObject;
        }
    }
}
