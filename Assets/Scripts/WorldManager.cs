using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject forestLocationPref;
    [SerializeField] private GameObject treePref;
    [SerializeField] private GameObject grassPref;
    [SerializeField] private GameObject redFlowerPref;
    [SerializeField] private GameObject blueFlowerPref;
    private GameObject currentLocation;
    private GameObject environmentStorage;
    private GameObject player;
    private const int widthF = 37;        // Ширина поля
    private const int heightF = 37;       // Высота поля
    private const int halfWidthF = 18;    // Половина ширины поля
    private const int halfHeightF = 18;   // Половина высоты поля
    private int[,] places;                // Массив мест объектов

    // Создание локации
    public void CreateLevel(GameObject _player)
    {
        player = _player;

        currentLocation = Instantiate(forestLocationPref, new Vector3(0f, 0f, 0f), Quaternion.identity);

        // Хранилище для объектов окружения локации
        environmentStorage = new GameObject("Environment");

        ClearPlaces();
        PlayerSpot();

        // Генерация окружения
        GenTrees(20);
        GenGrass(400);
        GenFlowers(10);
    }

    // Очищаем все места в массиве
    private void ClearPlaces()
    {
        places = new int[heightF, widthF];

        // Обнуляем массив мест объектов
        for (int i = 0; i < heightF; i++)
        {
            for (int j = 0; j < widthF; j++)
            {
                places[i, j] = 0;
            }
        }
    }

    // Занимаем место для игрока
    private void PlayerSpot()
    {
        // Позиция игрока
        Vector2 _playerPos = player.transform.position;
        int _x = (int)Math.Round(_playerPos.x, 0) + halfWidthF;
        int _y = (int)Math.Round(_playerPos.y, 0) + halfHeightF;

        // Занятое расстояние вокруг игрока
        int _radius = 2;

        // Обозначаем место вокруг игрока
        for (int y = _y - _radius; y <= _y + _radius; y++)
        {
            for (int x = _x - _radius; x <= _x + _radius; x++)
            {
                // Проверка нахождения в границах массива
                if (y >= 0 && y <= 40 && x >= 0 && x <= 40)
                {
                    places[y, x] = 1;
                }
            }
        }

        // Обозначаем место игрока
        places[_y, _x] = 2;
    }

    // Генерация деревьев
    private void GenTrees(int _n)
    {
        // Массив доступных координат
        int[,] _available = new int[heightF * widthF, 2];

        // Занятое расстояние вокруг дерева
        int _radius1 = 6;
        int _radius2 = 3;

        for (int i = 1; i <= _n; i++)
        {
            // Количество доступных мест
            int _nSpot = 0;

            // Ищем все свободные места
            for (int y = 0; y < heightF; y++)
            {
                for (int x = 0; x < widthF; x++)
                {
                    if (places[y, x] == 0)
                    {
                        _available[_nSpot, 0] = x;
                        _available[_nSpot, 1] = y;
                        _nSpot++;
                    }
                }
            }

            // Если закончились свободные места
            if (_nSpot == 0)
            {
                return;
            }

            // Выбираем случайное свободное место
            int _randPos = UnityEngine.Random.Range(0, _nSpot);
            float _x = _available[_randPos, 0] - halfWidthF;
            float _y = _available[_randPos, 1] - halfHeightF;

            // Уровень фона
            float _z = -1f + (_y - halfHeightF - 2f) / 100f - 0.01f;

            // Обозначаем места, недоступные для деревьев 
            for (int y = _available[_randPos, 1] - _radius1; y <= _available[_randPos, 1] + _radius1; y++)
            {
                for (int x = _available[_randPos, 0] - _radius1; x <= _available[_randPos, 0] + _radius1; x++)
                {
                    // Проверка нахождения в границах массива
                    if (y >= 0 && y < heightF && x >= 0 && x < widthF)
                    {
                        places[y, x] = 1;
                    }
                }
            }

            // Обозначаем места, недоступные для прочих объектов
            for (int y = _available[_randPos, 1] - _radius2; y <= _available[_randPos, 1] + _radius2; y++)
            {
                for (int x = _available[_randPos, 0] - _radius2; x <= _available[_randPos, 0] + _radius2; x++)
                {
                    // Проверка нахождения в границах массива
                    if (y >= 0 && y < heightF && x >= 0 && x < widthF)
                    {
                        places[y, x] = 2;
                    }
                }
            }

            // Обозначаем место дерева
            places[_available[_randPos, 1], _available[_randPos, 0]] = 3;

            // Позиция дерева
            Vector3 _position = new Vector3(_x, _y, _z);

            // Создаём дерево в полученной позиции
            Instantiate(treePref, _position, Quaternion.identity, environmentStorage.transform);
        }
    }

    // Генерация травы
    private void GenGrass(int _n)
    {
        // Массив доступных координат
        int[,] _available = new int[heightF * widthF, 2];

        // Количество доступных мест
        int _nSpot = 0;

        // Ищем все свободные места
        for (int y = 0; y < heightF; y++)
        {
            for (int x = 0; x < widthF; x++)
            {
                if ( (places[y, x] != 2) && (places[y, x] != 3) )
                {
                    _available[_nSpot, 0] = x;
                    _available[_nSpot, 1] = y;
                    _nSpot++;
                }
            }
        }

        for (int i = 1; i <= _n; i++)
        {
            // Выбираем случайное свободное место
            int _randPos = UnityEngine.Random.Range(0, _nSpot);
            float _x = _available[_randPos, 0] - halfWidthF;
            float _y = _available[_randPos, 1] - halfHeightF;

            // Уровень фона
            float _z = -1f + (_y - halfHeightF) / 100f - 0.014f;

            // Позиция травы
            Vector3 _position = new Vector3(_x, _y, _z);

            // Создаём траву в полученной позиции
            Instantiate(grassPref, _position, Quaternion.identity, environmentStorage.transform);
        }
    }

    // Генерация цветов
    private void GenFlowers(int _n)
    {
        // Массив доступных координат
        int[,] _available = new int[heightF * widthF, 2];

        // Количество доступных мест
        int _nSpot = 0;

        // Ищем все свободные места
        for (int y = 0; y < heightF; y++)
        {
            for (int x = 0; x < widthF; x++)
            {
                if ( (places[y, x] != 2) && (places[y, x] != 3) )
                {
                    _available[_nSpot, 0] = x;
                    _available[_nSpot, 1] = y;
                    _nSpot++;
                }
            }
        }

        for (int i = 1; i <= _n; i++)
        {
            // Выбираем случайное свободное место
            int _randPos = UnityEngine.Random.Range(0, _nSpot);
            float _x = _available[_randPos, 0] - halfWidthF;
            float _y = _available[_randPos, 1] - halfHeightF;

            // Уровень фона
            float _z = -1f + (_y - halfHeightF) / 100f - 0.014f;

            // Позиция травы
            Vector3 _position = new Vector3(_x, _y, _z);

            // Создаём случайный цветок в полученной позиции
            int _rand = UnityEngine.Random.Range(0, 2);

            if (_rand == 0)
            {
                Instantiate(redFlowerPref, _position, Quaternion.identity, environmentStorage.transform);
            }

            else
            {
                Instantiate(blueFlowerPref, _position, Quaternion.identity, environmentStorage.transform);
            }
        }
    }    
}
