using UnityEngine;
using System;
using System.IO;

[RequireComponent(typeof(AssetArray))]
public class GeneratePaper : MonoBehaviour {
    public GameObject paperPrefab;
    public Vector3 localOffset;
    private Vector3[] localPositions;

	// Use this for initialization
	void Start () {
        CalculatePositions();
        InstantiateAtPositions();
	}

    string[] GetTitles()
    {
        return GetComponent<AssetArray>().objects;
    }

    void CalculatePositions()
    {
        localPositions = new Vector3[GetTitles().Length];
        Vector3 worldOffset = transform.TransformVector(localOffset);
        Vector3 firstPosition = transform.position - (worldOffset * (GetTitles().Length-1))/2;
        for (int i = 0; i < GetTitles().Length; i++)
        {
            localPositions[i] = transform.InverseTransformPoint(firstPosition + worldOffset * i);
        }
    }

    void InstantiateAtPositions()
    {
        string[] titles = GetTitles();
        for (int i = 0; i < titles.Length; i++)
        {
            Vector3 worldPosition = transform.TransformPoint(localPositions[i]);
            var newObj = Instantiate(paperPrefab, worldPosition, transform.rotation) as GameObject;
            newObj.transform.SetParent(this.transform, true);
            var tex = Resources.Load<Texture2D>(GetPathInResources(titles[i]));
            newObj.GetComponent<Paper>().SetToImage(tex);
        }
    }
    
    string GetPathInResources(string fullPath)
    {
        //Get everything after Resources/.
        string resources = "Resources/";
        int resourcesIndex = fullPath.IndexOf(resources);
        if (resourcesIndex < 0)
        {
            Debug.LogError("The documents aren't in a resources folder. What do you expect?");
            return String.Empty;
        }
        string subpath = fullPath.Substring(resourcesIndex + resources.Length);
        
        //This is the easiest way to get rid of the file extension.
        string filename = Path.GetFileNameWithoutExtension(subpath);
        string basePath = Path.GetDirectoryName(subpath);
        return basePath + "/" + filename;
    }

    void OnDrawGizmos()
    {
        CalculatePositions();
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Vector3 size = paperPrefab.GetComponent<BoxCollider>().size;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        for (int i = 0; i < GetTitles().Length; i++)
        {
            Gizmos.DrawCube(localPositions[i], size);
        }
        Gizmos.matrix = oldMatrix;
    }
}
