using UnityEngine;
using System.Collections.Generic;

public class IgnoreCollisionsWith : MonoBehaviour {

    public GameObject collidersParent;
	// Use this for initialization
	void Start () {
        var colliders = collidersParent.GetComponentsInChildren<Collider>();
        foreach (var coll in colliders)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), coll);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
