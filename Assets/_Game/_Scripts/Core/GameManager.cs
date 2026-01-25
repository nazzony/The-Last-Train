using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool hasKey = false;
    public bool hasCoin = false;
    public bool isTrashSearched = false;
    public bool isMachineUsed = false;


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
}
