using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject collectIconPref;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip grassSound;
    private Animator animator;
    private float speed = 5f;            // �������� ������������ ������
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
    private int souls = 20;               // ���������� �������� ���
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

    private void Update()
    {
        PlayerAct();
    }

    private void FixedUpdate()
    {
        Movement();
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            // ��������� ����
            gameManager.GetComponent<SkillsMenuManager>().SkillsMenuInit();

            // ������������ ������
            StopPlayer();
        }

        // ������� �������� �������� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ��������� ����
            gameManager.GetComponent<GameMenuManager>().GameMenuInit();

            // ������������ ������
            StopPlayer();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������� ��������������� � ������ ��� ��������
        if (collision.tag == "Grass")
        {
            // ���� ��������������� � ������
            soundManager.PlaySound(grassSound);
        }
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

    // ���������� ���������� �� ������ � ������
    public ArrayList GetInfo()
    {
        ArrayList _info = new ArrayList();

        _info.Add(speed);
        _info.Add(maxHealth);
        _info.Add(health);
        _info.Add(damage);

        for (int i = 0; i < inventory.Length; i++)
        {
            _info.Add(inventory[i]);
        }

        _info.Add(souls);
        _info.Add(miniPlayerSpeed);
        _info.Add(aimSpeed);
        _info.Add(isInitiator);
        _info.Add(isCollector);
        _info.Add(isDodger);
        _info.Add(isBlessed);
        _info.Add(isShooter);

        return _info;
    }

    // �������� ���������� �� ������ �� ������
    public void SetInfo(ArrayList _info)
    {
        speed     = float.Parse(_info[0].ToString());
        maxHealth = int.Parse(_info[1].ToString());
        health    = int.Parse(_info[2].ToString());
        damage    = int.Parse(_info[3].ToString());

        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = int.Parse(_info[4 + i].ToString());
        }

        souls           = int.Parse(_info[20].ToString());
        miniPlayerSpeed = float.Parse(_info[21].ToString());
        aimSpeed        = float.Parse(_info[22].ToString());
        isInitiator     = bool.Parse(_info[23].ToString());
        isCollector     = bool.Parse(_info[24].ToString());
        isDodger        = bool.Parse(_info[25].ToString());
        isBlessed       = bool.Parse(_info[26].ToString());
        isShooter       = bool.Parse(_info[27].ToString());
    }
}
