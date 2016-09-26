using UnityEngine;
using System.Collections;

public class DisableInBuild : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if (Application.isEditor)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
