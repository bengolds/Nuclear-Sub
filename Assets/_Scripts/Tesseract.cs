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
        var powerTerm = coll.gameObject.GetComponentInParent<PowerTerminal>();
        Debug.Log(coll.gameObject.name);
        if (powerTerm != null)
        {
            powerTerm.PowerOn();
            Debug.Log("power term: " + powerTerm.name);
        }
    }
}
