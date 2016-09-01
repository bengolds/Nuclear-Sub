using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AssetArray))]
public class testobj : MonoBehaviour {

    public Vector3[] vectors;
    public string[] strings;
    public int[] ints;
    public string test;
    private AssetArray assets;

	// Use this for initialization
	void Start () {
        assets = GetComponent<AssetArray>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
