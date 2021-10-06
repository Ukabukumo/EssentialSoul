using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject forestLocation;
    [SerializeField] private GameObject tree;
    private GameObject currentLocation;

    // �������� �������
    public void CreateLevel()
    {
        currentLocation = Instantiate(forestLocation, new Vector3(0f, 0f, 0f), Quaternion.identity);
        GenTrees(10);
    }

    // ��������� ��������
    private void GenTrees(int _n)
    {
        // ������ ��������� ���������
        int[,] _places = new int[41, 41];

        // �������� ������ ��������� ���������
        for (int i = 0; i < 41; i++)
        {
            for (int j = 0; j < 41; j++)
            {
                _places[i, j] = 0;
            }
        }

        // ������ ��������� ���������
        int[,] _available = new int[1681, 2];

        for (int i = 1; i <= _n; i++)
        {
            // ���������� ��������� ����
            int _nSpot = 0;

            // ���� ��� ��������� �����
            for (int y = 0; y < 41; y++)
            {
                for (int x = 0; x < 41; x++)
                {
                    if (_places[y, x] == 0)
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
            int _randPos = Random.Range(0, _nSpot);
            int _x = _available[_randPos, 0] - 20;
            int _y = _available[_randPos, 1] - 20;

            // ���������� ����� ������ ������
            for (int y = _available[_randPos, 1] - 6; y <= _available[_randPos, 1] + 6; y++)
            {
                for (int x = _available[_randPos, 0] - 6; x <= _available[_randPos, 0] + 6; x++)
                {
                    // �������� ���������� � �������� �������
                    if (y >= 0 && y <= 40 && x >= 0 && x <= 40)
                    {
                        _places[y, x] = 1;
                    }
                }
            }

            // ���������� ����� ������
            _places[_available[_randPos, 1], _available[_randPos, 0]] = 2;

            // ������� ������
            Vector3 _position = new Vector3(_x, _y, -1.0f);

            // ������ ������ � ���������� �������
            Instantiate(tree, _position, Quaternion.identity);
        }
    }
}
