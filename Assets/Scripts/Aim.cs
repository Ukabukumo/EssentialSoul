using UnityEngine;

public abstract class Aim : MonoBehaviour
{
    // Движение прицела
    protected abstract void Movement();

    // Появление прицела в исходной области
    protected abstract void Respawn();

    // Проверка прохода через границу
    protected abstract void BorderCrossing();
}
