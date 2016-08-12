using UnityEngine;
using System.Collections.Generic;

public class AkToggleEvent : MonoBehaviour {

    public AkEvent onEvent;
    public AkEvent offEvent;
    public bool startsOn = false;
    private bool on;

	// Use this for initialization
	void Start () {
        on = startsOn;
	}
	
    public void Toggle()
    {
        uint id = 0;
        if (on)
        {
            id = (uint)offEvent.eventID;
        }
        else
        {
            id = (uint)onEvent.eventID;
        }
        AkSoundEngine.PostEvent(id, gameObject);
        on = !on;
    }
}
