using UnityEngine;

public class DoorHandling : MonoBehaviour
{
    [SerializeField] private int _currentId;
    [SerializeField] private int _targetDoorId;
    [SerializeField] private int toSceneNumber;

    public int getSceneId() {  return toSceneNumber; }
    public int getTargetId() { return _targetDoorId; }
    public int getCurrentId() { return _currentId; }
}
