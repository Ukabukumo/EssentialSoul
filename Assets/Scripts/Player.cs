using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Animator animator;
    private float distance = 0f;
    private int maxHealth = 5;
    private int health = 5;
    private int damage = 1;
    private bool isMove;
    private int[] inventory;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        // Создаём и очищаем инвентарь
        inventory = new int[16];
        for (int i = 0; i < 16; i++)
        {
            inventory[i] = 0;
        }
    }

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
    }

    // Передвижение игрока
    private void Movement()
    {
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
        else if (moveVertical == 0f && moveHorizontal == 0f)
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
    private bool Interaction()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
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
        float _x = transform.position.x;
        float _y = transform.position.y;
        float _offset = 2f;               // Расстояние смещения при переходе через границу
        float _border = 20f;              // Расстояние от центра до границы локации

        // Правая или левая граница локации
        if (Mathf.Abs(_x) >= _border)
        {
            // Смещение от края при переходе границы
            _x -= _x > 0 ? _offset : -_offset;

            // Зеркальная смена координаты (иллюзия перехода)
            _x = -_x;
        }

        // Верхняя или нижняя граница локации
        else if (Mathf.Abs(_y) >= _border)
        {
            // Смещение от края при переходе границы
            _y -= _y > 0 ? _offset : -_offset;

            // Зеркальная смена координаты (иллюзия перехода)
            _y = -_y;
        }

        // Иллюзия перехода
        transform.position = new Vector3(_x, _y, transform.position.z);
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
        return damage;
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
            if (Interaction())
            {
                Destroy(collision.gameObject);

                SetInventory(1);
            }
        }

        // Проверка соприкосновения с синим цветком
        if (collision.tag == "BlueFlower")
        {
            if (Interaction())
            {
                Destroy(collision.gameObject);

                SetInventory(2);
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

    // Добавление предмета в инвентарь
    public void SetInventory(int _item)
    {
        for (int i = 0; i < 16; i++)
        {
            if (inventory[i] == 0)
            {
                inventory[i] = _item;
                return;
            }
        }
    }
}
