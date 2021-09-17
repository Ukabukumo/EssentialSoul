using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    private float speed;   // �������� �������� ������

    // ��������� ������������� ������
    public void ArrowInit(float _speed)
    {
        speed = _speed;

        StartCoroutine("Act");
    }

    // �������� ������
    private void Movement()
    {
        Vector2 _movement = new Vector2(speed, 0f);
        transform.Translate(_movement * Time.fixedDeltaTime);
    }

    private IEnumerator Act()
    {
        while (!IsBorder())
        {
            yield return new WaitForFixedUpdate();

            Movement();
        }

        // ������������ ������
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }

    // �������� ����������� �������
    private bool IsBorder()
    {
        // ������� �������� ����
        if ( (Mathf.Abs(transform.position.x) > 5) || (Mathf.Abs(transform.position.y) > 5) )
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
