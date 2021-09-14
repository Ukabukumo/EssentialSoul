using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMain : Aim
{
    private float speed = 5f;
    private bool canShoot = true;

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
    }

    // Движение правильного прицела
    protected override void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);
    }

    // Появление прицела в исходной области
    protected override void Respawn()
    {
        int _angle = Random.Range(0, 360);
        float _x = 4f * Mathf.Cos(_angle);
        float _y = 4f * Mathf.Sin(_angle);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }

    // Проверка прохода через границу
    protected override void BorderCrossing()
    {
        if ((Mathf.Abs(transform.position.y) > 5) || (Mathf.Abs(transform.position.x) > 10))
        {
            Respawn();
        }
    }

    // Выстрел
    public int Shoot()
    {
        int _points = 0;

        if (Input.GetAxis("Fire1") == 1 && canShoot)
        {
            Vector2 curPosition = new Vector2(transform.position.x, transform.position.y);
            float _distance = curPosition.sqrMagnitude;

            // Проверка точности попадания в цель
            if (_distance < 0.1)
            {
                _points = 3;
            }

            else if (_distance < 0.5)
            {
                _points = 2;
            }

            else if (_distance < 1.5)
            {
                _points = 1;
            }

            Respawn();
            canShoot = false;
        }

        // Проверка отжатия клавиши, чтобы избежать лишних исполнений кода
        if (Input.GetAxis("Fire1") == 0)
        {
            canShoot = true;
        }

        return _points;
    }
}
