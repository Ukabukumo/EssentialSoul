using UnityEngine;

public abstract class Aim : MonoBehaviour
{
    // �������� �������
    protected abstract void Movement();

    // ��������� ������� � �������� �������
    protected abstract void Respawn();

    // �������� ������� ����� �������
    protected abstract void BorderCrossing();
}
