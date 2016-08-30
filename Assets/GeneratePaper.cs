using UnityEngine;

public class GeneratePaper : MonoBehaviour {
    public GameObject paperPrefab;
    public Vector3 offset;
    public string[] titles;
    private Vector3[] positions;

	// Use this for initialization
	void Start () {
        CalculatePositions();
        InstantiateAtPositions();
	}

    void CalculatePositions()
    {
        positions = new Vector3[titles.Length];
        Vector3 firstPosition = transform.position - (offset * (titles.Length-1))/2;
        for (int i = 0; i < titles.Length; i++)
        {
            positions[i] = firstPosition + offset * i;
        }
    }

    void InstantiateAtPositions()
    {
        for (int i = 0; i < titles.Length; i++)
        {
            var newObj = Instantiate(paperPrefab, positions[i], transform.rotation) as GameObject;
            newObj.transform.SetParent(this.transform, true);
            var tex = Resources.Load<Texture2D>(titles[i]);
            newObj.GetComponent<Paper>().SetToImage(tex);
        }
    }
    
    void OnDrawGizmos()
    {
        CalculatePositions();
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Vector3 size = paperPrefab.GetComponent<BoxCollider>().size;
        for (int i = 0; i < titles.Length; i++)
        {
            Gizmos.DrawCube(positions[i], size);
        }
    }
}
