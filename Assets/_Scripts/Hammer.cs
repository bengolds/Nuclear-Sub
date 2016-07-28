using UnityEngine;
using System.Collections.Generic;
using VRTK;

public class Hammer : VRTK_InteractableObject {
	public Transform impactPoint;
	[HideInInspector]
	public Rigidbody rigidbody;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
		

}
