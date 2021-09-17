using UnityEngine;

public class MiniPlayer : MonoBehaviour
{
    private float speed = 5f;   // Скорость игрока в миниигре

    private void FixedUpdate()
    {
        Movement();
    }

    // Передвижение игрока в миниигре
    private void Movement()
    {
        // Позиция перед началом движения
        Vector3 _prevPosition = transform.position;

        float _moveHorizontal = Input.GetAxisRaw("Horizontal");
        float _moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 _movement = new Vector2(_moveHorizontal, _moveVertical);
        transform.Translate(_movement * speed * Time.fixedDeltaTime);

        // Если пересёк границу по горизонтали
        if (IsHorizontalBorder())
        {
            transform.position = new Vector3(_prevPosition.x, transform.position.y, transform.position.z);
        }

        // Если пересёк границу по вертикали
        if (IsVerticalBorder())
        {
            transform.position = new Vector3(transform.position.x, _prevPosition.y, transform.position.z);
        }
    }

    // Проверка пересечения границы по горизонтали
    private bool IsHorizontalBorder()
    {
        // Левая или правая граница игрового поля
        if (Mathf.Abs(transform.position.x) > 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // Проверка пересечения границы по вертикали
    private bool IsVerticalBorder()
    {
        // Верхняя или нижняя граница игрового поля
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
