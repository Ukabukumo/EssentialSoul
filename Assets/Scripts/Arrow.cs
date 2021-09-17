using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    private float speed;   // Скорость движения стрелы

    // Установка характеристик стрелы
    public void ArrowInit(float _speed)
    {
        speed = _speed;

        StartCoroutine("Act");
    }

    // Движение стрелы
    private void Movement()
    {
        Vector2 _movement = new Vector2(speed, 0f);
        transform.Translate(_movement * Time.fixedDeltaTime);
    }

    private IEnumerator Act()
    {
        while (!IsBorder())
        {
            yield return new WaitForFixedUpdate();

            Movement();
        }

        // Исчезновение стрелы
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }

    // Проверка пересечения границы
    private bool IsBorder()
    {
        // Границы игрового поля
        if ( (Mathf.Abs(transform.position.x) > 5) || (Mathf.Abs(transform.position.y) > 5) )
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
