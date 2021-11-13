using UnityEngine;
using System.Collections;

public class AimMain : Aim
{
    private float speed;            // Скорость прицела
    private bool canShoot = true;   // Возможность выстрела

    // Инициализация основного прицела
    public void AimMainInit(float _speed)
    {
        speed = _speed;
        StartCoroutine(Act());
    }

    // Действия прицела
    private IEnumerator Act()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            Movement();
            BorderCrossing();
        }
    }

    // Движение правильного прицела
    protected override void Movement()
    {
        float _moveHorizontal = Input.GetAxisRaw("Horizontal");
        float _moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 _movement = new Vector2(_moveHorizontal, _moveVertical);
        transform.Translate(_movement * speed * Time.fixedDeltaTime);
    }

    // Появление прицела в исходной области
    protected override void Respawn()
    {
        int _angle = Random.Range(0, 360);
        float _radian = _angle * Mathf.PI / 180f;
        
        float _x = 4f * Mathf.Cos(_radian);
        float _y = 4f * Mathf.Sin(_radian);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }

    // Проверка пересечения границы игрового поля
    protected override void BorderCrossing()
    {
        // Границы игрового поля
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

        // Проверка отжатия клавиши, чтобы избежать лишних нажатий
        if (Input.GetAxis("Fire1") == 0)
        {
            canShoot = true;
        }

        return _points;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если соприкоснулся с защитой
        if (collision.tag == "Armor")
        {
            // Удаление единицы защиты
            if (collision.gameObject != null)
            {
                Destroy(collision.gameObject);
            }

            Respawn();
        }
    }

    // Устанавливаем инверсивное движение прицела
    public void SetInverseMove()
    {
        speed = -speed;
    }
}
