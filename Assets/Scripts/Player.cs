using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float distance = 0f;
    private int health = 5;
    private int damage = 1;

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
    }

    // ������������ ������
    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);

        // ����������, ���������� �������
        distance += (float)Math.Round(movement.sqrMagnitude, 3);
    }

    // �������� ����������� ������� �������
    private void BorderCrossing()
    {
        float _x = transform.position.x;
        float _y = transform.position.y;
        float _offset = 2f;               // ���������� �������� ��� �������� ����� �������
        float _border = 20f;              // ���������� �� ������ �� ������� �������

        // ������ ��� ����� ������� �������
        if (Mathf.Abs(_x) >= _border)
        {
            // �������� �� ���� ��� �������� �������
            _x -= _x > 0 ? _offset : -_offset;

            // ���������� ����� ���������� (������� ��������)
            _x = -_x;
        }

        // ������� ��� ������ ������� �������
        else if (Mathf.Abs(_y) >= _border)
        {
            // �������� �� ���� ��� �������� �������
            _y -= _y > 0 ? _offset : -_offset;

            // ���������� ����� ���������� (������� ��������)
            _y = -_y;
        }

        // ������� ��������
        transform.position = new Vector3(_x, _y, transform.position.z);
    }

    // ��������� ���������� ���������
    public float GetDistance()
    {
        return distance;
    }

    // ��������� ���������� ���������
    public void ZeroDistance()
    {
        distance = 0f;
    }

    // ��������� ����� ������
    public int GetDamage()
    {
        return damage;
    }

    // ��������� �������� ������
    public int GetHealth()
    {
        return health;
    }
}
