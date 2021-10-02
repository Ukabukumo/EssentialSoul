using UnityEngine;

public class Enemy
{
    // �������������� ����� �����
    public struct SwordAttack
    {
        public bool active;       // ������� ���� �����
        public int power;         // ����
        public float speed;       // ��������
        public float frequency;   // ������� ���������
    }

    // �������������� ����� ��������
    public struct ArrowAttack
    {
        public bool active;       // ������� ���� �����
        public int power;         // ����
        public float speed;       // ��������
        public float frequency;   // ������� ���������
    }

    // �������������� ����� ������������
    public struct SpellAttack
    {
        public bool active;       // ������� ���� �����
        public int power;         // ����
        public float lifetime;    // ����� �����
        public float frequency;   // ������� ���������
        public float distance;    // ���������� �� ������
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
        spellAttack.lifetime = 0f;
        spellAttack.frequency = 0f;
        spellAttack.distance = 0f;

        health = 0;
        nArmor = 0;
        nFalseAim = 0;
        inverseMove = false;
    }
}
