using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

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
    [SerializeField] private GameObject enemyHealthInfo;
    [SerializeField] private GameObject enemyArmorInfo;
    [SerializeField] private GameObject aimCloneInfo;
    [SerializeField] private GameObject aimCloneSign;
    [SerializeField] private GameObject inverseSign;
    [SerializeField] private GameObject swordSign;
    [SerializeField] private GameObject arrowSign;
    [SerializeField] private GameObject spellSign;
    [SerializeField] private GameObject playerHealthInfo;
    [SerializeField] private GameObject timeInfo;
    [SerializeField] private GameObject miniGameUI;
    [SerializeField] private GameObject battleInfo;
    private EventSystem eventSystem;
    private GameObject mgm;
    private float battleTime;      // ����� ��� �������� �����
    private int battleStage;       // ���� ��� (1 - ����� / 2 - ������)
    private Enemy enemy;           // �������������� ����������
    private int playerHealth;      // �������� ������
    private int playerMaxHealth;   // ������������ �������� ������

    private void Start()
    {
        eventSystem = EventSystem.current;
        mgm = Instantiate(miniGameManager);
        battleTime = 10f;

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

    // ������������� �����
    public void BattleInit()
    {
        // ��������� �������� ����� ���
        battleStage = 2;

        // ��������� �������� ������
        playerHealth = player.GetComponent<Player>().GetHealth();

        // ��������� ������������� �������� ������
        playerMaxHealth = player.GetComponent<Player>().GetMaxHealth();

        // ��������� ������
        player.SetActive(false);
        miniGameCamera.SetActive(true);

        // ����� ���������� � ������ ���
        battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU OVERTOOK THE DEMON!";

        CreateEnemy();
        WindowInit();
    }

    // ������������� ���� �����
    public void WindowInit()
    {
        // ��������� ������ �����
        battleBG.SetActive(true);

        // ��������� �������� ����������
        enemyHealthInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(enemy.health);

        // ��������� ������ ����������
        enemyArmorInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(enemy.nArmor);

        // ��������� ������ ����������
        swordSign.SetActive(enemy.swordAttack.active);
        arrowSign.SetActive(enemy.arrowAttack.active);
        spellSign.SetActive(enemy.spellAttack.active);

        // ��������� ������������ ��������
        inverseSign.SetActive(enemy.inverseMove);

        // ��������� ��������� ��������
        aimCloneSign.SetActive(enemy.nFalseAim > 0);
        aimCloneInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 
            Convert.ToString(enemy.nFalseAim);

        // ��������� �������� ������
        playerHealthInfo.GetComponent<TextMeshProUGUI>().text = 
            Convert.ToString(playerMaxHealth + " / " + playerHealth);

        // ��������� ������� �� ������� ���� ���
        timeInfo.GetComponent<TextMeshProUGUI>().text = "TIME: " + Convert.ToString(battleTime);

        // ���� ���� �����
        if (battleStage == 1)
        {
            // ������ ������ ������ �� ������ �����
            attackButton.gameObject.SetActive(true);
            defenseButton.gameObject.SetActive(false);

            // ��������� ������ ����� � ����
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(attackButton.gameObject);
        }

        // ���� ���� ������
        else if (battleStage == 2)
        {
            // ������ ������ ����� �� ������ ������
            attackButton.gameObject.SetActive(false);
            defenseButton.gameObject.SetActive(true);

            // ��������� ������ ������ � ����
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(defenseButton.gameObject);
        }
    }

    // �������� ����������
    private void CreateEnemy()
    {
        // ��������� ���� ���������� �� ���������������
        int n = UnityEngine.Random.Range(0, 3);
        enemy = new Enemy();

        switch (n)
        {
            // ������� ����
            case 0:
                enemy.health = 20;
                enemy.nArmor = 5;

                enemy.swordAttack.active = true;
                enemy.swordAttack.power = 1;
                enemy.swordAttack.speed = 3f;
                enemy.swordAttack.frequency = 1f;

                break;

            // ������� ������
            case 1:
                enemy.health = 15;
                enemy.nArmor = 2;
                enemy.inverseMove = true;

                enemy.arrowAttack.active = true;
                enemy.arrowAttack.power = 1;
                enemy.arrowAttack.speed = 6f;
                enemy.arrowAttack.frequency = 0.5f;
                
                break;

            // ������� �����������
            case 2:
                enemy.health = 10;
                enemy.nArmor = 2;
                enemy.nFalseAim = 2;

                enemy.spellAttack.active = true;
                enemy.spellAttack.power = 1;
                enemy.spellAttack.lifetime = 5f;
                enemy.spellAttack.frequency = 0.3f;
                enemy.spellAttack.distance = 1f;

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
            enemy.health, enemy.nArmor, enemy.nFalseAim, enemy.inverseMove, miniGameUI);

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // �������� ��� ������� ������ "DEFENSE"
    private void DefenseButtonAct()
    {
        // ������������� �������� ������
        mgm.GetComponent<Defense>().DefenseInit(battleTime, playerHealth, enemy, miniGameUI);

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

            // ����� ���������� �����
            battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU DEALTH " + 
                Convert.ToString(enemy.health - mgm.GetComponent<Attack>().GetEnemyHealth()) +
                " DAMAGE!";

            enemy.health = mgm.GetComponent<Attack>().GetEnemyHealth();

            battleStage = 2;
        }

        // ���� ������ ���� ������
        else if (battleStage == 2)
        {
            yield return new WaitWhile(() => !mgm.GetComponent<Defense>().IsEnd());

            // ����� ����������� �����
            battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU TOOK " +
                Convert.ToString(playerHealth - mgm.GetComponent<Defense>().GetPlayerHealth()) +
                " DAMAGE!";

            playerHealth = mgm.GetComponent<Defense>().GetPlayerHealth();

            battleStage = 1;
        }

        WindowInit();
        CheckEndBattle();
    }

    // �������� ��������� �����
    private void CheckEndBattle()
    {
        // ������� ������
        if (enemy.health <= 0)
        {
            //ExitBattle();
            //StartCoroutine("ExitBattle");
            StartCoroutine("ExitConfirm");
        }

        // ������� ���������
        else if (playerHealth <= 0)
        {
            //ExitBattle();
            //StartCoroutine("ExitBattle");
            ClearScene();
            GetComponent<MainMenuManager>().MainMenuInit();
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

    // �������� ����� ��������� ����
    private IEnumerator ExitConfirm()
    {
        // ����� ��������� �� ��������� �����
        battleInfo.GetComponent<TextMeshProUGUI>().text = "## YOU WIN! \n## Press ENTER to exit!";

        // ������� ������
        attackButton.gameObject.SetActive(false);
        defenseButton.gameObject.SetActive(false);
        itemsButton.gameObject.SetActive(false);
        leaveButton.gameObject.SetActive(false);

        // �������� ������� ������� ��� ��������� �����
        while (!Input.GetKey("return"))
        {
            yield return new WaitForFixedUpdate();
        }

        // ���������� ������
        itemsButton.gameObject.SetActive(true);
        leaveButton.gameObject.SetActive(true);

        // ������� �����
        ClearScene();
    }

    // ������� ���� �����
    private void ClearScene()
    {
        // ������� ���� �����
        battleBG.SetActive(false);

        // ������� ������ ������
        miniGameCamera.SetActive(false);

        // �������� ���������� ������
        player.SetActive(true);
    }
}
