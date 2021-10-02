using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class Defense : MonoBehaviour
{
    [SerializeField] private GameObject defenseBGPref;
    [SerializeField] private GameObject miniPlayerPref;
    [SerializeField] private GameObject swordPref;
    [SerializeField] private GameObject arrowPref;
    [SerializeField] private GameObject spellPref;
    [SerializeField] private GameObject warningPref;
    private GameObject miniGameUI;
    private GameObject storage;
    private GameObject miniPlayer;
    private float defenseTime;
    private int playerHealth;
    private Enemy enemy;

    // ������������� �������� ������
    public void DefenseInit(float _defenseTime, int _playerHealth, Enemy _enemy, GameObject _miniGameUI)
    {
        defenseTime = _defenseTime;
        playerHealth = _playerHealth;
        enemy = _enemy;
        miniGameUI = _miniGameUI;

        // ��������� ��� �������� �����
        storage = new GameObject("Storage");

        // �������� ����
        Instantiate(defenseBGPref, new Vector3(0f, 0f, -1f), Quaternion.identity, storage.transform);

        // ��������� ���������� � ��������
        miniGameUI.SetActive(true);

        // �������� ������ � ��������
        miniPlayer = Instantiate(miniPlayerPref, new Vector3(0f, 0f, -1.1f), Quaternion.identity, storage.transform);
        miniPlayer.GetComponent<MiniPlayer>().MiniPlayerInit(playerHealth, enemy);

        StartCoroutine("DefenseTimer");

        // ������ ��������� ����
        if (enemy.swordAttack.active)
        {
            StartCoroutine("SpawnSword", enemy.swordAttack.frequency);
        }

        // ������ ��������� �����
        if (enemy.arrowAttack.active)
        {
            StartCoroutine("SpawnArrow", enemy.arrowAttack.frequency);
        }

        // ������ ��������� ����������
        if (enemy.spellAttack.active)
        {
            StartCoroutine("SpawnSpell", enemy.spellAttack.frequency);
        }
    }

    // ������ ������
    private IEnumerator DefenseTimer()
    {
        TextMeshProUGUI _timeInfo = miniGameUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        while (!IsEnd())
        {
            yield return new WaitForFixedUpdate();
            defenseTime -= Time.fixedDeltaTime;
            playerHealth = miniPlayer.GetComponent<MiniPlayer>().GetHealth();
            _timeInfo.text = Convert.ToString(Math.Round(defenseTime, 2));
        }

        ClearScene();
    }

    // �������� ���� � �������� ��������
    private IEnumerator SpawnSword(float _frequency)
    {
        while (!IsEnd())
        {
            GenSword();
            yield return new WaitForSeconds(_frequency);
        }
    }

    // �������� ����� � �������� ��������
    private IEnumerator SpawnArrow(float _frequency)
    {
        while (!IsEnd())
        {
            GenArrow();
            yield return new WaitForSeconds(_frequency);
        }
    }

    // �������� ���������� � �������� ��������
    private IEnumerator SpawnSpell(float _frequency)
    {
        while (!IsEnd())
        {
            GenSpell();
            yield return new WaitForSeconds(_frequency);
        }
    }

    // �������������� ��������� ���������� ��������
    private IEnumerator WarningAttack(float _time, int _type, Vector3 _position, Quaternion _rotation)
    {
        // �������� ���������������� �����
        GameObject _warning = Instantiate(warningPref, new Vector3(_position.x, _position.y, -2f), Quaternion.identity, storage.transform);
        
        // ����� ��������������
        yield return new WaitForSeconds(_time);

        // ����������� ���������������� �����
        if (_warning != null)
        {
            Destroy(_warning);
        }
        
        // �������� ����
        if (_type == 1)
        {
            // ����� �������� ��������� � ��� ������������ �������� ����� ��������� ��������
            if (!IsEnd())
            {
                GameObject _sword = Instantiate(swordPref, _position, _rotation, storage.transform);
                _sword.GetComponent<Sword>().SwordInit(enemy.swordAttack.speed, miniPlayer.transform);
            }
        }

        // �������� ������
        else if (_type == 2)
        {
            // ����� �������� ��������� � ��� ������������ �������� ����� ��������� ��������
            if (!IsEnd())
            {
                GameObject _arrow = Instantiate(arrowPref, _position, _rotation, storage.transform);
                _arrow.GetComponent<Arrow>().ArrowInit(enemy.arrowAttack.speed);
            }
        }

        // �������� ����������
        else if (_type == 3)
        {
            // ����� �������� ��������� � ��� ������������ �������� ����� ��������� ��������
            if (!IsEnd())
            {
                GameObject _spell = Instantiate(spellPref, _position, Quaternion.identity, storage.transform);
                _spell.GetComponent<Spell>().SpellInit(enemy.spellAttack.lifetime);
            }
        }
        
    }

    // �������� ��������� ��������
    public bool IsEnd()
    {
        // ���� ����������� ����� ��� �������� ������ ����� �� ����
        if ( (defenseTime <= 0) || (playerHealth <= 0) )
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // ��������� ���������� �������� ������
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    // ��������� ����
    private void GenSword()
    {
        Vector3 _playerPos = miniPlayer.transform.position;

        float _radius = 1f;

        int _angle = UnityEngine.Random.Range(0, 360);
        float _radian = _angle * Mathf.PI / 180f;

        float _x = _radius * Mathf.Cos(_radian);
        float _y = _radius * Mathf.Sin(_radian);

        Vector3 _position = new Vector3(_playerPos.x + _x, _playerPos.y + _y, _playerPos.z);
        Quaternion _rotation = Quaternion.Euler(0f, 0f, 0f);

        // ��������� ���������������� ����� � �������� �������
        StartCoroutine(WarningAttack(1f, 1, _position, _rotation));
    }

    // ��������� ������
    private void GenArrow()
    {
        Vector3 _position;
        Quaternion _rotation;
        Vector3 _playerPos = miniPlayer.transform.position;
        float _borderPos = 4f;
        int _side = UnityEngine.Random.Range(0, 4);
        int _offset = UnityEngine.Random.Range(-1, 2);

        // ����������� ��������� �������
        switch (_side)
        {
            // ������
            case 0:
                _position = new Vector3(_playerPos.x + _offset, _borderPos, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 270f);
                break;

            // �����
            case 1:
                _position = new Vector3(_playerPos.x + _offset, -_borderPos, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 90f);
                break;

            // �����
            case 2:
                _position = new Vector3(-_borderPos, _playerPos.y + _offset, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 0f);
                break;

            // ������
            case 3:
                _position = new Vector3(_borderPos, _playerPos.y + _offset, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 180f);
                break;

            // ��-��������� (�����)
            default:
                _position = new Vector3(-_borderPos, _playerPos.y + _offset, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
        }

        // ��������� ���������������� ����� � �������� �������
        StartCoroutine(WarningAttack(1f, 2, _position, _rotation));
    }

    // ��������� ����������
    private void GenSpell()
    {
        Vector3 _playerPos = miniPlayer.transform.position;        // ������� ������
        float _diff = enemy.spellAttack.distance;                  // ���������� ������ �� ������
        float _border = 4f;                                        // ������� �������� ����
        float _upBorder, _downBorder, _leftBorder, _rightBorder;   // ������� ���� ��� ������

        // ��������� ���������� �� ������� �������
        if (_border - _playerPos.y >= _diff)
        {
            _upBorder = _diff;
        }

        else
        {
            _upBorder = _border - _playerPos.y;
        }

        // ��������� ���������� �� ������ �������
        if (-_border - _playerPos.y <= -_diff)
        {
            _downBorder = -_diff;
        }

        else
        {
            _downBorder = -_border - _playerPos.y; 
        }

        // ��������� ���������� �� ����� �������
        if (-_border - _playerPos.x <= -_diff)
        {
            _leftBorder = -_diff;
        }

        else
        {
            _leftBorder = -_border - _playerPos.x;
        }

        // ��������� ���������� �� ������ �������
        if (_border -_playerPos.x >= _diff)
        {
            _rightBorder = _diff;
        }

        else
        {
            _rightBorder = _border - _playerPos.x;
        }

        // ��������� ��������� ������� � �������� ���� ������ ������
        float _x = _playerPos.x + UnityEngine.Random.Range(_leftBorder, _rightBorder);
        float _y = _playerPos.y + UnityEngine.Random.Range(_downBorder, _upBorder);
        Vector3 _position = new Vector3(_x, _y, _playerPos.z);

        // ��������� ���������������� ����� � �������� �������
        StartCoroutine(WarningAttack(1f, 3, _position, Quaternion.identity));
    }

    // ������� �����
    private void ClearScene()
    {
        if (storage != null)
        {
            Destroy(storage);
        }

        // ����������� ���������� � ��������
        miniGameUI.SetActive(false);
    }
}
