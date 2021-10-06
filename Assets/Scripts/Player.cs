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

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        }

        // Расстояние, пройденное игроком
        distance += (float)Math.Round(movement.sqrMagnitude, 3);
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
}
