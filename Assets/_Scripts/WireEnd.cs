using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using VRTK;

public class WireEnd : MonoBehaviour {
    public WireEnd otherEnd;
    public GameObject sparkObject;
    public AkEvent sparkOnEvent;
    public AkEvent sparkOffEvent;
    public GameObject wireEndPrefab;
    public Transform wireEndAttachPoint;

    private VRTK_InteractableObject io;
    private Rigidbody rb;
    private bool soundPlaying = false;
    private PowerTerminal connectedTo;
    private ParticleSystem[] particleSystems;

    // Use this for initialization
    void Start() {
        particleSystems = sparkObject.GetComponentsInChildren<ParticleSystem>();
        io = GetComponent<VRTK_InteractableObject>();
        rb = GetComponent<Rigidbody>();
        TurnOffSparks();
    }

    void Update()
    {
        if (connectedTo != null && connectedTo.isPoweredOn)
        {
            TurnOnSparks();
        }
        else
        {
            TurnOffSparks();
        }

        if (io.IsGrabbed())
        {
            rb.mass = 1000;
        }
        else
        {
            rb.mass = 1;
        }
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
        var powerTerminal = touching.GetComponentInParent<PowerTerminal>();
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
        var powerTerminal = leaving.GetComponentInParent<PowerTerminal>();
        if (powerTerminal == connectedTo)
        {
            connectedTo = null;
        }
    }

    void TurnOnSparks()
    {
        foreach (var ps in particleSystems)
        {
            var em = ps.emission;
            em.enabled = true;
        }
        if (!soundPlaying)
        {
            AkSoundEngine.PostEvent((uint)sparkOnEvent.eventID, gameObject);
            soundPlaying = true;
        }
    }

    void TurnOffSparks()
    {
        foreach (var ps in particleSystems)
        {
            var em = ps.emission;
            em.enabled = false;
        }
        if (soundPlaying)
        {
            AkSoundEngine.PostEvent((uint)sparkOffEvent.eventID, gameObject);
            soundPlaying = false;
        }
    }
}
