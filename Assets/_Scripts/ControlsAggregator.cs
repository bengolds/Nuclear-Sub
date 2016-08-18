using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using VRTK;

public class ControlsAggregator : MonoBehaviour {

    public UnityEvent onAllOn;
    public UnityEvent onAnyOff;
    private VRTK_Control[] controls;
    private bool lastAllOn;

    void Start()
    {
        controls = GetComponentsInChildren<VRTK_Control>();
    }

    void Update()
    {
        bool allOn = true;
        foreach (var control in controls)
        {
            //In this case, on is 0 and off is 100.
            if (control.getValue() != 0)
            {
                allOn = false;
            }
        }

        if (allOn && !lastAllOn)
        {
            onAllOn.Invoke();
        }
        else if (!allOn && lastAllOn)
        {
            onAnyOff.Invoke();
        }
        lastAllOn = allOn;
    }

}
