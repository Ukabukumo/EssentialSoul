using UnityEngine;

public class Armor : MonoBehaviour
{
    private float angle = 0f;
    private float radian = 0f;
    private float speed = 1f;
    private float radius = 2.5f;
    private float direction = 1f;

    private void Start()
    {
        Spawn();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    // �������� ������
    private void Movement()
    {
        angle = angle + direction * speed;
        radian = angle * Mathf.PI / 180f;

        float _x = radius * Mathf.Cos(radian);
        float _y = radius * Mathf.Sin(radian);

        Vector2 movement = new Vector2(_x, _y);
        transform.position = new Vector3(_x, _y, transform.position.z);
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    // ��������� ������
    private void Spawn()
    {
        // ��������� ����
        angle = Random.Range(0, 360);

        // ��������� ���� � ��������
        radian = angle * Mathf.PI / 180f;

        // ����������� ������� ����������, �� ������� �������� ������
        radius = (float)Random.Range(0, 2) + 1.5f;

        // ����������� ����������� �������� ������
        switch (Random.Range(0, 2))
        {
            // ������ ������� �������
            case 0:
                direction = 1f;
                break;

            // �� ������� �������
            case 1:
                direction = -1f;
                break;

            // ������ ������� ������� (��-���������)
            default:
                direction = 1f;
                break;
        }

        // ����������� �������� �������� ������
        speed = Random.Range(1, 4);

        float _x = radius * Mathf.Cos(radian);
        float _y = radius * Mathf.Sin(radian);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }
}
