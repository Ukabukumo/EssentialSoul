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

        // ���������� ���������� ������
        lastSelectedObject = eventSystem.currentSelectedGameObject;

        StartCoroutine(Act());
    }

    // �������� � ����
    private IEnumerator Act()
    {
        // ���� ���� �������
        while (mainMenuBG.activeSelf)
        {
            yield return null;

            ChangeButton();
        }
    }

    // �������� ��� ������� ������ "START"
    private void StartButtonAct()
    {
        // ��������������� ����� ������� �� ������
        soundManager.PlaySound(pressButtonSound);

        // �������� ������
        GetComponent<WorldManager>().CreateLocation(player);

        // ��������� ������
        player.SetActive(true);

        // ����������� ����
        mainMenuBG.SetActive(false);

        // ��������������� ������
        soundManager.SetMusic(mainTheme);
        soundManager.PlayMusic();
    }

    // �������� ��� ������� ������ "EXIT"
    private void ExitButtonAct()
    {
        // ��������������� ����� ������� �� ������
        soundManager.PlaySound(pressButtonSound);

        // ����� �� ����
        Application.Quit();
    }

    // �������� ��� ����� ������
    private void ChangeButton()
    {
        // ���� ��������� ����� ������
        if (eventSystem.currentSelectedGameObject != lastSelectedObject)
        {
            // ��������������� ����� ����� ������
            soundManager.PlaySound(changeButtonSound);

            // ���������� ���������� ������
            lastSelectedObject = eventSystem.currentSelectedGameObject;
        }
    }
}
