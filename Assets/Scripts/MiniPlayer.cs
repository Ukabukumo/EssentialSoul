using UnityEngine;

public class MiniPlayer : MonoBehaviour
{
    private float speed = 5f;   // �������� ������ � ��������

    private void FixedUpdate()
    {
        Movement();
    }

    // ������������ ������ � ��������
    private void Movement()
    {
        // ������� ����� ������� ��������
        Vector3 _prevPosition = transform.position;

        float _moveHorizontal = Input.GetAxisRaw("Horizontal");
        float _moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 _movement = new Vector2(_moveHorizontal, _moveVertical);
        transform.Translate(_movement * speed * Time.fixedDeltaTime);

        // ���� ������ ������� �� �����������
        if (IsHorizontalBorder())
        {
            transform.position = new Vector3(_prevPosition.x, transform.position.y, transform.position.z);
        }

        // ���� ������ ������� �� ���������
        if (IsVerticalBorder())
        {
            transform.position = new Vector3(transform.position.x, _prevPosition.y, transform.position.z);
        }
    }

    // �������� ����������� ������� �� �����������
    private bool IsHorizontalBorder()
    {
        // ����� ��� ������ ������� �������� ����
        if (Mathf.Abs(transform.position.x) > 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // �������� ����������� ������� �� ���������
    private bool IsVerticalBorder()
    {
        // ������� ��� ������ ������� �������� ����
        if (Mathf.Abs(transform.position.y) > 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
