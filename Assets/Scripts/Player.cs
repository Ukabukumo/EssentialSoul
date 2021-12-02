using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject collectIconPref;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip grassSound;
    private Animator animator;
    private float speed = 5f;            // Скорость передвижения игрока
    private float distance = 0f;         // Пройденная дистанция
    private int maxHealth = 100;         // Максимальное здоровье игрока
    private int health = 100;            // Здоровье игрока
    private int damage = 10;             // Урон игрока
    private bool isMove;                 // Состояние движения игрока
    private int[] inventory;             // Инвентарь игрока
    private bool canCollect = true;      // Возможность подбирать предметы
    private float collectDelay = 0.5f;   // Время сбора предмета
    private GameObject collectIcon;
    private GameObject currentItem;      // Текущий предмет для сбора
    private int souls = 20;               // Количество собраных душ
    private float miniPlayerSpeed = 5f;  // Скорость игрока в миниигре
    private float aimSpeed = 5f;         // Скорость прицела
    private bool isInitiator = false;    // Особый навык "ИНИЦИАТОР"
    private bool isCollector = false;    // Особый навык "СБОРЩИК"
    private bool isDodger = false;       // Особый навык "ЛОВКАЧ"
    private bool isBlessed = false;      // Особый навык "БЛАГОСЛОВЕННЫЙ"
    private bool isShooter = false;      // Особый навык "СТРЕЛОК"

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = transform;

        animator = GetComponent<Animator>();
        
        // Создаём и очищаем инвентарь
        inventory = new int[16];
        for (int i = 0; i < 16; i++)
        {
            inventory[i] = 0;
        }
    }

    private void Update()
    {
        PlayerAct();
    }

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
    }

    // Передвижение игрока
    private void Movement()
    {
        if (!canCollect)
        {
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Horizontal", 0f);
            animator.SetTrigger("stop");

            return;
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);

        // Смена уровня фона
        float _z = -1f + (transform.position.y - 20f) / 100f;
        transform.position = new Vector3(transform.position.x, transform.position.y, _z);

        animator.SetFloat("Vertical", moveVertical);
        animator.SetFloat("Horizontal", moveHorizontal);

        // Условие, что игрок не двигается по вертикали
        if (moveVertical == 0f && moveHorizontal != 0f)
        {
            animator.SetTrigger("stopVertical");
        }

        // Условие, что игрок не двигается по горизонтали
        else if (moveHorizontal == 0f && moveVertical != 0f)
        {
            animator.SetTrigger("stopHorizontal");
        }

        // Условие, что игрок неподвижен
        else if ( (moveVertical == 0f && moveHorizontal == 0f))
        {
            animator.SetTrigger("stop");
            isMove = false;
        }

        // Условие, что игрок движется
        if (moveVertical != 0f || moveHorizontal != 0f)
        {
            isMove = true;
        }

        // Расстояние, пройденное игроком
        distance += (float)Math.Round(movement.sqrMagnitude, 3);
    }

    // Взаимодействие игрока
    private bool CollectItem()
    {
        if (Input.GetKey(KeyCode.RightShift) && canCollect)
        {
            StartCoroutine(CollectItem(collectDelay));
            return true;
        }

        else
        {
            return false;
        }
    }

    // Проверка пересечения границы локации
    private void BorderCrossing()
    {
        float _x = playerTransform.position.x;
        float _y = playerTransform.position.y;
        float _offset = 4f;               // Расстояние смещения при переходе через границу
        float _border = 20f;              // Расстояние от центра до границы локации

        // Правая или левая граница локации
        if (Mathf.Abs(_x) >= _border)
        {
            // Смещение от края при переходе границы
            _x -= _x > 0 ? _offset : -_offset;

            // Зеркальная смена координаты (иллюзия перехода)
            _x = -_x;

            // Иллюзия перехода
            transform.position = new Vector3(_x, _y, transform.position.z);

            // Создание новой локации
            gameManager.GetComponent<WorldManager>().CreateLocation(gameObject);
        }

        // Верхняя или нижняя граница локации
        else if (Mathf.Abs(_y) >= _border)
        {
            // Смещение от края при переходе границы
            _y -= _y > 0 ? _offset : -_offset;

            // Зеркальная смена координаты (иллюзия перехода)
            _y = -_y;

            // Иллюзия перехода
            transform.position = new Vector3(_x, _y, transform.position.z);

            // Создание новой локации
            gameManager.GetComponent<WorldManager>().CreateLocation(gameObject);
        }
    }

    // Действия игрока
    private void PlayerAct()
    {
        // Клавиша открытия меню навыков
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Открываем меню
            gameManager.GetComponent<SkillsMenuManager>().SkillsMenuInit();

            // Деактивируем игрока
            StopPlayer();
        }

        // Клавиша открытия игрового меню
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Открываем меню
            gameManager.GetComponent<GameMenuManager>().GameMenuInit();

            // Деактивируем игрока
            StopPlayer();
        }
    }

    // Получение пройденной дистанции
    public float GetDistance()
    {
        return distance;
    }

    // Обнуление пройденной дистанции
    public void ZeroDistance()
    {
        distance = 0f;
    }

    // Получение урона игрока
    public int GetDamage()
    {
        if (isShooter)
        {
            return damage * 2;
        }
        
        else
        {
            return damage;
        }
    }

    // Получение здоровья игрока
    public int GetHealth()
    {
        return health;
    }

    // Получение максимального здоровья игрока
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    // Передача здоровья игроку
    public void SetHealth(int _health)
    {
        health = _health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверка соприкосновения с травой при движении
        if (collision.tag == "Grass")
        {
            // Звук соприкосновения с травой
            soundManager.PlaySound(grassSound);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Проверка соприкосновения с травой при движении
        if (collision.tag == "Grass")
        {
            if (isMove)
            {
                collision.GetComponent<Animator>().SetBool("Touch", true);
            }

            else
            {
                collision.GetComponent<Animator>().SetBool("Touch", false);
            }
        }

        // Проверка соприкосновения с красным цветком
        if (collision.tag == "RedFlower")
        {
            if (CollectItem())
            {
                currentItem = collision.gameObject;
                AddItem(1);
            }
        }

        // Проверка соприкосновения с синим цветком
        if (collision.tag == "BlueFlower")
        {
            if (CollectItem())
            {
                currentItem = collision.gameObject;
                AddItem(2);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Проверка окончания соприкосновения с травой
        if (collision.tag == "Grass")
        {
            collision.GetComponent<Animator>().SetBool("Touch", false);
        }
    }

    // Получение инвенторя игрока
    public int[] GetInventory()
    {
        return inventory;
    }

    // Передача инвентаря игроку
    public void SetInventory(int[] _inventory)
    {
        inventory = _inventory;
    }

    // Добавление предмета в инвентарь
    public void AddItem(int _item)
    {
        // Ищем первую пустую ячейку
        for (int i = 0; i < 16; i++)
        {
            if (inventory[i] == 0)
            {
                inventory[i] = _item;
                break;
            }
        }

        // Если активен особый навык "СБОРЩИК", то дублируем предмет
        if (isCollector)
        {
            // Ищем первую пустую ячейку
            for (int i = 0; i < 16; i++)
            {
                if (inventory[i] == 0)
                {
                    inventory[i] = _item;
                    break;
                }
            }
        }
    }

    // Задержка поднятия предмета
    private IEnumerator CollectItem(float _time)
    {
        // Запрещаем игроку подбирать предметы
        canCollect = false;

        // Появление иконки подбирания объекта
        Vector3 _collectIconPos = transform.position + new Vector3(0f, 0f, -1f);
        collectIcon = Instantiate(collectIconPref, _collectIconPos, Quaternion.identity);

        yield return new WaitForSeconds(_time);

        Destroy(collectIcon);
        Destroy(currentItem);
        canCollect = true;
    }

    // Прекращение всех действий игрока
    public void StopPlayer()
    {
        if (collectIcon != null)
        {
            Destroy(collectIcon);
        }

        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        canCollect = true;

        gameObject.SetActive(false);
    }

    // Получение скорости игрока в миниигре
    public float GetMiniPlayerSpeed()
    {
        return miniPlayerSpeed;
    }

    // Получение скорости прицела
    public float GetAimSpeed()
    {
        return aimSpeed;
    }

    // Получение количества душ игрока
    public int GetSouls()
    {
        return souls;
    }

    // Добавление душ игроку
    public void AddSouls(int _n)
    {
        souls += _n;
    }

    // Добавление урона игроку
    public void AddDamage(int _n)
    {
        damage += _n;
    }

    // Добавление скорости передвижения игрока в миниигре
    public void AddMiniPlayerSpeed(float _n)
    {
        miniPlayerSpeed += _n;
        miniPlayerSpeed = (float)Math.Round(miniPlayerSpeed, 1);
    }

    // Добавление скорости прицела
    public void AddAimSpeed(float _n)
    {
        aimSpeed += _n;
        aimSpeed = (float)Math.Round(aimSpeed, 1);
    }

    public void SetInitiator(bool _value)
    {
        isInitiator = _value;
    }

    public void SetCollector(bool _value)
    {
        isCollector = _value;
    }

    public void SetDodger(bool _value)
    {
        isDodger = _value;
    }

    public void SetBlessed(bool _value)
    {
        isBlessed = _value;
    }

    public void SetShooter(bool _value)
    {
        isShooter = _value;
    }

    public bool GetInitiator()
    {
        return isInitiator;
    }

    public bool GetDodger()
    {
        return isDodger;
    }

    public bool GetBlessed()
    {
        return isBlessed;
    }

    // Сохранение информации об игроке в список
    public ArrayList GetInfo()
    {
        ArrayList _info = new ArrayList();

        _info.Add(speed);
        _info.Add(maxHealth);
        _info.Add(health);
        _info.Add(damage);

        for (int i = 0; i < inventory.Length; i++)
        {
            _info.Add(inventory[i]);
        }

        _info.Add(souls);
        _info.Add(miniPlayerSpeed);
        _info.Add(aimSpeed);
        _info.Add(isInitiator);
        _info.Add(isCollector);
        _info.Add(isDodger);
        _info.Add(isBlessed);
        _info.Add(isShooter);

        return _info;
    }

    // Загрузка информации об игроке из списка
    public void SetInfo(ArrayList _info)
    {
        speed     = float.Parse(_info[0].ToString());
        maxHealth = int.Parse(_info[1].ToString());
        health    = int.Parse(_info[2].ToString());
        damage    = int.Parse(_info[3].ToString());

        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = int.Parse(_info[4 + i].ToString());
        }

        souls           = int.Parse(_info[20].ToString());
        miniPlayerSpeed = float.Parse(_info[21].ToString());
        aimSpeed        = float.Parse(_info[22].ToString());
        isInitiator     = bool.Parse(_info[23].ToString());
        isCollector     = bool.Parse(_info[24].ToString());
        isDodger        = bool.Parse(_info[25].ToString());
        isBlessed       = bool.Parse(_info[26].ToString());
        isShooter       = bool.Parse(_info[27].ToString());
    }
}
