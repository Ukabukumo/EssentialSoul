using UnityEngine;
using System.Collections;

public class AimFake : Aim
{
    [SerializeField] private AudioClip touchArmorSound;
    private float speed;            // �������� �������
    private bool canShoot = true;   // ����������� ��������
    private SoundManager soundManager;

    // ����� �������� ������
    private string upKey = "";
    private string downKey = "";
    private string leftKey = "";
    private string rightKey = "";

    // ����� �������������� ������
    private string upKeyAlt = "";
    private string downKeyAlt = "";
    private string leftKeyAlt = "";
    private string rightKeyAlt = "";

    private void Start()
    {
        string[] _keys = { "w", "s", "a", "d" };
        string[] _keysAlt = { "up", "down", "left", "right" };

        // ������������ ������ � ������� ������
        for (int i = 0; i < 3; i++)
        {
            int k = Random.Range(i + 1, 4);

            // ������ ������ �������� ������
            string _tmp = _keys[k];
            _keys[k] = _keys[i];
            _keys[i] = _tmp;

            // ������ ������ �������������� ������
            _tmp = _keysAlt[k];
            _keysAlt[k] = _keysAlt[i];
            _keysAlt[i] = _tmp;
        }

        // ������������� ����� �������� ������
        upKey = _keys[0];
        downKey = _keys[1];
        leftKey = _keys[2];
        rightKey = _keys[3];

        // ������������� ����� �������������� ������
        upKeyAlt = _keysAlt[0];
        downKeyAlt = _keysAlt[1];
        leftKeyAlt = _keysAlt[2];
        rightKeyAlt = _keysAlt[3];
    }

    // ������������� ����������� �������
    public void AimFakeInit(SoundManager _soundManager, float _speed)
    {
        soundManager = _soundManager;
        speed = _speed;

        StartCoroutine(Act());
    }

    // �������� �������
    private IEnumerator Act()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            Movement();
            BorderCrossing();
            Shoot();
        }
    }

    // �������� ����������� �������
    protected override void Movement()
    {
        float _x = 0f;
        float _y = 0f;

        // �������� �����
        if ( Input.GetKey(upKey) || Input.GetKey(upKeyAlt) )
        {
            _y += 1f;
        }

        // �������� ����
        if ( Input.GetKey(downKey) || Input.GetKey(downKeyAlt) )
        {
            _y += -1f;
        }

        // �������� �����
        if ( Input.GetKey(leftKey) || Input.GetKey(leftKeyAlt) )
        {
            _x += -1f;
        }

        // �������� ������
        if ( Input.GetKey(rightKey) || Input.GetKey(rightKeyAlt) )
        {
            _x += 1f;
        }

        Vector2 movement = new Vector2(_x, _y);
        transform.Translate(movement * speed * Time.fixedDeltaTime);
    }

    // ��������� ������� � �������� �������
    protected override void Respawn()
    {
        int _angle = Random.Range(0, 360);
        float _radian = _angle * Mathf.PI / 180f;

        float _x = 4f * Mathf.Cos(_radian);
        float _y = 4f * Mathf.Sin(_radian);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }

    // �������� ������� ����� �������
    protected override void BorderCrossing()
    {
        if ((Mathf.Abs(transform.position.y) > 5) || (Mathf.Abs(transform.position.x) > 10))
        {
            Respawn();
        }
    }

    // �������� ��� ��������
    private void Shoot()
    {
        if (Input.GetAxis("Fire1") == 1 && canShoot)
        {
            Respawn();
            canShoot = false;
        }

        // �������� ������� �������, ����� �������� ������ �������
        if (Input.GetAxis("Fire1") == 0)
        {
            canShoot = true;
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������������� � �������
        if (collision.tag == "Armor")
        {
            soundManager.PlaySound(touchArmorSound);

            Respawn();
        }
    }
}
