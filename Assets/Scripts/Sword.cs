using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    private float speed;   // Скорость движения меча
    private Vector3 startPosition;

    // Установка характеристик меча
    public void SwordInit(float _speed)
    {
        speed = _speed;
        startPosition = transform.position;

        Vector2 movement = new Vector2(0f, transform.localScale.y);
        transform.Translate(movement);

        StartCoroutine("Act");
    }

    // Действия меча
    private IEnumerator Act()
    {
        float _angle = 0;

        // Пока не завершён поворот
        while (_angle < 180)
        {
            yield return new WaitForFixedUpdate();
            _angle += speed;

            Movement();
        }

        // Исчезновение меча
        gameObject.SetActive(false);
    }

    // Движение меча
    private void Movement()
    {
        transform.position = startPosition;
        transform.rotation *= Quaternion.Euler(0f, 0f, speed);

        // Перемещение меча вперёд, чтобы он описывал своим движением окружность,
        // а не вращался на месте
        Vector2 movement = new Vector2(0f, transform.localScale.y);
        transform.Translate(movement);
    }
}
