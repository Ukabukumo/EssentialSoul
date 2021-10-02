using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour
{
    private float lifetime;   // ����� ����� ����������

    // ��������� ������������� ����������
    public void SpellInit(float _lifetime)
    {
        lifetime = _lifetime;
        StartCoroutine("Act");
    }

    // �������� ����������
    private IEnumerator Act()
    {
        // ������ ����� ����������
        yield return new WaitForSeconds(lifetime);

        // ������������ ����������
        gameObject.SetActive(false);
    }
}
