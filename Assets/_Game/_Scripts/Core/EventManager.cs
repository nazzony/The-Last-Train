using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{

    public static EventManager current;
   

    private void Awake()
    {
        current = this; 
    }

    public event Action<Vector2> onPlayerMove;
    public void TriggerPlayerMove(Vector2 pos)
    {
        if(onPlayerMove != null)
        {
            onPlayerMove(pos);
        }
    }   
}
