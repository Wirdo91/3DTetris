using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockCreator : MonoBehaviour {

	List<Transform> _blocks;
	GameObject _currentBlock;

	// Use this for initialization
	void Start () {
		_blocks = new List<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.Box (new Rect (Vector2.zero, new Vector2 (Screen.width / 3, Screen.height)), "");

		if (GUI.Button (new Rect (5 * Vector2.one, new Vector2 (100, 25)), "New Block")) {
			GameObject _newBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
			_newBlock.transform.position = new Vector3(.5f, 0, .5f);

			_blocks.Add(_newBlock.transform);
		}

		if (_currentBlock == null) {
			GUI.enabled = false;
		}

		if (GUI.Button (new Rect (5 * Vector2.one + new Vector2 (0, 30), new Vector2 (100, 25)), "Remove Block")) {
			_blocks.Remove(_currentBlock.transform);
			Destroy(_currentBlock);
			_currentBlock = null;
		}
	}
}
