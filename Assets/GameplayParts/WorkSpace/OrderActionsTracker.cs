using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderActionsTracker
{
    private readonly HashSet<OrderAction> _trackedActions;

    public OrderActionsTracker()
    {
        _trackedActions = new HashSet<OrderAction>();
    }

    public void AddAction(OrderAction action)
    {
        _trackedActions.Add(action);
    }

    public IReadOnlyList<OrderAction> GetOrderActions() => _trackedActions.ToArray();
}