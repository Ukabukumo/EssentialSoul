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

    // Инициализация миниигры атака
    public void AttackInit(SoundManager _soundManager, float _attackTime, int _playerDamage, int _enemyHealth, 
        int _nArmor, int _nFakeAim, bool _inverseMove, GameObject _miniGameUI, float _aimSpeed)
    {
        // Условные сектора окружности для размещения прицелов
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

        // Хранилище для объектов сцены
        storage = new GameObject("Storage");

        // Создание фона
        Instantiate(attackBGPref, new Vector3(0f, 0f, -10f), Quaternion.identity, storage.transform);

        // Активация интерфейса в миниигре
        miniGameUI.SetActive(true);

        // Создание прицела
        int _angle;

        // Генерация уникальной позиции
        do
        {
            _angle = UnityEngine.Random.Range(0, 360);
        } while (!TakeSector(_angle));

        float _radian = _angle * Mathf.PI / 180f;
        float _aimX = 4f * Mathf.Cos(_radian);
        float _aimY = 4f * Mathf.Sin(_radian);
        aim = Instantiate(aimMainPref, new Vector3(_aimX, _aimY, -12f), Quaternion.identity, storage.transform);
        aim.GetComponent<AimMain>().AimMainInit(soundManager, aimSpeed);

        // Создание цели
        Instantiate(targetPref, new Vector3(0, 0, -11f), Quaternion.identity, storage.transform);

        // Создание защиты противника
        GenArmor(nArmor);

        // Создание поддельных прицелов
        GenFakeAim(nFakeAim);

        // Установка инверсивного движения
        if (inverseMove)
        {
            aim.GetComponent<AimMain>().SetInverseMove();
        }
        
        StartCoroutine("AttackTimer");
        StartCoroutine("CheckShoot");
    }

    // Таймер атаки
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

    // Очистка сцены
    private void ClearScene()
    {
        if (storage != null)
        {
            Destroy(storage);
        }

        // Деактивация интерфейса в миниигре
        miniGameUI.SetActive(false);
    }

    // Проверка окончания миниигры
    public bool IsEnd()
    {
        // Если закончилось время или здоровье противника упало до нуля
        if ( (attackTime <= 0) || (enemyHealth <= 0) )
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // Проверка выстрела
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

    // Подсчёт нанесённого урона
    private void CountDamage(int _points)
    {
        enemyHealth -= _points * playerDamage;
    }

    // Получение здоровья противника
    public int GetEnemyHealth()
    {
        return enemyHealth;
    }

    // Генерация защиты противника
    private void GenArmor(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            Instantiate(armorPref, new Vector3(0f, 0f, -12f), Quaternion.identity, storage.transform);
        }
    }

    // Генерация поддельных прицелов
    private void GenFakeAim(int _num)
    {
        int _angle;
        float _radian, _x, _y;

        // Ограничение количества поддельных прицелов 
        if (_num > 11)
        {
            return;
        }

        for (int i = 0; i < _num; i++)
        {
            // Генерация уникальной позиции
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

    // Занимаем сектор для уникального местоположения прицелов
    private bool TakeSector(int _angle)
    {
        int _curSector = (int)Mathf.Floor(_angle / 10f);
        int _prevSector = (35 - _curSector) % 36;
        int _nextSector = (37 - _curSector) % 36;

        // Если сектор не занят
        if ( (sectors[_prevSector] == 0) && (sectors[_curSector] == 0) && (sectors[_nextSector] == 0) )
        {
            sectors[_curSector] = 1;

            return true;
        }

        // Если сектор занят
        else
        {
            return false;
        }
    }
}
