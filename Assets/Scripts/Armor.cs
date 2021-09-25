using UnityEngine;

public class Armor : MonoBehaviour
{
    private float angle = 0f;
    private float radian = 0f;
    private float speed = 1f;
    private float radius = 2.5f;
    private float direction = 1f;

    private void Start()
    {
        Spawn();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    // Движение защиты
    private void Movement()
    {
        angle = angle + direction * speed;
        radian = angle * Mathf.PI / 180f;

        float _x = radius * Mathf.Cos(radian);
        float _y = radius * Mathf.Sin(radian);

        Vector2 movement = new Vector2(_x, _y);
        transform.position = new Vector3(_x, _y, transform.position.z);
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    // Появление защиты
    private void Spawn()
    {
        // Случайный угол
        angle = Random.Range(0, 360);

        // Случайный угол в радианах
        radian = angle * Mathf.PI / 180f;

        // Определение радиуса окружности, по которой движется защита
        radius = (float)Random.Range(0, 2) + 1.5f;

        // Определение направления движения защиты
        switch (Random.Range(0, 2))
        {
            // Против часовой стрелки
            case 0:
                direction = 1f;
                break;

            // По часовой стрелке
            case 1:
                direction = -1f;
                break;

            // Против часовой стрелки (по-умолчанию)
            default:
                direction = 1f;
                break;
        }

        // Определение скорости движения защиты
        speed = Random.Range(1, 4);

        float _x = radius * Mathf.Cos(radian);
        float _y = radius * Mathf.Sin(radian);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }
}
