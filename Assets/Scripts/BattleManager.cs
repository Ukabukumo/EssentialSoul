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
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip battleTheme;
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip startBattle;
    [SerializeField] private AudioClip changeButtonSound;
    [SerializeField] private AudioClip pressButtonSound;

    private EventSystem eventSystem;
    private GameObject lastSelectedObject;
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
            // ���� ������ �����
            soundManager.PlaySound(startBattle);

            player.GetComponent<Player>().ZeroDistance();
            BattleInit();
        }
    }

    // ������������� �����
    public void BattleInit()
    {
        // ��������� �������� ����� ���
        // ���� ���� ������ ����� "���������"
        if (player.GetComponent<Player>().GetInitiator())
        {
            // ������������� ���� ����� - �����
            battleStage = 1;
        }
        
        else
        {
            // �������� ���������� ���� ���
            int _n = UnityEngine.Random.Range(0, 2);
            
            switch (_n)
            {
                // ������������ ���� �����
                case 0:
                    battleStage = 1;
                    break;

                // ������������ ���� ������
                case 1:
                    battleStage = 2;
                    break;

                // ��-��������� ���� �����
                default:
                    battleStage = 1;
                    break;
            }
        }

        // ��������� �������� ������
        playerHealth = player.GetComponent<Player>().GetHealth();

        // ��������� ������������� �������� ������
        playerMaxHealth = player.GetComponent<Player>().GetMaxHealth();

        // ��������� ������
        player.GetComponent<Player>().StopPlayer();
        miniGameCamera.SetActive(true);

        // ����� ���������� � ������ ���
        battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU OVERTOOK THE DEMON!";

        CreateEnemy();
        WindowInit();

        // ������������ ������
        soundManager.SetMusic(battleTheme);
        soundManager.PlayMusic();
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
            Convert.ToString(playerHealth + "/" + playerMaxHealth);

        // ��������� ������� �� ������� ���� ���
        timeInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(battleTime);

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

        // ���������� ���������� ������
        lastSelectedObject = eventSystem.currentSelectedGameObject;

        StartCoroutine(Act());
    }

    // �������� � ����
    private IEnumerator Act()
    {
        // ���� ���� �������
        while (battleBG.activeSelf)
        {
            yield return null;

            ChangeButton();
        }
    }

    // �������� ����������
    private void CreateEnemy()
    {
        // ��������� ���� ���������� �� ���������������
        int _n = UnityEngine.Random.Range(0, 3);
        enemy = new Enemy();

        switch (_n)
        {
            // ������� ����
            case 0:
                enemy.health = 200;
                enemy.nArmor = 5;

                enemy.swordAttack.active = true;
                enemy.swordAttack.power = 10;
                enemy.swordAttack.speed = 3f;
                enemy.swordAttack.frequency = 1f;

                break;

            // ������� ������
            case 1:
                enemy.health = 150;
                enemy.nArmor = 2;
                enemy.inverseMove = true;

                enemy.arrowAttack.active = true;
                enemy.arrowAttack.power = 10;
                enemy.arrowAttack.speed = 6f;
                enemy.arrowAttack.frequency = 0.5f;
                
                break;

            // ������� �����������
            case 2:
                enemy.health = 100;
                enemy.nArmor = 2;
                enemy.nFalseAim = 2;

                enemy.spellAttack.active = true;
                enemy.spellAttack.power = 10;
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
        // ��������������� ����� ������� �� ������
        soundManager.PlaySound(pressButtonSound);

        // ������������� �������� �����
        mgm.GetComponent<Attack>().AttackInit(soundManager, battleTime, player.GetComponent<Player>().GetDamage(), 
            enemy.health, enemy.nArmor, enemy.nFalseAim, enemy.inverseMove, miniGameUI, 
            player.GetComponent<Player>().GetAimSpeed());

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // �������� ��� ������� ������ "DEFENSE"
    private void DefenseButtonAct()
    {
        // ��������������� ����� ������� �� ������
        soundManager.PlaySound(pressButtonSound);

        Player _player = player.GetComponent<Player>();
        // ������������� �������� ������
        mgm.GetComponent<Defense>().DefenseInit(soundManager, battleTime, playerHealth, enemy, miniGameUI, 
            _player.GetMiniPlayerSpeed(), _player.GetDodger(), _player.GetBlessed());

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // �������� ��� ������� ������ "ITEMS"
    private void ItemsButtonAct()
    {
        // ��������������� ����� ������� �� ������
        soundManager.PlaySound(pressButtonSound);

        // ��������� ���� ���������
        GetComponent<ItemsMenuManager>().ItemsMenuInit();

        battleBG.SetActive(false);
    }

    // �������� ��� ������� ������ "LEAVE"
    private void LeaveButtonAct()
    {
        // ��������������� ����� ������� �� ������
        soundManager.PlaySound(pressButtonSound);

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
            battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU DEALT " + 
                Convert.ToString(enemy.health - mgm.GetComponent<Attack>().GetEnemyHealth()) +
                " DAMAGE!";

            enemy.health = mgm.GetComponent<Attack>().GetEnemyHealth();

            // ������� ������������� �������� ��� ������ ����������
            if (enemy.health < 0)
            {
                enemy.health = 0;
            }

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

        // ��������� ���������� ������� �� ��������
        battleTime = 10f;

        WindowInit();
        CheckEndBattle();
    }

    // �������� ��������� �����
    private void CheckEndBattle()
    {
        // ������� ������
        if (enemy.health <= 0)
        {
            StartCoroutine("ExitConfirm");
        }

        // ������� ���������
        else if (playerHealth <= 0)
        {
            ExitBattle();
            GetComponent<MainMenuManager>().MainMenuInit();
        }
    }

    // ���������� �����
    private void ExitBattle()
    {
        // ������� ���� �����
        battleBG.SetActive(false);

        // ������� ������ ������
        miniGameCamera.SetActive(false);

        // �������� ���������� ������
        player.SetActive(true);

        // �������� �������� ������
        player.GetComponent<Player>().SetHealth(playerHealth);

        // ������������ ������
        soundManager.SetMusic(mainTheme);
        soundManager.PlayMusic();
    }

    // �������� ����� ��������� ����
    private IEnumerator ExitConfirm()
    {
        // ����� ��������� �� ��������� �����
        battleInfo.GetComponent<TextMeshProUGUI>().text = "## YOU WIN! \n## Press ENTER to exit!";

        // ���������� ���� �� ������
        player.GetComponent<Player>().AddSouls(1);

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

        // ��������� �����
        ExitBattle();
    }

    // ��������� �������� ������
    public void SetHealth(int _health)
    {
        playerHealth += _health;

        // ��������, ����� �������� �� ������������ ������������
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
    }

    // ��������� ������� �� ��������
    public void ChangeBattleTime(float _time)
    {
        if (battleStage == 1)
        {
            battleTime -= _time;
        }

        else if (battleStage == 2)
        {
            battleTime += _time;
        }

        // ���� ����� �� �������� �����������
        if (battleTime <= 0)
        {
            // ����� ����� �����
            if (battleStage == 1)
            {
                battleStage = 2;
            }

            else if (battleStage == 2)
            {
                battleStage = 1;
            }

            battleTime = 10f;
        }
    }

    // ��������� �������� ����� �����
    public int GetBattleStage()
    {
        return battleStage;
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
