using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public static class EventManager
{
   public static UnityEvent ArrowActivatedEvent = new UnityEvent();
   public static UnityEvent<List<GameObject>> GetArrowEvent = new UnityEvent<List<GameObject>>();
}
