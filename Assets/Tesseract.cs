using UnityEngine;
using System.Collections.Generic;

public class Tesseract : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision coll)
    {
        var powerTerm = coll.gameObject.GetComponent<PowerTerminal>();
        if (powerTerm != null)
        {
            powerTerm.PowerOn();
        }
    }
}
