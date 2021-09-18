using UnityEngine;
using System.Collections;
using System;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject aimMainPref;
    [SerializeField] private GameObject aimFakePref;
    [SerializeField] private GameObject targetPref;
    [SerializeField] private GameObject attackBGPref;
    [SerializeField] private GameObject armorPref;
    private GameObject storage;
    private GameObject aim;
    private int[] sectors;
    private float attackTime;
    private int playerDamage;
    private int enemyHealth;
    private int nArmor;
    private int nFakeAim;
    private bool inverseMove;

    // ������������� �������� �����
    public void AttackInit(float _attackTime, int _playerDamage, int _enemyHealth, 
        int _nArmor, int _nFakeAim, bool _inverseMove)
    {
        // �������� ������� ���������� ��� ���������� ��������
        sectors = new int[36];
        for (int i = 0; i < 36; i++)
        {
            sectors[i] = 0;
        }

        attackTime = _attackTime;
        playerDamage = _playerDamage;
        enemyHealth = _enemyHealth;
        nArmor = _nArmor;
        nFakeAim = _nFakeAim;
        inverseMove = _inverseMove;

        // ��������� ��� �������� �����
        storage = new GameObject("Storage");

        // �������� ����
        Instantiate(attackBGPref, new Vector3(0f, 0f, -1f), Quaternion.identity, storage.transform);

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
        aim = Instantiate(aimMainPref, new Vector3(_aimX, _aimY, -1.2f), Quaternion.identity, storage.transform);

        // �������� ����
        Instantiate(targetPref, new Vector3(0, 0, -1.1f), Quaternion.identity, storage.transform);

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
        while (!IsEnd())
        {
            yield return new WaitForFixedUpdate();
            attackTime -= Time.fixedDeltaTime;
            //Debug.Log(attackTime);
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
            Instantiate(armorPref, new Vector3(0f, 0f, -1.2f), Quaternion.identity, storage.transform);
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
            Instantiate(aimFakePref, new Vector3(_x, _y, -1.2f), Quaternion.identity, storage.transform);
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
