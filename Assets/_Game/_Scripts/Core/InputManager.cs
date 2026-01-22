using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0f) return;
        if(Input.GetMouseButtonDown (0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D isHit = Physics2D.Raycast(worldPos, Vector2.zero);
            if(isHit.collider != null && isHit.collider.CompareTag("Ground"))
            {
                EventManager.current.TriggerPlayerMove(isHit.point);
            }

        }
    }
}
