using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [Header("Quest Items")]
    public ItemData coinItemData;
    public ItemData keyItemData;

    public Transform playerTransform;
    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetMouseButtonDown(0))
        {
            //AudioManager.instance.playSFX(AudioManager.instance.clickSound);

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D isHit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (isHit.collider == null) return;

            //Ground
            if (isHit.collider != null && isHit.collider.CompareTag("Ground"))
            {
                EventManager.current.TriggerPlayerMove(isHit.point);
            }

            //Door
            else if (isHit.collider != null && isHit.collider.CompareTag("Door"))
            {
                float distance = Vector2.Distance(playerTransform.position, isHit.transform.position);
                bool playerHasKey = InventoryManager.instance.HasItemType(ItemData.ItemType.Key);
                if (distance <= 2.35f)
                {
                    DoorHandling number = isHit.collider.GetComponent<DoorHandling>();
                    GameData.TargetDoorId = number.getTargetId();
                    if (number != null)
                    {
                        string allItems = "В кишені: ";
                        foreach (var item in InventoryManager.instance.items) allItems += item.itemType + ", ";
                        Debug.Log(allItems);
                        
                        bool hasKeyInBag = InventoryManager.instance.HasItemType(ItemData.ItemType.Key);

                        if ((number.isLocked && hasKeyInBag) || !number.isLocked)
                        {
                            Debug.Log("Ключ підійшов! Відчиняю.");
                            AudioManager.instance.playSFX(AudioManager.instance.clickSound);
                            SceneFader.instance.LoadScene(number.getSceneId());
                        }
                        else
                        {
                            Debug.Log("Двері зачинені. Потрібен ключ (Type: Key).");
                            AudioManager.instance.playSFX(AudioManager.instance.clickSound);
                        }
                    }
                }
            }

            //Trash Bin
            else if (isHit.collider != null && isHit.collider.CompareTag("TrashBin"))
            {
                float distance = Vector2.Distance(playerTransform.position, isHit.transform.position);
                if (distance <= 2f)
                {
                    if (GameManager.instance.isTrashSearched == false)
                    {
                        GameManager.instance.isTrashSearched = true;

                        InventoryManager.instance.AddItem(coinItemData);

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
            else
            {
                float distance = Vector2.Distance(playerTransform.position, isHit.transform.position);
                IItemReceiver receiver = isHit.collider.GetComponent<IItemReceiver>();
                if (distance <= 2f)
                {
                    if (receiver != null)
                    {
                        receiver.TryAcceptItem(null, InventoryManager.instance);
                    }
                }
            }
        }
    }
}