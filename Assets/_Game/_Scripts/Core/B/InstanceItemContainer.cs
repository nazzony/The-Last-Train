/*
 * Компонент на предметі у світі.
 * Тримає посилання на ItemData (SO), щоб інвентар знав, що підбирається.
 * місток між предметом у сцені та інвентарем.
 */
using UnityEngine;

public class InstanceItemContainer : MonoBehaviour
{
    public ItemData item;

    public ItemData TakeItem()
    {
        ItemData result = item;
        item = null;
        return result;
    }
}