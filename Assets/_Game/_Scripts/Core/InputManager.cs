using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public Transform playerTransform;
    void Update()
    {
        if (Time.timeScale == 0f) return;

        if(Input.GetMouseButtonDown (0))
        {



            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D isHit = Physics2D.Raycast(worldPos, Vector2.zero);


            if (isHit.collider != null && isHit.collider.CompareTag("Ground"))
            {
                EventManager.current.TriggerPlayerMove(isHit.point);
            } 


            else if (isHit.collider != null && isHit.collider.CompareTag("Door"))
            {
                float distance = Vector2.Distance(playerTransform.position, isHit.transform.position);
                if (distance <= 2.35f)
                {
                    DoorHandling number = isHit.collider.GetComponent<DoorHandling>();
                    GameData.TargetDoorId = number.getTargetId();
                    if(number != null)
                    {
                        if ((number.isLocked && GameManager.instance.hasKey) || !number.isLocked)
                        {
                            AudioManager.instance.playSFX(AudioManager.instance.doorSound);
                            SceneFader.instance.LoadScene(number.getSceneId());
                        }
                        else if (number.isLocked && !GameManager.instance.hasKey)
                        {
                            Debug.Log("Need A Key");
                        }
                    }
                }
            }

            else if (isHit.collider != null && isHit.collider.CompareTag("TrashBin"))
            {
                float distance = Vector2.Distance(playerTransform.position, isHit.transform.position);
                if(distance <= 2f)
                {
                    if (GameManager.instance.isTrashSearched == false)
                    {
                        GameManager.instance.isTrashSearched = true;
                        GameManager.instance.hasCoin = true;
                        AudioManager.instance.playSFX(AudioManager.instance.coinSound);
                        Debug.Log("Picked up a coin");
                    }
                    else
                    {
                        AudioManager.instance.playSFX(AudioManager.instance.trashSound);
                        Debug.Log("Only Trash Here");
                    }
                }
            }

            else if (isHit.collider != null && isHit.collider.CompareTag("VendingMachine"))
            {
                float distance = Vector2.Distance(playerTransform.position, isHit.transform.position);
                if( distance <= 2f)
                {
                    if (GameManager.instance.isMachineUsed == true)
                    {
                        Debug.Log("Doesn't work anymore");
                        AudioManager.instance.playSFX(AudioManager.instance.vendingSound);
                    }
                    else if (GameManager.instance.isMachineUsed == false && GameManager.instance.hasCoin == true)
                    {
                        GameManager.instance.hasCoin = false;
                        GameManager.instance.isMachineUsed = true;
                        GameManager.instance.hasKey = true;
                        AudioManager.instance.playSFX(AudioManager.instance.keySound);
                        Debug.Log("Got a Key!");
                    }
                    else if (GameManager.instance.isMachineUsed == false && GameManager.instance.hasCoin == false)

                    {
                        Debug.Log("Don't have any money");
                        AudioManager.instance.playSFX(AudioManager.instance.vendingSound);
                    }
                }
               
            }



        }
    }
}
