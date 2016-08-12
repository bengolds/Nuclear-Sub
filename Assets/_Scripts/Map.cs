using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public Material poweredOnMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PowerOn()
    {
        GetComponent<MeshRenderer>().material = poweredOnMaterial;
    }
}
