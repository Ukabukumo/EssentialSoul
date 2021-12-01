using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject miniGameCamera;
    [SerializeField] GameObject player;
    [SerializeField] Button returnButton;
    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;
    [SerializeField] Button exitButton;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;

        // ���������� ���������� �� ������
        returnButton.onClick.AddListener(ReturnToGame);
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    // ������������� ����
    public void GameMenuInit()
    {
        WindowInit();
    }

    // ������������� ���� ����
    private void WindowInit()
    {
        // ��������� ����
        gameMenu.SetActive(true);

        // ��������� �������������� ������
        miniGameCamera.SetActive(true);

        // ��������� ������ ������ � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(returnButton.gameObject);

        StartCoroutine(Act());
    }

    // �������� � ����
    private IEnumerator Act()
    {
        while (gameMenu.activeSelf)
        {
            yield return null;
        }
    }

    // �������� ��� ������� ������ "RETURN"
    private void ReturnToGame()
    {
        // ����������� ����
        gameMenu.SetActive(false);

        // ��������� ������
        player.SetActive(true);

        // ������� �������������� ������
        miniGameCamera.SetActive(false);
    }

    // ���������� ����
    private void SaveGame()
    {
        string _myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string _dirname = Path.Combine(_myDocuments, "EssentialSoul");

        // ���� ���������� �� ����������, �� ������ �
        DirectoryInfo _dirInfo = new DirectoryInfo(_dirname);
        if (!_dirInfo.Exists)
        {
            _dirInfo.Create();
        }

        string _filename = Path.Combine(_dirname, "save.txt");

        StreamWriter _sw = new StreamWriter(_filename, false);

        // ��������� ������ �� ������ � ����
        ArrayList _playerInfo = player.GetComponent<Player>().GetInfo();
        
        _sw.WriteLine("<<Player>>");
        for (int i = 0; i < _playerInfo.Count; i++)
        {
            _sw.WriteLine(_playerInfo[i]);
        }

        // �����������
        _sw.WriteLine("-----");

        // ��������� ������ � ������� � ����
        ArrayList _skillsInfo = GetComponent<SkillsMenuManager>().GetInfo();

        _sw.WriteLine("<<Skills>>");
        for (int i = 0; i < _skillsInfo.Count; i++)
        {
            _sw.WriteLine(_skillsInfo[i]);
        }

        _sw.Close();
    }

    // �������� ����
    private void LoadGame()
    {
        string _myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string _dirname = Path.Combine(_myDocuments, "EssentialSoul");

        // ���� ���������� �� ����������, �� �� �����������
        DirectoryInfo _dirInfo = new DirectoryInfo(_dirname);
        if (!_dirInfo.Exists)
        {
            return;
        }

        string _filename = Path.Combine(_dirname, "save.txt");

        // ���� ���� �� ����������, �� �� �����������
        FileInfo _fileInfo = new FileInfo(_filename);
        if (!_fileInfo.Exists)
        {
            return;
        }

        StreamReader _sr = new StreamReader(_filename);
        string _line;

        // �������� �� ����� ���������� �� ������
        _sr.ReadLine();
        ArrayList _playerInfo = new ArrayList();
        while ((_line = _sr.ReadLine()) != "-----")
        {
            _playerInfo.Add(_line);
        }

        // �������� �� ����� ���������� � �������
        _sr.ReadLine();
        ArrayList _skillsInfo = new ArrayList();
        while ((_line = _sr.ReadLine()) != null)
        {
            _skillsInfo.Add(_line);
        }

        _sr.Close();

        // �������� ����������� ���������� �� ������
        player.GetComponent<Player>().SetInfo(_playerInfo);

        // �������� ����������� ���������� � �������
        GetComponent<SkillsMenuManager>().SetInfo(_skillsInfo);
    }

    // �������� ��� ������� ������ "EXIT"
    private void ExitGame()
    {
        // ����� �� ����
        Application.Quit();
    }
}
