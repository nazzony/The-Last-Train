using UnityEngine;
using UnityEngine.Events;

public class TempMovement : MonoBehaviour
{
    [SerializeField]
    private string _horizontalAxis = "Horizontal";
    [SerializeField]
    private Rigidbody2D _rb2d;
    [SerializeField]
    private float _speed = 3f;

    public UnityEvent OnPlayerDie;

    private Vector2 _input;

    private void FixedUpdate()
    {
        _rb2d.linearVelocity = _input * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw(_horizontalAxis); // left and right movement
        _input = new Vector2(horizontalInput, 0);
        _input.Normalize();
    }
}
