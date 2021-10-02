using UnityEngine;
using System.Collections;

public class MiniPlayer : MonoBehaviour
{
    private int health;         // Здоровье игрока
    private float speed = 5f;   // Скорость игрока
    private float cooldown;     // Время неуязвимости после получения урона
    private Enemy enemy;        // Характеристики противника
    private bool attackable;    // Флаг уязвимости игрока
    private Animator animator;

    // Инициализация игрока
    public void MiniPlayerInit(int _health, Enemy _enemy)
    {
        health = _health;
        enemy = _enemy;

        attackable = true;
        cooldown = 1f;

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

    // Неуязвимость игрока
    private IEnumerator DamageCooldown()
    {
        animator.SetBool("Attackable", false);
        
        yield return new WaitForSeconds(cooldown);
        
        attackable = true;
        animator.SetBool("Attackable", true);
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
            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            health -= enemy.arrowAttack.power;
            attackable = false;
            StartCoroutine("DamageCooldown");
        }

        // Если столкнулся с мечом
        if ( (_other.tag == "Sword") && attackable )
        {
            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            health -= enemy.swordAttack.power;
            attackable = false;
            StartCoroutine("DamageCooldown");
        }

        // Если столкнулся с заклинанием
        if ( (_other.tag == "Spell") && attackable )
        {
            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            health -= enemy.spellAttack.power;

            attackable = false;
            StartCoroutine("DamageCooldown");
        }
    }

    // Получение здоровья игрока
    public int GetHealth()
    {
        return health;
    }
}
