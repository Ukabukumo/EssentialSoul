using UnityEngine;
using System.Collections;

public class MiniPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip damageSound;
    private int health;         // Здоровье игрока
    private float speed;        // Скорость игрока
    private float cooldown;     // Время неуязвимости после получения урона
    private Enemy enemy;        // Характеристики противника
    private bool attackable;    // Флаг уязвимости игрока
    private bool isDodger;      // Особый навык "ЛОВКАЧ"
    private bool isBlessed;     // Особый навык "БЛАГОСЛОВЕННЫЙ"
    private Animator animator;
    private SoundManager soundManager;

    // Инициализация игрока
    public void MiniPlayerInit(SoundManager _soundManager, int _health, 
        Enemy _enemy, float _speed, bool _isDodger, bool _isBlessed)
    {
        soundManager = _soundManager;
        health = _health;
        enemy = _enemy;
        speed = _speed;
        isDodger = _isDodger;
        isBlessed = _isBlessed;

        attackable = true;

        // Если активен особый навык благословенный, то увеличиваем время неуязвимости
        if (isBlessed)
        {
            cooldown = 2f;
        }

        else
        {
            cooldown = 1f;
        }

        animator = GetComponent<Animator>();

        StartCoroutine("Act");
    }

    // Действия игрока
    private IEnumerator Act()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            Movement();
        }
    }

    // Неуязвимость после получения урона
    private IEnumerator DamageCooldown()
    {
        attackable = false;
        animator.SetBool("Attackable", false);
        
        yield return new WaitForSeconds(cooldown);
        
        attackable = true;
        animator.SetBool("Attackable", true);
    }

    // Неуязвимость после уклонения
    private IEnumerator DodgeCooldown()
    {
        attackable = false;

        yield return new WaitForSeconds(cooldown);

        attackable = true;
    }

    // Передвижение игрока
    private void Movement()
    {
        // Позиция перед началом движения
        Vector3 _prevPosition = transform.position;

        float _moveHorizontal = Input.GetAxisRaw("Horizontal");
        float _moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 _movement = new Vector2(_moveHorizontal, _moveVertical);
        transform.Translate(_movement * speed * Time.fixedDeltaTime);

        // Если пересёк границу по горизонтали
        if (IsHorizontalBorder())
        {
            transform.position = new Vector3(_prevPosition.x, transform.position.y, transform.position.z);
        }

        // Если пересёк границу по вертикали
        if (IsVerticalBorder())
        {
            transform.position = new Vector3(transform.position.x, _prevPosition.y, transform.position.z);
        }
    }

    // Проверка пересечения границы по горизонтали
    private bool IsHorizontalBorder()
    {
        // Левая или правая граница игрового поля
        if (Mathf.Abs(transform.position.x) > 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // Проверка пересечения границы по вертикали
    private bool IsVerticalBorder()
    {
        // Верхняя или нижняя граница игрового поля
        if (Mathf.Abs(transform.position.y) > 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnTriggerStay2D(Collider2D _other)
    {
        // Если столкнулся со стрелой
        if ( (_other.tag == "Arrow") && attackable )
        {
            if (Dodge())
            {
                StartCoroutine(DodgeCooldown());
                return;
            }

            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            // Звук получения урона
            soundManager.PlaySound(damageSound);

            health -= enemy.arrowAttack.power;
            StartCoroutine(DamageCooldown());
        }

        // Если столкнулся с мечом
        if ( (_other.tag == "Sword") && attackable )
        {
            if (Dodge())
            {
                StartCoroutine(DodgeCooldown());
                return;
            }

            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            // Звук получения урона
            soundManager.PlaySound(damageSound);

            health -= enemy.swordAttack.power;
            StartCoroutine(DamageCooldown());
        }

        // Если столкнулся с заклинанием
        if ( (_other.tag == "Spell") && attackable )
        {
            if (Dodge())
            {
                StartCoroutine(DodgeCooldown());
                return;
            }

            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            // Звук получения урона
            soundManager.PlaySound(damageSound);

            health -= enemy.spellAttack.power;
            StartCoroutine(DamageCooldown());
        }
    }

    // Получение здоровья игрока
    public int GetHealth()
    {
        return health;
    }

    // Уклонение от атаки
    private bool Dodge()
    {
        // Если активен особый навык "ЛОВКАЧ"
        if (isDodger)
        {
            // Шанс увернуться 10%
            if (Random.Range(0, 100) < 10)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }
}
