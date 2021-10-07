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
    private float battleTime;      // Время для текущего этапа
    private int battleStage;       // Этап боя (1 - атака / 2 - защита)
    private Enemy enemy;           // Характеристики противника
    private int playerHealth;      // Здоровье игрока
    private int playerMaxHealth;   // Максимальное здоровье игрока

    private void Start()
    {
        eventSystem = EventSystem.current;
        mgm = Instantiate(miniGameManager);
        battleTime = 10f;

        // Добавление слушателей на кнопки
        attackButton.onClick.AddListener(AttackButtonAct);
        defenseButton.onClick.AddListener(DefenseButtonAct);
        itemsButton.onClick.AddListener(ItemsButtonAct);
        leaveButton.onClick.AddListener(LeaveButtonAct);
    }

    private void Update()
    {
        // Проверка пройденного расстояния
        if (player.GetComponent<Player>().GetDistance() >= 200f)
        {
            player.GetComponent<Player>().ZeroDistance();
            BattleInit();
        }
    }

    // Инициализация битвы
    public void BattleInit()
    {
        // Установка текущего этапа боя
        battleStage = 2;

        // Получение здоровья игрока
        playerHealth = player.GetComponent<Player>().GetHealth();

        // Получение максимального здоровья игрока
        playerMaxHealth = player.GetComponent<Player>().GetMaxHealth();

        // Остановка игрока
        player.SetActive(false);
        miniGameCamera.SetActive(true);

        // Вывод информации о начале боя
        battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU OVERTOOK THE DEMON!";

        CreateEnemy();
        WindowInit();
    }

    // Инициализация окна битвы
    public void WindowInit()
    {
        // Появление экрана битвы
        battleBG.SetActive(true);

        // Индикатор здоровья противника
        enemyHealthInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(enemy.health);

        // Индикатор защиты противника
        enemyArmorInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(enemy.nArmor);

        // Индикатор оружия противника
        swordSign.SetActive(enemy.swordAttack.active);
        arrowSign.SetActive(enemy.arrowAttack.active);
        spellSign.SetActive(enemy.spellAttack.active);

        // Индикатор инверсивного движения
        inverseSign.SetActive(enemy.inverseMove);

        // Индикатор фальшивых прицелов
        aimCloneSign.SetActive(enemy.nFalseAim > 0);
        aimCloneInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 
            Convert.ToString(enemy.nFalseAim);

        // Индикатор здоровья игрока
        playerHealthInfo.GetComponent<TextMeshProUGUI>().text = 
            Convert.ToString(playerMaxHealth + " / " + playerHealth);

        // Индикатор времени на текущий этап боя
        timeInfo.GetComponent<TextMeshProUGUI>().text = "TIME: " + Convert.ToString(battleTime);

        // Если этап атаки
        if (battleStage == 1)
        {
            // Меняем кнопку ЗАЩИТЫ на кнопку АТАКИ
            attackButton.gameObject.SetActive(true);
            defenseButton.gameObject.SetActive(false);

            // Подсветка кнопки АТАКА в меню
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(attackButton.gameObject);
        }

        // Если этап защиты
        else if (battleStage == 2)
        {
            // Меняем кнопку АТАКИ на кнопку ЗАЩИТЫ
            attackButton.gameObject.SetActive(false);
            defenseButton.gameObject.SetActive(true);

            // Подсветка кнопки ЗАЩИТА в меню
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(defenseButton.gameObject);
        }
    }

    // Создание противника
    private void CreateEnemy()
    {
        // Генерация типа противника по характеристикам
        int n = UnityEngine.Random.Range(0, 3);
        enemy = new Enemy();

        switch (n)
        {
            // Простой воин
            case 0:
                enemy.health = 20;
                enemy.nArmor = 5;

                enemy.swordAttack.active = true;
                enemy.swordAttack.power = 1;
                enemy.swordAttack.speed = 3f;
                enemy.swordAttack.frequency = 1f;

                break;

            // Простой лучник
            case 1:
                enemy.health = 15;
                enemy.nArmor = 2;
                enemy.inverseMove = true;

                enemy.arrowAttack.active = true;
                enemy.arrowAttack.power = 1;
                enemy.arrowAttack.speed = 6f;
                enemy.arrowAttack.frequency = 0.5f;
                
                break;

            // Простой заклинатель
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

    // Действие при нажатие кнопки "ATTACK"
    private void AttackButtonAct()
    {
        // Инициализация миниигры атака
        mgm.GetComponent<Attack>().AttackInit(battleTime, player.GetComponent<Player>().GetDamage(), 
            enemy.health, enemy.nArmor, enemy.nFalseAim, enemy.inverseMove, miniGameUI);

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // Действие при нажатие кнопки "DEFENSE"
    private void DefenseButtonAct()
    {
        // Инициализация миниигры защита
        mgm.GetComponent<Defense>().DefenseInit(battleTime, playerHealth, enemy, miniGameUI);

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // Действие при нажатие кнопки "ITEMS"
    private void ItemsButtonAct()
    {
        Debug.Log("Items");
    }

    // Действие при нажатие кнопки "LEAVE"
    private void LeaveButtonAct()
    {
        ExitBattle();
    }

    // Проверка окончания миниигры
    IEnumerator CheckEndMinigame()
    {
        // Если сейчас этап атаки
        if (battleStage == 1)
        {
            yield return new WaitWhile(() => !mgm.GetComponent<Attack>().IsEnd());

            // Вывод нанесённого урона
            battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU DEALTH " + 
                Convert.ToString(enemy.health - mgm.GetComponent<Attack>().GetEnemyHealth()) +
                " DAMAGE!";

            enemy.health = mgm.GetComponent<Attack>().GetEnemyHealth();

            battleStage = 2;
        }

        // Если сейчас этап защиты
        else if (battleStage == 2)
        {
            yield return new WaitWhile(() => !mgm.GetComponent<Defense>().IsEnd());

            // Вывод полученного урона
            battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU TOOK " +
                Convert.ToString(playerHealth - mgm.GetComponent<Defense>().GetPlayerHealth()) +
                " DAMAGE!";

            playerHealth = mgm.GetComponent<Defense>().GetPlayerHealth();

            battleStage = 1;
        }

        WindowInit();
        CheckEndBattle();
    }

    // Проверка окончания битвы
    private void CheckEndBattle()
    {
        // Условие победы
        if (enemy.health <= 0)
        {
            //ExitBattle();
            //StartCoroutine("ExitBattle");
            StartCoroutine("ExitConfirm");
        }

        // Условие проигрыша
        else if (playerHealth <= 0)
        {
            //ExitBattle();
            //StartCoroutine("ExitBattle");
            ClearScene();
            GetComponent<MainMenuManager>().MainMenuInit();
        }
    }

    // Закрытие окна битвы
    private void ExitBattle()
    {
        // Убираем меню битвы
        battleBG.SetActive(false);

        // Убираем вторую камеру
        miniGameCamera.SetActive(false);

        // Передача управления игроку
        player.SetActive(true);
    }

    // Действия перед закрытием окна
    private IEnumerator ExitConfirm()
    {
        // Вывод подсказки об окончании битвы
        battleInfo.GetComponent<TextMeshProUGUI>().text = "## YOU WIN! \n## Press ENTER to exit!";

        // Убираем кнопки
        attackButton.gameObject.SetActive(false);
        defenseButton.gameObject.SetActive(false);
        itemsButton.gameObject.SetActive(false);
        leaveButton.gameObject.SetActive(false);

        // Ожидание нажатия клавиши для окончания битвы
        while (!Input.GetKey("return"))
        {
            yield return new WaitForFixedUpdate();
        }

        // Возвращаем кнопки
        itemsButton.gameObject.SetActive(true);
        leaveButton.gameObject.SetActive(true);

        // Очищаем сцену
        ClearScene();
    }

    // Очистка окна битвы
    private void ClearScene()
    {
        // Убираем меню битвы
        battleBG.SetActive(false);

        // Убираем вторую камеру
        miniGameCamera.SetActive(false);

        // Передача управления игроку
        player.SetActive(true);
    }
}
