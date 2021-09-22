using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject forestLocation;
    private GameObject currentLocation;

    /*public void Awake()
    {
        CreateLevel();
    }*/

    // Создание локации
    public void CreateLevel()
    {
        currentLocation = Instantiate(forestLocation, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }
}
