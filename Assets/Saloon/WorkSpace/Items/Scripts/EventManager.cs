using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityEvent<bool> DragBegin = new UnityEvent<bool>();

    public static void SendDragBegin(bool a)
    {
        DragBegin.Invoke(a);
    }
}
