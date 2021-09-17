using UnityEngine;

public class Enemy
{
    // Характеристики атаки мечом
    public struct SwordAttack
    {
        public bool active;     // Наличие вида атаки
        public int power;       // Сила
        public float speed;       // Скорость
        public float frequency;   // Частота появления
    }

    // Характеристики атаки стрелами
    public struct ArrowAttack
    {
        public bool active;     // Наличие вида атаки
        public int power;       // Сила
        public float speed;       // Скорость
        public float frequency;   // Частота появления
    }

    // Характеристики атаки заклинаниями
    public struct SpellAttack
    {
        public bool active;     // Наличие вида атаки
        public int power;       // Сила
        public float speed;       // Скорость
        public float frequency;   // Частота появления
    }

    public SwordAttack swordAttack = new SwordAttack();
    public ArrowAttack arrowAttack = new ArrowAttack();
    public SpellAttack spellAttack = new SpellAttack();

    public int health;
    public int nArmor;
    public int nFalseAim;
    public bool inverseMove;

    public Enemy()
    {
        swordAttack.active = false;
        swordAttack.power = 0;
        swordAttack.speed = 0f;
        swordAttack.frequency = 0f;

        arrowAttack.active = false;
        arrowAttack.power = 0;
        arrowAttack.speed = 0f;
        arrowAttack.frequency = 0f;

        spellAttack.active = false;
        spellAttack.power = 0;
        spellAttack.speed = 0f;
        spellAttack.frequency = 0f;

        health = 0;
        nArmor = 0;
        nFalseAim = 0;
        inverseMove = false;
    }
}
