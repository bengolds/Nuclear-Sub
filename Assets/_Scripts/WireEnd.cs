using UnityEngine;
using System.Collections.Generic;

public class WireEnd : MonoBehaviour {
    public WireEnd otherEnd;
    private PowerTerminal connectedTo;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public bool isPowered() {
        return connectedTo != null && connectedTo.isPoweredOn;
    }

    void OnCollisionStay(Collision collision)
    {
        TouchingObject(collision.gameObject);

    }

    void OnTriggerStay(Collider other)
    {
        TouchingObject(other.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        LeavingObject(collision.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        LeavingObject(other.gameObject);
    }

    void TouchingObject(GameObject touching)
    {
        var powerTerminal = touching.GetComponent<PowerTerminal>();
        if (powerTerminal != null)
        {
            connectedTo = powerTerminal;
            if (otherEnd.isPowered())
            {
                connectedTo.PowerOn();
            }
        }
    }

    void LeavingObject(GameObject leaving)
    {
        var powerTerminal = leaving.GetComponent<PowerTerminal>();
        if (powerTerminal == connectedTo)
        {
            connectedTo = null;
        }
    }
}
