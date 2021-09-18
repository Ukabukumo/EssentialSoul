using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject battleBG;
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
    private Enemy enemy;
    private int playerHealth;

    public void Start()
    {
        eventSystem = EventSystem.current;
        mgm = Instantiate(miniGameManager);
        battleTime = 10f;
        battleStage = 2;

        // ���������� ���������� �� ������
        attackButton.onClick.AddListener(AttackButtonAct);
        defenseButton.onClick.AddListener(DefenseButtonAct);
        itemsButton.onClick.AddListener(ItemsButtonAct);
        leaveButton.onClick.AddListener(LeaveButtonAct);
    }

    private void Update()
    {
        // �������� ����������� ����������
        if (player.GetComponent<Player>().GetDistance() >= 200f)
        {
            player.GetComponent<Player>().ZeroDistance();
            BattleInit();
        }
    }

    public void BattleInit()
    {
        // ��������� �������� ������
        playerHealth = player.GetComponent<Player>().GetHealth();

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
        battleBG.SetActive(true);

        // ��������� ������ ������ � ����
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(attackButton.gameObject);
    }

    // �������� ����������
    private void CreateEnemy()
    {
        // ��������� ���� ���������� �� ���������������
        int n = Random.Range(0, 1);
        enemy = new Enemy();
        switch (n)
        {
            case 0:
                enemy.health = 10;

                /*enemy.swordAttack.active = true;
                enemy.swordAttack.power = 1;
                enemy.swordAttack.speed = 2f;
                enemy.swordAttack.frequency = 1f;*/

                /*enemy.arrowAttack.active = true;
                enemy.arrowAttack.power = 1;
                enemy.arrowAttack.speed = 5f;
                enemy.arrowAttack.frequency = 0.5f;*/

                enemy.spellAttack.active = true;
                enemy.spellAttack.power = 1;
                enemy.spellAttack.frequency = 0.5f;

                enemy.nArmor = 5;
                enemy.nFalseAim = 2;
                //enemy.inverseMove = true;
                break;

            case 1:
                enemy.health = 15;
                enemy.nArmor = 10;
                enemy.nFalseAim = 3;
                //enemy.inverseMove = true;
                break;

            default:
                enemy.health = 20;
                enemy.nArmor = 15;
                enemy.nFalseAim = 4;
                break;
        }
    }

    // �������� ��� ������� ������ "ATTACK"
    private void AttackButtonAct()
    {
        // ������������� �������� �����
        mgm.GetComponent<Attack>().AttackInit(battleTime, player.GetComponent<Player>().GetDamage(), 
            enemy.health, enemy.nArmor, enemy.nFalseAim, enemy.inverseMove);

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // �������� ��� ������� ������ "DEFENSE"
    private void DefenseButtonAct()
    {
        // ������������� �������� ������
        mgm.GetComponent<Defense>().DefenseInit(battleTime, playerHealth, enemy);

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // �������� ��� ������� ������ "ITEMS"
    private void ItemsButtonAct()
    {
        Debug.Log("Items");
    }

    // �������� ��� ������� ������ "LEAVE"
    private void LeaveButtonAct()
    {
        ExitBattle();
    }

    // �������� ��������� ��������
    IEnumerator CheckEndMinigame()
    {
        // ���� ������ ���� �����
        if (battleStage == 1)
        {
            yield return new WaitWhile(() => !mgm.GetComponent<Attack>().IsEnd());
            enemy.health = mgm.GetComponent<Attack>().GetEnemyHealth();
        }

        // ���� ������ ���� ������
        else if (battleStage == 2)
        {
            yield return new WaitWhile(() => !mgm.GetComponent<Defense>().IsEnd());
            playerHealth = mgm.GetComponent<Defense>().GetPlayerHealth();
        }

        WindowInit();
        CheckEndBattle();
    }

    // �������� ��������� �����
    private void CheckEndBattle()
    {
        if (enemy.health <= 0 || playerHealth <= 0)
        {
            ExitBattle();
        }
    }

    // �������� ���� �����
    private void ExitBattle()
    {
        // ������� ���� �����
        battleBG.SetActive(false);

        // ������� ������ ������
        miniGameCamera.SetActive(false);

        // �������� ���������� ������
        player.SetActive(true);
    }
}
