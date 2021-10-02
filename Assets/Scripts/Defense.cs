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

    // Инициализация миниигры защита
    public void DefenseInit(float _defenseTime, int _playerHealth, Enemy _enemy, GameObject _miniGameUI)
    {
        defenseTime = _defenseTime;
        playerHealth = _playerHealth;
        enemy = _enemy;
        miniGameUI = _miniGameUI;

        // Хранилище для объектов сцены
        storage = new GameObject("Storage");

        // Создание фона
        Instantiate(defenseBGPref, new Vector3(0f, 0f, -1f), Quaternion.identity, storage.transform);

        // Активация интерфейса в миниигре
        miniGameUI.SetActive(true);

        // Создание игрока в миниигре
        miniPlayer = Instantiate(miniPlayerPref, new Vector3(0f, 0f, -1.1f), Quaternion.identity, storage.transform);
        miniPlayer.GetComponent<MiniPlayer>().MiniPlayerInit(playerHealth, enemy);

        StartCoroutine("DefenseTimer");

        // Запуск генерации меча
        if (enemy.swordAttack.active)
        {
            StartCoroutine("SpawnSword", enemy.swordAttack.frequency);
        }

        // Запуск генерации стрел
        if (enemy.arrowAttack.active)
        {
            StartCoroutine("SpawnArrow", enemy.arrowAttack.frequency);
        }

        // Запуск генерации заклинаний
        if (enemy.spellAttack.active)
        {
            StartCoroutine("SpawnSpell", enemy.spellAttack.frequency);
        }
    }

    // Таймер защиты
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

    // Создание меча с заданной частотой
    private IEnumerator SpawnSword(float _frequency)
    {
        while (!IsEnd())
        {
            GenSword();
            yield return new WaitForSeconds(_frequency);
        }
    }

    // Создание стрел с заданной частотой
    private IEnumerator SpawnArrow(float _frequency)
    {
        while (!IsEnd())
        {
            GenArrow();
            yield return new WaitForSeconds(_frequency);
        }
    }

    // Создание заклинаний с заданной частотой
    private IEnumerator SpawnSpell(float _frequency)
    {
        while (!IsEnd())
        {
            GenSpell();
            yield return new WaitForSeconds(_frequency);
        }
    }

    // Предупреждение появления атакующего элемента
    private IEnumerator WarningAttack(float _time, int _type, Vector3 _position, Quaternion _rotation)
    {
        // Создание предупреждающего знака
        GameObject _warning = Instantiate(warningPref, new Vector3(_position.x, _position.y, -2f), Quaternion.identity, storage.transform);
        
        // Время предупреждения
        yield return new WaitForSeconds(_time);

        // Уничтожение предупреждающего знака
        if (_warning != null)
        {
            Destroy(_warning);
        }
        
        // Создание меча
        if (_type == 1)
        {
            // Чтобы избежать обращение к уже уничтоженным объектам после окончания миниигры
            if (!IsEnd())
            {
                GameObject _sword = Instantiate(swordPref, _position, _rotation, storage.transform);
                _sword.GetComponent<Sword>().SwordInit(enemy.swordAttack.speed, miniPlayer.transform);
            }
        }

        // Создание стрелы
        else if (_type == 2)
        {
            // Чтобы избежать обращение к уже уничтоженным объектам после окончания миниигры
            if (!IsEnd())
            {
                GameObject _arrow = Instantiate(arrowPref, _position, _rotation, storage.transform);
                _arrow.GetComponent<Arrow>().ArrowInit(enemy.arrowAttack.speed);
            }
        }

        // Создание заклинания
        else if (_type == 3)
        {
            // Чтобы избежать обращение к уже уничтоженным объектам после окончания миниигры
            if (!IsEnd())
            {
                GameObject _spell = Instantiate(spellPref, _position, Quaternion.identity, storage.transform);
                _spell.GetComponent<Spell>().SpellInit(enemy.spellAttack.lifetime);
            }
        }
        
    }

    // Проверка окончания миниигры
    public bool IsEnd()
    {
        // Если закончилось время или здоровье игрока упало до нуля
        if ( (defenseTime <= 0) || (playerHealth <= 0) )
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // Получение изменённого здоровья игрока
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    // Генерация меча
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

        // Появление предупреждающего знака в заданной позиции
        StartCoroutine(WarningAttack(1f, 1, _position, _rotation));
    }

    // Генерация стрелы
    private void GenArrow()
    {
        Vector3 _position;
        Quaternion _rotation;
        Vector3 _playerPos = miniPlayer.transform.position;
        float _borderPos = 4f;
        int _side = UnityEngine.Random.Range(0, 4);
        int _offset = UnityEngine.Random.Range(-1, 2);

        // Определение стартовой позиции
        switch (_side)
        {
            // Сверху
            case 0:
                _position = new Vector3(_playerPos.x + _offset, _borderPos, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 270f);
                break;

            // Снизу
            case 1:
                _position = new Vector3(_playerPos.x + _offset, -_borderPos, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 90f);
                break;

            // Слева
            case 2:
                _position = new Vector3(-_borderPos, _playerPos.y + _offset, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 0f);
                break;

            // Справа
            case 3:
                _position = new Vector3(_borderPos, _playerPos.y + _offset, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 180f);
                break;

            // По-умолчанию (Слева)
            default:
                _position = new Vector3(-_borderPos, _playerPos.y + _offset, _playerPos.z);
                _rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
        }

        // Появление предупреждающего знака в заданной позиции
        StartCoroutine(WarningAttack(1f, 2, _position, _rotation));
    }

    // Генерация заклинания
    private void GenSpell()
    {
        Vector3 _playerPos = miniPlayer.transform.position;        // Позиция игрока
        float _diff = enemy.spellAttack.distance;                  // Расстояние спавна от игрока
        float _border = 4f;                                        // Границы игрового поля
        float _upBorder, _downBorder, _leftBorder, _rightBorder;   // Границы поля для спавна

        // Сравнение расстояния до ВЕРХНЕЙ границы
        if (_border - _playerPos.y >= _diff)
        {
            _upBorder = _diff;
        }

        else
        {
            _upBorder = _border - _playerPos.y;
        }

        // Сравнение расстояния до НИЖНЕЙ границы
        if (-_border - _playerPos.y <= -_diff)
        {
            _downBorder = -_diff;
        }

        else
        {
            _downBorder = -_border - _playerPos.y; 
        }

        // Сравнение расстояния до ЛЕВОЙ границы
        if (-_border - _playerPos.x <= -_diff)
        {
            _leftBorder = -_diff;
        }

        else
        {
            _leftBorder = -_border - _playerPos.x;
        }

        // Сравнение расстояния до ПРАВОЙ границы
        if (_border -_playerPos.x >= _diff)
        {
            _rightBorder = _diff;
        }

        else
        {
            _rightBorder = _border - _playerPos.x;
        }

        // Генерация случайной позиции в заданном поле вокруг игрока
        float _x = _playerPos.x + UnityEngine.Random.Range(_leftBorder, _rightBorder);
        float _y = _playerPos.y + UnityEngine.Random.Range(_downBorder, _upBorder);
        Vector3 _position = new Vector3(_x, _y, _playerPos.z);

        // Появление предупреждающего знака в заданной позиции
        StartCoroutine(WarningAttack(1f, 3, _position, Quaternion.identity));
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
}
