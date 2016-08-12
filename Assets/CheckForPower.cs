using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CheckForPower : MonoBehaviour {

    public bool powered = false;
    public UnityEvent eventToExecute;

	public void PowerOn()
    {
        powered = true;
    }

    public void ExecuteIfPowered()
    {
        if (powered)
        {
            eventToExecute.Invoke();
        }
    }
}
