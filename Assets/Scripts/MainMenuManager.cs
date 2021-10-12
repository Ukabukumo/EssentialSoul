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

        // ���������� ���������� �� ������
        startButton.onClick.AddListener(StartButtonAct);
        exitButton.onClick.AddListener(ExitButtonAct);
    }

    // ������������� ����
    public void MainMenuInit()
    {
        WindowInit();
    }

    // ������������� ���� ����
    private void WindowInit()
    {
        // ��������� ����
        mainMenuBG.SetActive(true);

        // ��������� ������ "START" � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(startButton.gameObject);
    }

    // �������� ��� ������� ������ "START"
    private void StartButtonAct()
    {
        // �������� ������
        GetComponent<WorldManager>().CreateLevel(player);

        // ��������� ������
        player.SetActive(true);

        // ����������� ����
        mainMenuBG.SetActive(false);
    }

    // �������� ��� ������� ������ "EXIT"
    private void ExitButtonAct()
    {
        // ����� �� ����
        Application.Quit();
    }
}
