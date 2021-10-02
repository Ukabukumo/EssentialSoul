using UnityEngine;
using System.Collections;

public class MiniPlayer : MonoBehaviour
{
    private int health;         // �������� ������
    private float speed = 5f;   // �������� ������
    private float cooldown;     // ����� ������������ ����� ��������� �����
    private Enemy enemy;        // �������������� ����������
    private bool attackable;    // ���� ���������� ������
    private Animator animator;

    // ������������� ������
    public void MiniPlayerInit(int _health, Enemy _enemy)
    {
        health = _health;
        enemy = _enemy;

        attackable = true;
        cooldown = 1f;

        animator = GetComponent<Animator>();

        StartCoroutine("Act");
    }

    // �������� ������
    private IEnumerator Act()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            Movement();
        }
    }

    // ������������ ������
    private IEnumerator DamageCooldown()
    {
        animator.SetBool("Attackable", false);
        
        yield return new WaitForSeconds(cooldown);
        
        attackable = true;
        animator.SetBool("Attackable", true);
    }

    // ������������ ������
    private void Movement()
    {
        // ������� ����� ������� ��������
        Vector3 _prevPosition = transform.position;

        float _moveHorizontal = Input.GetAxisRaw("Horizontal");
        float _moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 _movement = new Vector2(_moveHorizontal, _moveVertical);
        transform.Translate(_movement * speed * Time.fixedDeltaTime);

        // ���� ������ ������� �� �����������
        if (IsHorizontalBorder())
        {
            transform.position = new Vector3(_prevPosition.x, transform.position.y, transform.position.z);
        }

        // ���� ������ ������� �� ���������
        if (IsVerticalBorder())
        {
            transform.position = new Vector3(transform.position.x, _prevPosition.y, transform.position.z);
        }
    }

    // �������� ����������� ������� �� �����������
    private bool IsHorizontalBorder()
    {
        // ����� ��� ������ ������� �������� ����
        if (Mathf.Abs(transform.position.x) > 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // �������� ����������� ������� �� ���������
    private bool IsVerticalBorder()
    {
        // ������� ��� ������ ������� �������� ����
        if (Mathf.Abs(transform.position.y) > 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnTriggerStay2D(Collider2D _other)
    {
        // ���� ���������� �� �������
        if ( (_other.tag == "Arrow") && attackable )
        {
            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            health -= enemy.arrowAttack.power;
            attackable = false;
            StartCoroutine("DamageCooldown");
        }

        // ���� ���������� � �����
        if ( (_other.tag == "Sword") && attackable )
        {
            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            health -= enemy.swordAttack.power;
            attackable = false;
            StartCoroutine("DamageCooldown");
        }

        // ���� ���������� � �����������
        if ( (_other.tag == "Spell") && attackable )
        {
            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            health -= enemy.spellAttack.power;

            attackable = false;
            StartCoroutine("DamageCooldown");
        }
    }

    // ��������� �������� ������
    public int GetHealth()
    {
        return health;
    }
}
