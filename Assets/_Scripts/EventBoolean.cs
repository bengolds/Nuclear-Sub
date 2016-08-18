using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventBoolean : MonoBehaviour {

    public bool on = false;
    public UnityEvent onOn;
    public UnityEvent onOff;

	public void TurnOn()
    {
        on = true;
    }

    public void TurnOff()
    {
        on = true;
    }

    public void Execute()
    {
        if (on)
        {
            onOn.Invoke();
        } else
        {
            onOff.Invoke();
        }
    }
}
