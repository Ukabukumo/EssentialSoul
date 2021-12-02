using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject miniGameCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip changeButtonSound;
    [SerializeField] private AudioClip pressButtonSound;

    private EventSystem eventSystem;
    private GameObject lastSelectedObject;

    private void Start()
    {
        eventSystem = EventSystem.current;

        // Добавление слушателей на кнопки
        returnButton.onClick.AddListener(ReturnToGame);
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    // Инициализация меню
    public void GameMenuInit()
    {
        WindowInit();
    }

    // Инициализация окна меню
    private void WindowInit()
    {
        // Активация окна
        gameMenu.SetActive(true);

        // Добавляем дополнительную камеру
        miniGameCamera.SetActive(true);

        // Подсветка первой кнопки в меню
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(returnButton.gameObject);

        // Назначение предыдущей кнопки
        lastSelectedObject = eventSystem.currentSelectedGameObject;

        StartCoroutine(Act());
    }

    // Действия в меню
    private IEnumerator Act()
    {
        // Пока меню активно
        while (gameMenu.activeSelf)
        {
            yield return null;

            ChangeButton();
        }
    }

    // Действие при нажатии кнопки "RETURN"
    private void ReturnToGame()
    {
        // Воспроизведение звука нажатия на кнопку
        soundManager.PlaySound(pressButtonSound);

        // Деактивация меню
        gameMenu.SetActive(false);

        // Активация игрока
        player.SetActive(true);

        // Убираем дополнительную камеру
        miniGameCamera.SetActive(false);
    }

    // Сохранение игры
    private void SaveGame()
    {
        string _myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string _dirname = Path.Combine(_myDocuments, "EssentialSoul");

        // Если директория не существует, то создаём её
        DirectoryInfo _dirInfo = new DirectoryInfo(_dirname);
        if (!_dirInfo.Exists)
        {
            _dirInfo.Create();
        }

        string _filename = Path.Combine(_dirname, "save.txt");

        StreamWriter _sw = new StreamWriter(_filename, false);

        // Сохраняем данные об игроке в файл
        ArrayList _playerInfo = player.GetComponent<Player>().GetInfo();
        
        _sw.WriteLine("<<Player>>");
        for (int i = 0; i < _playerInfo.Count; i++)
        {
            _sw.WriteLine(_playerInfo[i]);
        }

        // Разделитель
        _sw.WriteLine("-----");

        // Сохраняем данные о навыках в файл
        ArrayList _skillsInfo = GetComponent<SkillsMenuManager>().GetInfo();

        _sw.WriteLine("<<Skills>>");
        for (int i = 0; i < _skillsInfo.Count; i++)
        {
            _sw.WriteLine(_skillsInfo[i]);
        }

        _sw.Close();

        // Возврат в игру
        ReturnToGame();
    }

    // Загрузка игры
    private void LoadGame()
    {
        string _myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string _dirname = Path.Combine(_myDocuments, "EssentialSoul");

        // Если директория не существует, то не загружаемся
        DirectoryInfo _dirInfo = new DirectoryInfo(_dirname);
        if (!_dirInfo.Exists)
        {
            return;
        }

        string _filename = Path.Combine(_dirname, "save.txt");

        // Если файл не существует, то не загружаемся
        FileInfo _fileInfo = new FileInfo(_filename);
        if (!_fileInfo.Exists)
        {
            return;
        }

        StreamReader _sr = new StreamReader(_filename);
        string _line;

        // Выгрузка из файла информации об игроке
        _sr.ReadLine();
        ArrayList _playerInfo = new ArrayList();
        while ((_line = _sr.ReadLine()) != "-----")
        {
            _playerInfo.Add(_line);
        }

        // Выгрузка из файла информации о навыках
        _sr.ReadLine();
        ArrayList _skillsInfo = new ArrayList();
        while ((_line = _sr.ReadLine()) != null)
        {
            _skillsInfo.Add(_line);
        }

        _sr.Close();

        // Передача загруженной информации об игроке
        player.GetComponent<Player>().SetInfo(_playerInfo);

        // Передача загруженной информации о навыках
        GetComponent<SkillsMenuManager>().SetInfo(_skillsInfo);

        // Возврат в игру
        ReturnToGame();
    }

    // Действие при нажатии кнопки "EXIT"
    private void ExitGame()
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
