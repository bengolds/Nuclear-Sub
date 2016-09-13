using UnityEngine;
using System.Collections;

public class BasicWatchableBool : HasWatchableBool
{
    public bool startingValue;
    
    void Start()
    {
        SilentSetBoolValue(startingValue);
    }
}