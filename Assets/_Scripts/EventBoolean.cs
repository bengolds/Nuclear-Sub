using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventBoolean : MonoBehaviour {

    public GameObject objectWithBoolean;
    public UnityEvent onOn;
    public UnityEvent onOff;
    private IWatchableBool watchedBool;
    
    void Start()
    {
        watchedBool = objectWithBoolean.GetComponent<IWatchableBool>();
        if (watchedBool == null)
        {
            Debug.LogError("No IWatchableBool on " + objectWithBoolean.name);
        }
    }

    public void Execute()
    {
        if (watchedBool.boolValue)
        {
            onOn.Invoke();
        } else
        {
            onOff.Invoke();
        }
    }
}
