using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour
{
    private float lifetime;   // Время жизни заклинания

    // Установка характеристик заклинания
    public void SpellInit(float _lifetime)
    {
        lifetime = _lifetime;
        StartCoroutine("Act");
    }

    // Действия заклинания
    private IEnumerator Act()
    {
        // Таймер жизни заклинания
        yield return new WaitForSeconds(lifetime);

        // Исчезновение заклинания
        gameObject.SetActive(false);
    }
}
