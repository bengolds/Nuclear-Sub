using UnityEngine;
using System.Collections.Generic;

public class SafeBolt : MonoBehaviour {

	public Transform chiselSnapPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		var chisel = other.GetComponent<Chisel> ();
		if (chisel != null) {
			chisel.LockIntoPlace (chiselSnapPoint);
		}
	}
//
//	bool isChisel(Collider other) {
//		return other.GetComponent<Chisel>() != null;
//	}
}
