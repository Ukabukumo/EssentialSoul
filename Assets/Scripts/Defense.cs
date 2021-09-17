using UnityEngine;
using System.Collections;

public class Defense : MonoBehaviour
{
    [SerializeField] private GameObject defenseBGPref;
    [SerializeField] private GameObject miniPlayerPref;
    [SerializeField] private GameObject arrowPref;
    private GameObject storage;
    private GameObject miniPlayer;
    private float defenseTime;
    private int playerHealth;
    private Enemy enemy;

    // ������������� �������� ������
    public void DefenseInit(float _defenseTime, int _playerHealth, Enemy _enemy)
    {
        defenseTime = _defenseTime;
        playerHealth = _playerHealth;
        enemy = _enemy;

        // ��������� ��� �������� �����
        storage = new GameObject("Storage");

        // �������� ����
        Instantiate(defenseBGPref, new Vector3(0f, 0f, -1f), Quaternion.identity, storage.transform);

        // �������� ������ � ��������
        miniPlayer = Instantiate(miniPlayerPref, new Vector3(0f, 0f, -1.1f), Quaternion.identity, storage.transform);

        StartCoroutine("DefenseTimer");

        if (enemy.arrowAttack.active)
        {
            StartCoroutine("SpawnArrow", enemy.arrowAttack.frequency);
        }
    }

    // ������ ������
    private IEnumerator DefenseTimer()
    {
        while (!IsEnd())
        {
            yield return new WaitForFixedUpdate();
            defenseTime -= Time.fixedDeltaTime;
            //Debug.Log(defenseTime);
        }

        ClearScene();
    }

    private IEnumerator SpawnArrow(float _frequency)
    {
        while (!IsEnd())
        {
            GenArrow();
            yield return new WaitForSeconds(_frequency);
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

    // ��������� ������
    private void GenArrow()
    {
        Vector3 _position;
        Quaternion _rotation;
        Vector3 _playerPos = miniPlayer.transform.position;
        float _borderPos = 4f;
        int _side = Random.Range(0, 4);
        int _offset = Random.Range(-1, 2);

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

            // ��-���������
            default:
                _position = new Vector3(-_borderPos, _playerPos.y + _offset, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
        }

        GameObject _arrow = Instantiate(arrowPref, _position, _rotation, storage.transform);
        _arrow.GetComponent<Arrow>().ArrowInit(enemy.arrowAttack.speed);
    }

    // ������� �����
    private void ClearScene()
    {
        if (storage != null)
        {
            Destroy(storage);
        }
    }
}
