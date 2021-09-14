using UnityEngine;
using System.Collections;
using System;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject aimPref;
    [SerializeField] private GameObject targetPref;
    [SerializeField] private GameObject attackBackgroundPref;
    [SerializeField] private GameObject armorPref;
    private GameObject storage;
    private GameObject aim;
    private float attackTime;
    private int playerDamage;
    private int enemyHealth;
    private int enemyArmor;

    public void AttackInit(float _attackTime, int _playerDamage, int _enemyHealth, int _enemyArmor)
    {
        attackTime = _attackTime;
        playerDamage = _playerDamage;
        enemyHealth = _enemyHealth;
        enemyArmor = _enemyArmor;

        // ��������� ��� �������� �����
        storage = new GameObject("Storage");

        // �������� ����
        Instantiate(attackBackgroundPref, new Vector3(0f, 0f, -1f), Quaternion.identity, storage.transform);

        // �������� �������
        int _angle = UnityEngine.Random.Range(0, 360);
        float _aimX = 4f * Mathf.Cos(_angle);
        float _aimY = 4f * Mathf.Sin(_angle);
        aim = Instantiate(aimPref, new Vector3(_aimX, _aimY, -1.2f), Quaternion.identity);

        // �������� ����
        Instantiate(targetPref, new Vector3(0, 0, -1.1f), Quaternion.identity, storage.transform);

        // �������� ������ ����������
        GenArmor(enemyArmor);
        
        StartCoroutine("AttackTimer");
        StartCoroutine("CheckShoot");
    }

    // ������ �����
    IEnumerator AttackTimer()
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

        if (aim != null)
        {
            Destroy(aim);
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
    IEnumerator CheckShoot()
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
}
