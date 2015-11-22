using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCreator : MonoBehaviour {

	[SerializeField]
	GameObject vert;
	[SerializeField]
	GameObject hori;

    int _gameHeight = 16;

    List<GridLayer> _gameGrid;


    //Grid should be 10x10x20
	void Start ()
    {
        DrawGrid();

        _gameGrid = new List<GridLayer>();

        for (int i = 0; i < _gameHeight; i++)
        {
            _gameGrid.Add(new GridLayer());
        }
	}

    void Update()
    {
    }

    //TODO Should be more versatile
    //Account for various grid sizes
    void DrawGrid()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(vert, this.transform.position + new Vector3(4, 8, i), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(vert, this.transform.position + new Vector3(4, 8, -i), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(vert, this.transform.position + new Vector3(-4, 8, i), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(vert, this.transform.position + new Vector3(-4, 8, -i), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;

            Instantiate(vert, this.transform.position + new Vector3(i, 8, 4), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(vert, this.transform.position + new Vector3(-i, 8, 4), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(vert, this.transform.position + new Vector3(i, 8, -4), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(vert, this.transform.position + new Vector3(-i, 8, -4), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
        }
        for (int y = 0; y < 17; y++)
        {
            Instantiate(hori, this.transform.position + new Vector3(4, y, 0), Quaternion.Euler(0, 90, 0)).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(hori, this.transform.position + new Vector3(-4, y, 0), Quaternion.Euler(0, 90, 0)).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(hori, this.transform.position + new Vector3(0, y, 4), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
            Instantiate(hori, this.transform.position + new Vector3(0, y, -4), Quaternion.identity).hideFlags = HideFlags.HideInHierarchy;
        }
    }
}
