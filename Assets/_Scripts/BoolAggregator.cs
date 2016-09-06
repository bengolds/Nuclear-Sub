using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum BoolAggregatorType
{
    Or,
    And
}

public class BoolAggregator : MonoBehaviour, IWatchableBool {

    public GameObject[] objectsWithBools;
    public GameObject[] objectsWithNegatedBools;
    public BoolAggregatorType aggregationType;

    private List<IWatchableBool> watchedBools;
    private List<IWatchableBool> watchedNegatedBools;
    private bool oldValue, currValue;

    public event WatchableBoolEventHandler OnBoolValueChanged;

    public bool boolValue
    {
        get
        {
            return currValue;
        }
    }

    // Use this for initialization
    void Start () {
        watchedBools = new List<IWatchableBool>();
        watchedNegatedBools = new List<IWatchableBool>();
        foreach (var obj in objectsWithBools)
        {
            watchedBools.AddRange(obj.GetComponentsInChildren<IWatchableBool>());
        }
        foreach (var obj in objectsWithNegatedBools)
        {
            watchedNegatedBools.AddRange(obj.GetComponentsInChildren<IWatchableBool>());
        }
        watchedBools.Remove(this);
        watchedNegatedBools.Remove(this);
    }
	
	// Update is called once per frame
	void Update () {
        currValue = CalculateValue();
        if (currValue != oldValue && OnBoolValueChanged != null)
        {
            OnBoolValueChanged.Invoke(this, currValue);
        }

        oldValue = currValue;
	}

    bool CalculateValue()
    {
        if (aggregationType == BoolAggregatorType.Or)
        {
            return watchedBools.Any(watched => watched.boolValue) ||
                watchedNegatedBools.Any(watched => !watched.boolValue);
        } else if (aggregationType == BoolAggregatorType.And) {
            return watchedBools.All(watched => watched.boolValue) &&
                watchedNegatedBools.All(watched => !watched.boolValue);
        } else
        {
            return false;
        }
    }
}
