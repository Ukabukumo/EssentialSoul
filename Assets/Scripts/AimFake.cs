using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFake : Aim
{
    private float speed = 5f;

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
    }

    // Движение поддельного прицела
    protected override void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);
    }

    // Появление прицела в исходной области
    protected override void Respawn()
    {
        int _angle = Random.Range(0, 360);
        float _x = 4f * Mathf.Cos(_angle);
        float _y = 4f * Mathf.Sin(_angle);

        transform.position = new Vector3(_x, _y, transform.position.z);
    }

    // Проверка прохода через границу
    protected override void BorderCrossing()
    {
        if ((Mathf.Abs(transform.position.y) > 5) || (Mathf.Abs(transform.position.x) > 10))
        {
            Respawn();
        }
    }
}
