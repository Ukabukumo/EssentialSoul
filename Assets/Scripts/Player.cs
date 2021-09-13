using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float distance = 0f;
    private bool canMove = true;

    private void FixedUpdate()
    {
        if (canMove)
        {
            Movement();
        }

        BorderCrossing();
    }

    // Передвижение игрока
    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);

        // Расстояние, пройденное игроком
        distance += (float)Math.Round(movement.sqrMagnitude, 3);
    }

    // Проверка перехода через границу локации
    private void BorderCrossing()
    {
        float _x = transform.position.x;
        float _y = transform.position.y;
        float _offset = 1f;               // Расстояние смещения при переходе через границу
        float _border = 18f;              // Расстояние от центра до границы локации

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

    public float GetDistance()
    {
        return distance;
    }

    public void ZeroDistance()
    {
        distance = 0f;
    }
}
