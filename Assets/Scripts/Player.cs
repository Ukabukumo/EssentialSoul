using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject collectIconPref;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private float speed = 5f;  // �������� ������������ ������
    private Animator animator;
    private float distance = 0f;         // ���������� ���������
    private int maxHealth = 100;         // ������������ �������� ������
    private int health = 100;            // �������� ������
    private int damage = 10;             // ���� ������
    private bool isMove;                 // ��������� �������� ������
    private int[] inventory;             // ��������� ������
    private bool canCollect = true;      // ����������� ��������� ��������
    private float collectDelay = 0.5f;   // ����� ����� ��������
    private GameObject collectIcon;
    private GameObject currentItem;      // ������� ������� ��� �����
    private int souls = 0;               // ���������� �������� ���
    private float miniPlayerSpeed = 5f;  // �������� ������ � ��������
    private float aimSpeed = 5f;         // �������� �������
    private bool isInitiator = false;    // ������ ����� "���������"
    private bool isCollector = false;    // ������ ����� "�������"
    private bool isDodger = false;       // ������ ����� "������"
    private bool isBlessed = false;      // ������ ����� "��������������"
    private bool isShooter = false;      // ������ ����� "�������"

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = transform;

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
        PlayerAct();
        BorderCrossing();
    }

    // ������������ ������
    private void Movement()
    {
        if (!canCollect)
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
    private bool CollectItem()
    {
        if (Input.GetKey(KeyCode.RightShift) && canCollect)
        {
            StartCoroutine(CollectItem(collectDelay));
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
        float _x = playerTransform.position.x;
        float _y = playerTransform.position.y;
        float _offset = 4f;               // ���������� �������� ��� �������� ����� �������
        float _border = 20f;              // ���������� �� ������ �� ������� �������

        // ������ ��� ����� ������� �������
        if (Mathf.Abs(_x) >= _border)
        {
            // �������� �� ���� ��� �������� �������
            _x -= _x > 0 ? _offset : -_offset;

            // ���������� ����� ���������� (������� ��������)
            _x = -_x;

            // ������� ��������
            transform.position = new Vector3(_x, _y, transform.position.z);

            // �������� ����� �������
            gameManager.GetComponent<WorldManager>().CreateLocation(gameObject);
        }

        // ������� ��� ������ ������� �������
        else if (Mathf.Abs(_y) >= _border)
        {
            // �������� �� ���� ��� �������� �������
            _y -= _y > 0 ? _offset : -_offset;

            // ���������� ����� ���������� (������� ��������)
            _y = -_y;

            // ������� ��������
            transform.position = new Vector3(_x, _y, transform.position.z);

            // �������� ����� �������
            gameManager.GetComponent<WorldManager>().CreateLocation(gameObject);
        }
    }

    // �������� ������
    private void PlayerAct()
    {
        // ������� �������� ���� �������
        if (Input.GetKey(KeyCode.C))
        {
            gameManager.GetComponent<SkillsMenuManager>().SkillsMenuInit();
            gameObject.SetActive(false);
        }
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
        if (isShooter)
        {
            return damage * 2;
        }
        
        else
        {
            return damage;
        }
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
            if (CollectItem())
            {
                currentItem = collision.gameObject;
                AddItem(1);
            }
        }

        // �������� ��������������� � ����� �������
        if (collision.tag == "BlueFlower")
        {
            if (CollectItem())
            {
                currentItem = collision.gameObject;
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

    // �������� ��������� ������
    public void SetInventory(int[] _inventory)
    {
        inventory = _inventory;
    }

    // ���������� �������� � ���������
    public void AddItem(int _item)
    {
        // ���� ������ ������ ������
        for (int i = 0; i < 16; i++)
        {
            if (inventory[i] == 0)
            {
                inventory[i] = _item;
                break;
            }
        }

        // ���� ������� ������ ����� "�������", �� ��������� �������
        if (isCollector)
        {
            // ���� ������ ������ ������
            for (int i = 0; i < 16; i++)
            {
                if (inventory[i] == 0)
                {
                    inventory[i] = _item;
                    break;
                }
            }
        }
    }

    // �������� �������� ��������
    private IEnumerator CollectItem(float _time)
    {
        // ��������� ������ ��������� ��������
        canCollect = false;

        // ��������� ������ ���������� �������
        Vector3 _collectIconPos = transform.position + new Vector3(0f, 0f, -1f);
        collectIcon = Instantiate(collectIconPref, _collectIconPos, Quaternion.identity);

        yield return new WaitForSeconds(_time);

        Destroy(collectIcon);
        Destroy(currentItem);
        canCollect = true;
    }

    // ����������� ���� �������� ������
    public void StopPlayer()
    {
        if (collectIcon != null)
        {
            Destroy(collectIcon);
        }

        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        canCollect = true;

        gameObject.SetActive(false);
    }

    // ��������� �������� ������ � ��������
    public float GetMiniPlayerSpeed()
    {
        return miniPlayerSpeed;
    }

    // ��������� �������� �������
    public float GetAimSpeed()
    {
        return aimSpeed;
    }

    // ��������� ���������� ��� ������
    public int GetSouls()
    {
        return souls;
    }

    // ���������� ��� ������
    public void AddSouls(int _n)
    {
        souls += _n;
    }

    // ���������� ����� ������
    public void AddDamage(int _n)
    {
        damage += _n;
    }

    // ���������� �������� ������������ ������ � ��������
    public void AddMiniPlayerSpeed(float _n)
    {
        miniPlayerSpeed += _n;
        miniPlayerSpeed = (float)Math.Round(miniPlayerSpeed, 1);
    }

    // ���������� �������� �������
    public void AddAimSpeed(float _n)
    {
        aimSpeed += _n;
        aimSpeed = (float)Math.Round(aimSpeed, 1);
    }

    public void SetInitiator(bool _value)
    {
        isInitiator = _value;
    }

    public void SetCollector(bool _value)
    {
        isCollector = _value;
    }

    public void SetDodger(bool _value)
    {
        isDodger = _value;
    }

    public void SetBlessed(bool _value)
    {
        isBlessed = _value;
    }

    public void SetShooter(bool _value)
    {
        isShooter = _value;
    }

    public bool GetInitiator()
    {
        return isInitiator;
    }

    public bool GetDodger()
    {
        return isDodger;
    }

    public bool GetBlessed()
    {
        return isBlessed;
    }
}
