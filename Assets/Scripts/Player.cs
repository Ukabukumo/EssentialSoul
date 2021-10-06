using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Animator animator;
    private float distance = 0f;
    private int maxHealth = 5;
    private int health = 5;
    private int damage = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
    }

    // ������������ ������
    private void Movement()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);

        animator.SetFloat("Vertical", moveVertical);
        animator.SetFloat("Horizontal", moveHorizontal);

        // �������, ��� ����� �� ��������� �� ���������
        if (moveVertical == 0f && moveHorizontal != 0f)
        {
            animator.SetTrigger("stopVertical");
        }

        // �������, ��� ����� �� ��������� �� �����������
        else if (moveHorizontal == 0f && moveVertical != 0f)
        {
            animator.SetTrigger("stopHorizontal");
        }

        // �������, ��� ����� ����������
        else if (moveVertical == 0f && moveHorizontal == 0f)
        {
            animator.SetTrigger("stop");
        }

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

    // ��������� ������������� �������� ������
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
