using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {

	[SerializeField]
	Vector3 _startPos;

	[SerializeField]
	Block[] _availableBlocks;

	[SerializeField]
	Transform[] _Blocks;

	// Use this for initialization
	void Start () {
		_Blocks = new Transform[this.transform.childCount];
		for (int i = 0; i < this.transform.childCount; i++) {
			_Blocks[i] = this.transform.GetChild(i);
		}
	}

	public Block SpawnNewBlock()
	{
		GameObject newBlock = (GameObject) Instantiate (_Blocks [Random.Range (0, _Blocks.Length)].gameObject, _startPos, Quaternion.identity);
		return newBlock.GetComponent<Block>();
	}

    void LoadFromResources()
    {
        TextAsset[] blocks = Resources.LoadAll("Blocks") as TextAsset[];

        foreach (TextAsset block in blocks)
        {
            //Spawn iniBlockTemplate

            foreach (string item in block.text.Split('|'))
            {
                string[] XYZ = item.Split(',');
                GameObject intBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                intBlock.transform.position =
                    new Vector3(float.Parse(XYZ[0]), float.Parse(XYZ[1]), float.Parse(XYZ[2]));
                //Set iniBlockTemplate as parent
            }

        }
    }
}
