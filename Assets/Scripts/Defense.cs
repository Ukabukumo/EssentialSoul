using UnityEngine;

public class Defense : MonoBehaviour
{
    [SerializeField] private GameObject defenseBGPref;
    private GameObject storage;

    // Инициализация миниигры защита
    public void DefenceInit()
    {
        // Хранилище для объектов сцены
        storage = new GameObject("Storage");

        // Создание фона
        Instantiate(defenseBGPref, new Vector3(0f, 0f, -1f), Quaternion.identity, storage.transform);
    }
}
