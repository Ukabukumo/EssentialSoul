using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Animator animator;
    private float distance = 0f;
    private int maxHealth = 5;
    private int health = 5;
    private int damage = 1;
    private bool isMove;
    private int[] inventory;
    private bool canInteract = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        // ������ � ������� ���������
        inventory = new int[16];
        for (int i = 0; i < 16; i++)
        {
            inventory[i] = 0;
        }
    }

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
    }

    // ������������ ������
    private void Movement()
    {
        if (speed <= 0f)
        {
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Horizontal", 0f);
            animator.SetTrigger("stop");

            return;
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);

        // ����� ������ ����
        float _z = -1f + (transform.position.y - 20f) / 100f;
        transform.position = new Vector3(transform.position.x, transform.position.y, _z);

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
        else if ( (moveVertical == 0f && moveHorizontal == 0f))
        {
            animator.SetTrigger("stop");
            isMove = false;
        }

        // �������, ��� ����� ��������
        if (moveVertical != 0f || moveHorizontal != 0f)
        {
            isMove = true;
        }

        // ����������, ���������� �������
        distance += (float)Math.Round(movement.sqrMagnitude, 3);
    }

    // �������������� ������
    private bool Interaction()
    {
        if (Input.GetKey(KeyCode.RightShift) && canInteract)
        {
            StartCoroutine(CollectItem(1f));
            return true;
        }

        else
        {
            return false;
        }
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

    // �������� �������� ������
    public void SetHealth(int _health)
    {
        health = _health;
    }    

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �������� ��������������� � ������ ��� ��������
        if (collision.tag == "Grass")
        {
            if (isMove)
            {
                collision.GetComponent<Animator>().SetBool("Touch", true);
            }

            else
            {
                collision.GetComponent<Animator>().SetBool("Touch", false);
            }
        }

        // �������� ��������������� � ������� �������
        if (collision.tag == "RedFlower")
        {
            if (Interaction())
            {
                Destroy(collision.gameObject);

                AddItem(1);
            }
        }

        // �������� ��������������� � ����� �������
        if (collision.tag == "BlueFlower")
        {
            if (Interaction())
            {
                Destroy(collision.gameObject);

                AddItem(2);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �������� ��������� ��������������� � ������
        if (collision.tag == "Grass")
        {
            collision.GetComponent<Animator>().SetBool("Touch", false);
        }
    }

    // ��������� ��������� ������
    public int[] GetInventory()
    {
        return inventory;
    }

    public void SetInventory(int[] _inventory)
    {
        inventory = _inventory;
    }

    // ���������� �������� � ���������
    public void AddItem(int _item)
    {
        for (int i = 0; i < 16; i++)
        {
            if (inventory[i] == 0)
            {
                inventory[i] = _item;
                return;
            }
        }
    }

    // �������� �������� ��������
    private IEnumerator CollectItem(float _time)
    {
        // ������������� ������
        float _speed = speed;
        speed = 0f;

        canInteract = false;

        yield return new WaitForSeconds(_time);
        
        canInteract = true;
        speed = _speed;
    }
}
