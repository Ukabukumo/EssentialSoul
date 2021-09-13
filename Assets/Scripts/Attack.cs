using UnityEngine;
using System.Collections;
using System;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject aimPref;
    [SerializeField] private GameObject targetPref;
    [SerializeField] private GameObject attackBackgroundPref;
    private GameObject aim;
    private GameObject target;
    private GameObject attackBackground;
    private float attackTime;
    private int factor;

    public void AttackInit(float _attackTime)
    {
        // Создание фона
        attackBackground = Instantiate(attackBackgroundPref, new Vector3(0f, 0f, -1f), Quaternion.identity);

        // Создание прицела
        int _angle = UnityEngine.Random.Range(0, 360);
        float _aimX = 4f * Mathf.Cos(_angle);
        float _aimY = 4f * Mathf.Sin(_angle);
        aim = Instantiate(aimPref, new Vector3(_aimX, _aimY, -1.2f), Quaternion.identity);

        // Создание цели
        target = Instantiate(targetPref, new Vector3(0, 0, -1.1f), Quaternion.identity);
        
        attackTime = _attackTime;
        StartCoroutine("AttackTimer");
    }

    // Таймер атаки
    IEnumerator AttackTimer()
    {
        while (attackTime > 0)
        {
            yield return new WaitForSeconds(0.01f);
            attackTime -= 0.01f;
            Debug.Log(Math.Round(attackTime, 2));
        }

        // Получение заработанных очков
        factor = aim.GetComponent<Aim>().GetPoints();

        ClearScene();
    }

    // Очистка сцены
    private void ClearScene()
    {
        if (aim != null)
        {
            Destroy(aim);
        }

        if (target != null)
        {
            Destroy(target);
        }
        
        if (attackBackground != null)
        {
            Destroy(attackBackground);
        }
    }

    // Проверка окончания миниигры
    public bool IsEnd()
    {
        if (attackTime > 0)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    // Получение коэффициента урона
    public int GetFactor()
    {
        return factor;
    }
}
