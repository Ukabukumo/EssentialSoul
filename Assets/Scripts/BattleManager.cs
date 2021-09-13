using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject battleBackground;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button defenseButton;
    [SerializeField] private Button itemsButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private GameObject miniGameManager;
    [SerializeField] private GameObject miniGameCamera;
    private EventSystem eventSystem;
    private GameObject mgm;
    private float battleTime;
    private int battleStage;

    public void Start()
    {
        eventSystem = EventSystem.current;
        mgm = Instantiate(miniGameManager);
        battleTime = 5f;
        battleStage = 1;

        // ���������� ���������� �� ������
        attackButton.onClick.AddListener(AttackButtonAct);
        defenseButton.onClick.AddListener(DefenseButtonAct);
        itemsButton.onClick.AddListener(ItemsButtonAct);
        leaveButton.onClick.AddListener(LeaveButtonAct);
    }

    private void Update()
    {
        if (player.GetComponent<Player>().GetDistance() >= 200f)
        {
            player.GetComponent<Player>().ZeroDistance();
            BattleInit();
        }
    }

    private class Enemy
    {
        // �������������� ����� �����
        public struct SwordAttack
        {
            public int power;       // ����
            public int speed;       // ��������
            public int frequency;   // ������� ���������
        }

        // �������������� ����� ��������
        public struct ArrowAttack
        {
            public int power;       // ����
            public int speed;       // ��������
            public int frequency;   // ������� ���������
        }

        // �������������� ����� ������������
        public struct SpellAttack
        {
            public int power;       // ����
            public int speed;       // ��������
            public int frequency;   // ������� ���������
        }

        public SwordAttack swordAttack = new SwordAttack();
        public ArrowAttack arrowAttack = new ArrowAttack();
        public SpellAttack spellAttack = new SpellAttack();
        public int health;

        public Enemy()
        {
            swordAttack.power = 0;
            swordAttack.speed = 0;
            swordAttack.frequency = 0;

            arrowAttack.power = 0;
            arrowAttack.speed = 0;
            arrowAttack.frequency = 0;

            spellAttack.power = 0;
            spellAttack.speed = 0;
            spellAttack.frequency = 0;

            health = 0;
        }
    }

    public void BattleInit()
    {
        // ��������� ������
        player.SetActive(false);
        miniGameCamera.SetActive(true);

        WindowInit();
        CreateEnemy();
    }

    // ������������� ���� �����
    public void WindowInit()
    {
        // ��������� ������ �����
        battleBackground.SetActive(true);

        // ��������� ������ ������ � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(attackButton.gameObject);
    }

    // �������� ����������
    private void CreateEnemy()
    {
        // ��������� ���� ���������� �� ���������������
        int n = Random.Range(0, 2);
        switch (n)
        {
            case 0:
                Debug.Log("Enemy1");
                break;

            case 1:
                Debug.Log("Enemy2");
                break;

            default:
                Debug.Log("Enemy");
                break;
        }
    }

    // �������� ��� ������� ������ "ATTACK"
    private void AttackButtonAct()
    {
        mgm.GetComponent<Attack>().AttackInit(battleTime);
        battleBackground.SetActive(false);
        StartCoroutine("CheckEnd");
    }

    // �������� ��� ������� ������ "DEFENSE"
    private void DefenseButtonAct()
    {
        Debug.Log("Defense");
    }

    // �������� ��� ������� ������ "ITEMS"
    private void ItemsButtonAct()
    {
        Debug.Log("Items");
    }

    // �������� ��� ������� ������ "LEAVE"
    private void LeaveButtonAct()
    {
        // ������� ���� �����
        battleBackground.SetActive(false);

        // ������� ������ ������
        miniGameCamera.SetActive(false);

        // �������� ���������� ������
        player.SetActive(true);
    }

    IEnumerator CheckEnd()
    {
        // ���� ������ ���� �����
        if (battleStage == 1)
        {
            yield return new WaitWhile(() => !mgm.GetComponent<Attack>().IsEnd());
            Debug.Log(mgm.GetComponent<Attack>().GetFactor());
        }

        // ���� ������ ���� ������
        else if (battleStage == 2)
        {
            yield return new WaitWhile(() => !mgm.GetComponent<Attack>().IsEnd());
        }

        WindowInit();
    }
}
