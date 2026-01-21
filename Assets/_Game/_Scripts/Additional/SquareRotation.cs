using UnityEngine;

public class SquareRotation : MonoBehaviour
{
    [SerializeField] private float _speed = 200;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, _speed * Time.deltaTime);
    }
}
