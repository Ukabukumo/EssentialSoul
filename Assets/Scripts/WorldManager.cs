using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject forestLocationPref;
    [SerializeField] private GameObject treePref;
    [SerializeField] private GameObject grassPref;
    private GameObject currentLocation;
    private GameObject player;
    private const int widthF = 41;        // ������ ����
    private const int heightF = 41;       // ������ ����
    private const int halfWidthF = 20;    // �������� ������ ����
    private const int halfHeightF = 20;   // �������� ������ ����
    private int[,] places;                // ������ ���� ��������

    // �������� �������
    public void CreateLevel(GameObject _player)
    {
        player = _player;

        currentLocation = Instantiate(forestLocationPref, new Vector3(0f, 0f, 0f), Quaternion.identity);

        ClearPlaces();
        PlayerSpot();
        GenTrees(20);
        GenGrass(400);
    }

    // ������� ��� ����� � �������
    private void ClearPlaces()
    {
        places = new int[heightF, widthF];

        // �������� ������ ���� ��������
        for (int i = 0; i < heightF; i++)
        {
            for (int j = 0; j < widthF; j++)
            {
                places[i, j] = 0;
            }
        }
    }

    // �������� ����� ��� ������
    private void PlayerSpot()
    {
        // ������� ������
        Vector2 _playerPos = player.transform.position;
        int _x = (int)Math.Round(_playerPos.x, 0) + halfWidthF;
        int _y = (int)Math.Round(_playerPos.y, 0) + halfHeightF;

        // ������� ���������� ������ ������
        int _radius = 2;

        // ���������� ����� ������ ������
        for (int y = _y - _radius; y <= _y + _radius; y++)
        {
            for (int x = _x - _radius; x <= _x + _radius; x++)
            {
                // �������� ���������� � �������� �������
                if (y >= 0 && y <= 40 && x >= 0 && x <= 40)
                {
                    places[y, x] = 1;
                }
            }
        }

        // ���������� ����� ������
        places[_y, _x] = 2;
    }

    // ��������� ��������
    private void GenTrees(int _n)
    {
        // ������ ��������� ���������
        int[,] _available = new int[heightF * widthF, 2];

        // ������� ���������� ������ ������
        int _radius = 6;

        for (int i = 1; i <= _n; i++)
        {
            // ���������� ��������� ����
            int _nSpot = 0;

            // ���� ��� ��������� �����
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

            // ���� ����������� ��������� �����
            if (_nSpot == 0)
            {
                return;
            }

            // �������� ��������� ��������� �����
            int _randPos = UnityEngine.Random.Range(0, _nSpot);
            float _x = _available[_randPos, 0] - halfWidthF;
            float _y = _available[_randPos, 1] - halfHeightF;

            // ������� ����
            float _z = -1f + (_y - halfHeightF - 2f) / 100f - 0.01f;

            // ���������� ����� ������ ������
            for (int y = _available[_randPos, 1] - _radius; y <= _available[_randPos, 1] + _radius; y++)
            {
                for (int x = _available[_randPos, 0] - _radius; x <= _available[_randPos, 0] + _radius; x++)
                {
                    // �������� ���������� � �������� �������
                    if (y >= 0 && y < heightF && x >= 0 && x < widthF)
                    {
                        places[y, x] = 1;
                    }
                }
            }

            // ���������� ����� ������
            places[_available[_randPos, 1], _available[_randPos, 0]] = 2;

            // ������� ������
            Vector3 _position = new Vector3(_x, _y, _z);

            // ������ ������ � ���������� �������
            Instantiate(treePref, _position, Quaternion.identity);
        }
    }

    // ��������� �����
    private void GenGrass(int _n)
    {
        // ������ ��������� ���������
        int[,] _available = new int[heightF * widthF, 2];

        // ���������� ��������� ����
        int _nSpot = 0;

        // ���� ��� ��������� �����
        for (int y = 0; y < heightF; y++)
        {
            for (int x = 0; x < widthF; x++)
            {
                if (places[y, x] != 2)
                {
                    _available[_nSpot, 0] = x;
                    _available[_nSpot, 1] = y;
                    _nSpot++;
                }
            }
        }

        for (int i = 1; i <= _n; i++)
        {
            // �������� ��������� ��������� �����
            int _randPos = UnityEngine.Random.Range(0, _nSpot);
            float _x = _available[_randPos, 0] - halfWidthF;
            float _y = _available[_randPos, 1] - halfHeightF;

            // ������� ����
            float _z = -1f + (_y - halfHeightF) / 100f + 0.006f;

            // ������� �����
            Vector3 _position = new Vector3(_x, _y, _z);

            // ������ ������ � ���������� �������
            Instantiate(grassPref, _position, Quaternion.identity);
        }
    }
}
