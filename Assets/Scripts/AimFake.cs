using UnityEngine;

public class AimFake : Aim
{
    private float speed = 5f;
    private bool canShoot = true;

    private string upKey;
    private string downKey;
    private string leftKey;
    private string rightKey;

    private void Start()
    {
        // Случайная клавиша для движения вверх
        string[] _up = { "s", "a", "d" };
        upKey = _up[Random.Range(0, _up.Length)];

        // Случайная клавиша для движения вниз
        string[] _down = { "w", "a", "d" };
        downKey = _down[Random.Range(0, _down.Length)];

        // Случайная клавиша для движения влево
        string[] _left = { "w", "s", "d" };
        leftKey = _left[Random.Range(0, _left.Length)];

        // Случайная клавиша для движения вправо
        string[] _right = { "w", "s", "a" };
        rightKey = _right[Random.Range(0, _right.Length)];
    }

    private void FixedUpdate()
    {
        Movement();
        BorderCrossing();
        Shoot();
    }

    // Движение поддельного прицела
    protected override void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Debug.Log(moveHorizontal + " " + moveVertical);
        

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.fixedDeltaTime);

        /*float _x, _y;

        if (Input.GetKeyDown(upKey))
        {

        }*/
    }

    // Появление прицела в исходной области
    protected override void Respawn()
    {
        int _angle = Random.Range(0, 360);
        float _radian = _angle * Mathf.PI / 180f;
        float _x = 4f * Mathf.Cos(_radian);
        float _y = 4f * Mathf.Sin(_radian);

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

    // Действие при выстреле
    private void Shoot()
    {
        if (Input.GetAxis("Fire1") == 1 && canShoot)
        {
            Respawn();
            canShoot = false;
        }

        // Проверка отжатия клавиши, чтобы избежать лишних нажатий
        if (Input.GetAxis("Fire1") == 0)
        {
            canShoot = true;
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если соприкоснулся с защитой
        if (collision.tag == "Armor")
        {
            Respawn();
        }
    }
}
