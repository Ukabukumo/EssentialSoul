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

    // Действия стрелы
    private IEnumerator Act()
    {
        // Пока не пересечена граница игрового поля
        while (!IsBorder())
        {
            yield return new WaitForFixedUpdate();

            Movement();
        }

        // Исчезновение стрелы
        gameObject.SetActive(false);
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
