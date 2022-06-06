using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkSpaceStarter : MonoBehaviour
{
    [SerializeField] private Drink _drink;

    private void Start()
    {
        OrderCreationEvents.Instance.StartCreatingDrink(_drink);
    }
}
