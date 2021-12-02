using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject aimMainPref;
    [SerializeField] private GameObject aimFakePref;
    [SerializeField] private GameObject targetPref;
    [SerializeField] private GameObject attackBGPref;
    [SerializeField] private GameObject armorPref;
    private GameObject miniGameUI;
    private GameObject storage;
    private GameObject aim;
    private int[] sectors;
    private float attackTime;
    private int playerDamage;
    private int enemyHealth;
    private int nArmor;
    private int nFakeAim;
    private bool inverseMove;
    private float aimSpeed;
    private SoundManager soundManager;

    // ������������� �������� �����
    public void AttackInit(SoundManager _soundManager, float _attackTime, int _playerDamage, int _enemyHealth, 
        int _nArmor, int _nFakeAim, bool _inverseMove, GameObject _miniGameUI, float _aimSpeed)
    {
        // �������� ������� ���������� ��� ���������� ��������
        sectors = new int[36];
        for (int i = 0; i < 36; i++)
        {
            sectors[i] = 0;
        }

        soundManager = _soundManager;
        attackTime = _attackTime;
        playerDamage = _playerDamage;
        enemyHealth = _enemyHealth;
        nArmor = _nArmor;
        nFakeAim = _nFakeAim;
        inverseMove = _inverseMove;
        miniGameUI = _miniGameUI;
        aimSpeed = _aimSpeed;

        // ��������� ��� �������� �����
        storage = new GameObject("Storage");

        // �������� ����
        Instantiate(attackBGPref, new Vector3(0f, 0f, -10f), Quaternion.identity, storage.transform);

        // ��������� ���������� � ��������
        miniGameUI.SetActive(true);

        // �������� �������
        int _angle;

        // ��������� ���������� �������
        do
        {
            _angle = UnityEngine.Random.Range(0, 360);
        } while (!TakeSector(_angle));

        float _radian = _angle * Mathf.PI / 180f;
        float _aimX = 4f * Mathf.Cos(_radian);
        float _aimY = 4f * Mathf.Sin(_radian);
        aim = Instantiate(aimMainPref, new Vector3(_aimX, _aimY, -12f), Quaternion.identity, storage.transform);
        aim.GetComponent<AimMain>().AimMainInit(soundManager, aimSpeed);

        // �������� ����
        Instantiate(targetPref, new Vector3(0, 0, -11f), Quaternion.identity, storage.transform);

        // �������� ������ ����������
        GenArmor(nArmor);

        // �������� ���������� ��������
        GenFakeAim(nFakeAim);

        // ��������� ������������ ��������
        if (inverseMove)
        {
            aim.GetComponent<AimMain>().SetInverseMove();
        }
        
        StartCoroutine("AttackTimer");
        StartCoroutine("CheckShoot");
    }

    // ������ �����
    private IEnumerator AttackTimer()
    {
        TextMeshProUGUI _timeInfo = miniGameUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        while (!IsEnd())
        {
            yield return new WaitForFixedUpdate();
            attackTime -= Time.fixedDeltaTime;
            _timeInfo.text = Convert.ToString(Math.Round(attackTime, 2));
        }

        ClearScene();
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

    // �������� ��������� ��������
    public bool IsEnd()
    {
        // ���� ����������� ����� ��� �������� ���������� ����� �� ����
        if ( (attackTime <= 0) || (enemyHealth <= 0) )
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // �������� ��������
    private IEnumerator CheckShoot()
    {
        while (!IsEnd())
        {
            yield return new WaitForFixedUpdate();

            if (aim != null)
            {
                CountDamage(aim.GetComponent<AimMain>().Shoot());
            }
        }
    }

    // ������� ���������� �����
    private void CountDamage(int _points)
    {
        enemyHealth -= _points * playerDamage;
    }

    // ��������� �������� ����������
    public int GetEnemyHealth()
    {
        return enemyHealth;
    }

    // ��������� ������ ����������
    private void GenArmor(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            Instantiate(armorPref, new Vector3(0f, 0f, -12f), Quaternion.identity, storage.transform);
        }
    }

    // ��������� ���������� ��������
    private void GenFakeAim(int _num)
    {
        int _angle;
        float _radian, _x, _y;

        // ����������� ���������� ���������� �������� 
        if (_num > 11)
        {
            return;
        }

        for (int i = 0; i < _num; i++)
        {
            // ��������� ���������� �������
            do
            {
                _angle = UnityEngine.Random.Range(0, 360);
            } while (!TakeSector(_angle));
            
            _radian = _angle * Mathf.PI / 180f;
            _x = 4f * Mathf.Cos(_radian);
            _y = 4f * Mathf.Sin(_radian);
            GameObject _aimFake = Instantiate(aimFakePref, new Vector3(_x, _y, -12f), Quaternion.identity, storage.transform);
            _aimFake.GetComponent<AimFake>().AimFakeInit(soundManager, aimSpeed);
        }
    }

    // �������� ������ ��� ����������� �������������� ��������
    private bool TakeSector(int _angle)
    {
        int _curSector = (int)Mathf.Floor(_angle / 10f);
        int _prevSector = (35 - _curSector) % 36;
        int _nextSector = (37 - _curSector) % 36;

        // ���� ������ �� �����
        if ( (sectors[_prevSector] == 0) && (sectors[_curSector] == 0) && (sectors[_nextSector] == 0) )
        {
            sectors[_curSector] = 1;

            return true;
        }

        // ���� ������ �����
        else
        {
            return false;
        }
    }
}
