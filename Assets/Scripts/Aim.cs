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

    // �������� �������
    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);
    }

    // ��������� ������� �� ����
    private void Respawn()
    {
        int _angle = Random.Range(0, 360);
        float _x = 4f * Mathf.Cos(_angle);
        float _y = 4f * Mathf.Sin(_angle);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }

    // �������� ��������� � ����
    private void CheckShoot()
    {
        if (Input.GetAxis("Fire1") == 1 && canShoot)
        {
            Vector2 curPosition = new Vector2(transform.position.x, transform.position.y);
            float distance = curPosition.sqrMagnitude;

            // �������� �������� ��������� � ����
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

        // �������� ������� �������, ����� �������� ������ ���������� ����
        if (Input.GetAxis("Fire1") == 0)
        {
            canShoot = true;
        }
    }

    // �������� ������� ����� �������
    private void BorderCrossing()
    {
        if ( (Mathf.Abs(transform.position.y) > 5) || (Mathf.Abs(transform.position.x) > 10) )
        {
            Respawn();
        }
    }

    // ��������� ������������ �����
    public int GetPoints()
    {
        return points;
    }
}
