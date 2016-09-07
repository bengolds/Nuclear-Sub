using UnityEngine;
using System.Collections;

public class Ignition : HasWatchableBool {

    public PowerTerminal powerTerminal;

    public void TryTurnOn()
    {
        if (powerTerminal.isPoweredOn)
        {
            boolValue = true;
        }
    }
}
