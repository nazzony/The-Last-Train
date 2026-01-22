using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InputManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform doorsTransform;
    void Update()
    {
        if (Time.timeScale == 0f) return;

        if(Input.GetMouseButtonDown (0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D isHit = Physics2D.Raycast(worldPos, Vector2.zero);
            float distance = Vector2.Distance(playerTransform.position, doorsTransform.position);

            if (isHit.collider != null && isHit.collider.CompareTag("Ground"))
            {
                EventManager.current.TriggerPlayerMove(isHit.point);
            } 

            else if (isHit.collider != null && isHit.collider.CompareTag("Door") && distance <= 2.3f)
            {
                DoorHandling number = isHit.collider.GetComponent<DoorHandling>();
                if (number != null)
                {
                    SceneManager.LoadScene(number.toSceneNumber);
                }
            }

        }
    }
}
