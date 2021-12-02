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
            // Звук начала битвы
            soundManager.PlaySound(startBattle);

            player.GetComponent<Player>().ZeroDistance();
            BattleInit();
        }
    }

    // Инициализация битвы
    public void BattleInit()
    {
        // Установка текущего этапа боя
        // Если есть особый навык "ИНИЦИАТОР"
        if (player.GetComponent<Player>().GetInitiator())
        {
            // Устанавливаем этап битвы - атака
            battleStage = 1;
        }
        
        else
        {
            // Случайно генерируем этап боя
            int _n = UnityEngine.Random.Range(0, 2);
            
            switch (_n)
            {
                // Сгенерирован этап атаки
                case 0:
                    battleStage = 1;
                    break;

                // Сгенерирован этап защиты
                case 1:
                    battleStage = 2;
                    break;

                // По-умолчанию этап атаки
                default:
                    battleStage = 1;
                    break;
            }
        }

        // Получение здоровья игрока
        playerHealth = player.GetComponent<Player>().GetHealth();

        // Получение максимального здоровья игрока
        playerMaxHealth = player.GetComponent<Player>().GetMaxHealth();

        // Остановка игрока
        player.GetComponent<Player>().StopPlayer();
        miniGameCamera.SetActive(true);

        // Вывод информации о начале боя
        battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU OVERTOOK THE DEMON!";

        CreateEnemy();
        WindowInit();

        // Переключение музыки
        soundManager.SetMusic(battleTheme);
        soundManager.PlayMusic();
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
            Convert.ToString(playerHealth + "/" + playerMaxHealth);

        // Индикатор времени на текущий этап боя
        timeInfo.GetComponent<TextMeshProUGUI>().text = Convert.ToString(battleTime);

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

        // Назначение предыдущей кнопки
        lastSelectedObject = eventSystem.currentSelectedGameObject;

        StartCoroutine(Act());
    }

    // Действия в меню
    private IEnumerator Act()
    {
        // Пока меню активно
        while (battleBG.activeSelf)
        {
            yield return null;

            ChangeButton();
        }
    }

    // Создание противника
    private void CreateEnemy()
    {
        // Генерация типа противника по характеристикам
        int _n = UnityEngine.Random.Range(0, 3);
        enemy = new Enemy();

        switch (_n)
        {
            // Простой воин
            case 0:
                enemy.health = 200;
                enemy.nArmor = 5;

                enemy.swordAttack.active = true;
                enemy.swordAttack.power = 10;
                enemy.swordAttack.speed = 3f;
                enemy.swordAttack.frequency = 1f;

                break;

            // Простой лучник
            case 1:
                enemy.health = 150;
                enemy.nArmor = 2;
                enemy.inverseMove = true;

                enemy.arrowAttack.active = true;
                enemy.arrowAttack.power = 10;
                enemy.arrowAttack.speed = 6f;
                enemy.arrowAttack.frequency = 0.5f;
                
                break;

            // Простой заклинатель
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

    // Действие при нажатие кнопки "ATTACK"
    private void AttackButtonAct()
    {
        // Воспроизведение звука нажатия на кнопку
        soundManager.PlaySound(pressButtonSound);

        // Инициализация миниигры атака
        mgm.GetComponent<Attack>().AttackInit(soundManager, battleTime, player.GetComponent<Player>().GetDamage(), 
            enemy.health, enemy.nArmor, enemy.nFalseAim, enemy.inverseMove, miniGameUI, 
            player.GetComponent<Player>().GetAimSpeed());

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // Действие при нажатие кнопки "DEFENSE"
    private void DefenseButtonAct()
    {
        // Воспроизведение звука нажатия на кнопку
        soundManager.PlaySound(pressButtonSound);

        Player _player = player.GetComponent<Player>();
        // Инициализация миниигры защита
        mgm.GetComponent<Defense>().DefenseInit(soundManager, battleTime, playerHealth, enemy, miniGameUI, 
            _player.GetMiniPlayerSpeed(), _player.GetDodger(), _player.GetBlessed());

        battleBG.SetActive(false);
        StartCoroutine("CheckEndMinigame");
    }

    // Действие при нажатие кнопки "ITEMS"
    private void ItemsButtonAct()
    {
        // Воспроизведение звука нажатия на кнопку
        soundManager.PlaySound(pressButtonSound);

        // Активация окна предметов
        GetComponent<ItemsMenuManager>().ItemsMenuInit();

        battleBG.SetActive(false);
    }

    // Действие при нажатие кнопки "LEAVE"
    private void LeaveButtonAct()
    {
        // Воспроизведение звука нажатия на кнопку
        soundManager.PlaySound(pressButtonSound);

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
            battleInfo.GetComponent<TextMeshProUGUI>().text = "YOU DEALT " + 
                Convert.ToString(enemy.health - mgm.GetComponent<Attack>().GetEnemyHealth()) +
                " DAMAGE!";

            enemy.health = mgm.GetComponent<Attack>().GetEnemyHealth();

            // Убираем отрицательное здоровье при выводе информации
            if (enemy.health < 0)
            {
                enemy.health = 0;
            }

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

        // Установка начального времени на миниигру
        battleTime = 10f;

        WindowInit();
        CheckEndBattle();
    }

    // Проверка окончания битвы
    private void CheckEndBattle()
    {
        // Условие победы
        if (enemy.health <= 0)
        {
            StartCoroutine("ExitConfirm");
        }

        // Условие проигрыша
        else if (playerHealth <= 0)
        {
            ExitBattle();
            GetComponent<MainMenuManager>().MainMenuInit();
        }
    }

    // Завершение битвы
    private void ExitBattle()
    {
        // Убираем меню битвы
        battleBG.SetActive(false);

        // Убираем вторую камеру
        miniGameCamera.SetActive(false);

        // Передача управления игроку
        player.SetActive(true);

        // Передача здоровья игроку
        player.GetComponent<Player>().SetHealth(playerHealth);

        // Переключение музыки
        soundManager.SetMusic(mainTheme);
        soundManager.PlayMusic();
    }

    // Действия перед закрытием окна
    private IEnumerator ExitConfirm()
    {
        // Вывод подсказки об окончании битвы
        battleInfo.GetComponent<TextMeshProUGUI>().text = "## YOU WIN! \n## Press ENTER to exit!";

        // Добавление души за победу
        player.GetComponent<Player>().AddSouls(1);

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

        // Завершаем битву
        ExitBattle();
    }

    // Изменение здоровья игрока
    public void SetHealth(int _health)
    {
        playerHealth += _health;

        // Проверка, чтобы здоровье не превосходило максимальное
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
    }

    // Изменение времени на миниигру
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

        // Если время на миниигру закончилось
        if (battleTime <= 0)
        {
            // Смена этапа битвы
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

    // Получение текущего этапа битвы
    public int GetBattleStage()
    {
        return battleStage;
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
