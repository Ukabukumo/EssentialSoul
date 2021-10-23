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

        // ���������� ���������� �� ������ �������
        for (int i = 0; i < 55; i++)
        {
        }
    }

    // ������������� ����
    public void SkillsMenuInit()
    {
        WindowInit();
    }

    // ������������� ���� ����
    private void WindowInit()
    {
        // ��������� ����
        skillsMenu.SetActive(true);

        // ��������� ������� ������ � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(skills.transform.GetChild(0).gameObject);

        StartCoroutine(Act());
    }

    // �������� � ����
    private IEnumerator Act()
    {
        while (!Input.GetKey(KeyCode.Escape))
        {
            yield return new WaitForFixedUpdate();
        }

        // ����������� ���� �������
        skillsMenu.SetActive(false);

        // ��������� ������
        player.SetActive(true);
    }
}
