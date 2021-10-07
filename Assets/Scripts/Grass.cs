using UnityEngine;

public class Grass : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("Touch");
        }
    }
}
