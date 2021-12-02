using UnityEngine;
using System.Collections;

public class MiniPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip damageSound;
    private int health;         // �������� ������
    private float speed;        // �������� ������
    private float cooldown;     // ����� ������������ ����� ��������� �����
    private Enemy enemy;        // �������������� ����������
    private bool attackable;    // ���� ���������� ������
    private bool isDodger;      // ������ ����� "������"
    private bool isBlessed;     // ������ ����� "��������������"
    private Animator animator;
    private SoundManager soundManager;

    // ������������� ������
    public void MiniPlayerInit(SoundManager _soundManager, int _health, 
        Enemy _enemy, float _speed, bool _isDodger, bool _isBlessed)
    {
        soundManager = _soundManager;
        health = _health;
        enemy = _enemy;
        speed = _speed;
        isDodger = _isDodger;
        isBlessed = _isBlessed;

        attackable = true;

        // ���� ������� ������ ����� ��������������, �� ����������� ����� ������������
        if (isBlessed)
        {
            cooldown = 2f;
        }

        else
        {
            cooldown = 1f;
        }

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

    // ������������ ����� ��������� �����
    private IEnumerator DamageCooldown()
    {
        attackable = false;
        animator.SetBool("Attackable", false);
        
        yield return new WaitForSeconds(cooldown);
        
        attackable = true;
        animator.SetBool("Attackable", true);
    }

    // ������������ ����� ���������
    private IEnumerator DodgeCooldown()
    {
        attackable = false;

        yield return new WaitForSeconds(cooldown);

        attackable = true;
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
            if (Dodge())
            {
                StartCoroutine(DodgeCooldown());
                return;
            }

            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            // ���� ��������� �����
            soundManager.PlaySound(damageSound);

            health -= enemy.arrowAttack.power;
            StartCoroutine(DamageCooldown());
        }

        // ���� ���������� � �����
        if ( (_other.tag == "Sword") && attackable )
        {
            if (Dodge())
            {
                StartCoroutine(DodgeCooldown());
                return;
            }

            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            // ���� ��������� �����
            soundManager.PlaySound(damageSound);

            health -= enemy.swordAttack.power;
            StartCoroutine(DamageCooldown());
        }

        // ���� ���������� � �����������
        if ( (_other.tag == "Spell") && attackable )
        {
            if (Dodge())
            {
                StartCoroutine(DodgeCooldown());
                return;
            }

            if (_other.gameObject != null)
            {
                Destroy(_other.gameObject);
            }

            // ���� ��������� �����
            soundManager.PlaySound(damageSound);

            health -= enemy.spellAttack.power;
            StartCoroutine(DamageCooldown());
        }
    }

    // ��������� �������� ������
    public int GetHealth()
    {
        return health;
    }

    // ��������� �� �����
    private bool Dodge()
    {
        // ���� ������� ������ ����� "������"
        if (isDodger)
        {
            // ���� ���������� 10%
            if (Random.Range(0, 100) < 10)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }
}
