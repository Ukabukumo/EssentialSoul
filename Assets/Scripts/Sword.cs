using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    private float speed;   // �������� �������� ����
    private Vector3 startPosition;

    // ��������� ������������� ����
    public void SwordInit(float _speed)
    {
        speed = _speed;
        startPosition = transform.position;

        Vector2 movement = new Vector2(0f, transform.localScale.y);
        transform.Translate(movement);

        StartCoroutine("Act");
    }

    // �������� ����
    private IEnumerator Act()
    {
        float _angle = 0;

        // ���� �� �������� �������
        while (_angle < 180)
        {
            yield return new WaitForFixedUpdate();
            _angle += speed;

            Movement();
        }

        // ������������ ����
        gameObject.SetActive(false);
    }

    // �������� ����
    private void Movement()
    {
        transform.position = startPosition;
        transform.rotation *= Quaternion.Euler(0f, 0f, speed);

        // ����������� ���� �����, ����� �� �������� ����� ��������� ����������,
        // � �� �������� �� �����
        Vector2 movement = new Vector2(0f, transform.localScale.y);
        transform.Translate(movement);
    }
}
