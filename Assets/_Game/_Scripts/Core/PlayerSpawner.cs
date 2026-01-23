using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform player;
    void Start()
    {
        if (GameData.TargetDoorId == -1) { return; } 
        else
        {
            DoorHandling[] _doors = FindObjectsByType<DoorHandling>(FindObjectsSortMode.None);
            foreach (DoorHandling door in _doors)
            {
                if (door.getCurrentId() == GameData.TargetDoorId)
                {
                    player.position = door.getSpawnPoint().transform.position;
                }
            }
        }
    }
    void Update()
    {
        
    }
}
