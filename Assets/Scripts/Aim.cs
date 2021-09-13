using UnityEngine;

public class Aim : MonoBehaviour
{
    private float speed = 5f;
    private int points = 0;
    private bool canShoot = true;

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
        CheckShoot();
    }

    // Движение прицела
    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);
    }

    // Отдаление прицела от цели
    private void Respawn()
    {
        int _angle = Random.Range(0, 360);
        float _x = 4f * Mathf.Cos(_angle);
        float _y = 4f * Mathf.Sin(_angle);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }

    // Проверка попадания в цель
    private void CheckShoot()
    {
        if (Input.GetAxis("Fire1") == 1 && canShoot)
        {
            Vector2 curPosition = new Vector2(transform.position.x, transform.position.y);
            float distance = curPosition.sqrMagnitude;

            // Проверка точности попадания в цель
            if (distance < 0.1)
            {
                points += 3;
            }

            else if (distance < 0.5)
            {
                points += 2;
            }

            else if (distance < 1.5)
            {
                points += 1;
            }

            Respawn();
            canShoot = false;
        }

        // Проверка отжатия клавиши, чтобы избежать лишних исполнений кода
        if (Input.GetAxis("Fire1") == 0)
        {
            canShoot = true;
        }
    }

    // Проверка прохода через границу
    private void BorderCrossing()
    {
        if ( (Mathf.Abs(transform.position.y) > 5) || (Mathf.Abs(transform.position.x) > 10) )
        {
            Respawn();
        }
    }

    // Получение заработанных очков
    public int GetPoints()
    {
        return points;
    }
}
