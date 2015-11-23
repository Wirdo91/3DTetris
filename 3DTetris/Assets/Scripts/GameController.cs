using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	BlockManager _blockManager;

    ScoreSystem score;

    [SerializeField]
    int scoreForLayer, scoreForLayerMultiplier;

    List<GridLayer> _gameMatrix;

    [SerializeField]
    Transform _cameraAnchor;

    [SerializeField]
	float _moveDelay = 1.0f;
	float _moveTimer = 0.0f;

    int _height = 16;

	Block currentBlock = null;

    bool gameOver = true;
    public bool Paused = false;

    public delegate void OnEndGame();
    public event OnEndGame EndGame;

	// Use this for initialization
	void Start () {
		_blockManager = FindObjectOfType (typeof(BlockManager)) as BlockManager;

        score = GetComponent<ScoreSystem>();

        _gameMatrix = new List<GridLayer>();

        for (int i = 0; i < _height; i++)
        {
            _gameMatrix.Add(new GridLayer());
        }
	}

    public void StartGame()
    {
        if (currentBlock != null)
        {
            Destroy(currentBlock.gameObject);
            currentBlock = null;
        }
        foreach (GridLayer layer in _gameMatrix)
        {
            layer.Despawn();
        }
        _gameMatrix = new List<GridLayer>();
        for (int i = 0; i < _height; i++)
        {
            _gameMatrix.Add(new GridLayer());
        }

        gameOver = false;
        Paused = false;
    }

    public void GameOver()
    {
        gameOver = true;
        score.EndGame();
        EndGame();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameOver || Paused)
        {
            return;
        }

		if (currentBlock == null)
        {
			currentBlock = _blockManager.SpawnNewBlock ();
			currentBlock.gameObject.SetActive(true);
		}
        else {
			if (_moveTimer < 0)
            {
                if (IsBlocksBlocked(currentBlock, Vector3.down, Vector3.zero))
                {
                    foreach (Transform item in currentBlock.InternalBlocks)
                    {
                        if (item.position.y >= _height)
                        {
                            GameOver();
                            return;
                        }
                        _gameMatrix[(int)item.position.y].SetTransform(item.position.x, item.position.z, item);
                        item.parent = this.transform;
                    }

                    score.AddScore(currentBlock.Point);

                    Destroy(currentBlock.gameObject);
                    currentBlock = null;

                    int layerscore = 0;
                    for (int l = _gameMatrix.Count - 1; l >= 0; l--)
                    {
                        if(_gameMatrix[l].Full)
                        {
                            _gameMatrix[l].Despawn();
                            _gameMatrix.RemoveAt(l);
                            _gameMatrix.Add(new GridLayer());
                            for (int i = l; i < _gameMatrix.Count; i++)
                            {
                                _gameMatrix[i].UpdatePosition(i);
                            }

                            if (layerscore > 0)
                                layerscore *= scoreForLayerMultiplier;
                            else
                                layerscore += scoreForLayer;
                        }
                    }
                    score.AddScore(layerscore);
                }
                else
                {
                    currentBlock.transform.Translate(Vector3.down, Space.World);
                }
				_moveTimer = _moveDelay;
			}
			_moveTimer -= Time.deltaTime;
		}
	}

    bool IsBlocksBlocked(Block block, Vector3 dirChange, Vector3 rotChange)
    {
        Vector3 blockPosition;
        foreach (Transform intBlock in block.InternalBlocks)
        {
            blockPosition = block.Position;
            //Rotation of the internal blocks
            blockPosition += Quaternion.Euler(rotChange) * intBlock.localPosition;
            //Movement of the internal blocks
            blockPosition += dirChange;
            
            //Checks Out of Bounds
            if (blockPosition.x < 0 || blockPosition.x > 7 ||
                blockPosition.z < 0 || blockPosition.z > 7)
            {
                return true;
            }
            //Check if bottom is reached
            else if (blockPosition.y < 0)
            {
                return true;
            }
            //Ignores if the block is above the grid
            else if (blockPosition.y > _height - 1)
            {
                continue;
            }
            //Check if grid position is occupied
            else if (_gameMatrix[(int)blockPosition.y].Layer[(int)blockPosition.x, (int)blockPosition.z] != null)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 rotModifier = Vector3.zero;
    Vector3 posModifier = Vector3.zero;
    void UpdateBlockTransform()
    {
        if (rotModifier + posModifier == Vector3.zero || currentBlock == null)
            return;

        Vector3 camAnchorEuler = _cameraAnchor.rotation.eulerAngles;
        camAnchorEuler.x = 0;
        camAnchorEuler.y = 90 * (int)Mathf.Round(camAnchorEuler.y / 90);
        camAnchorEuler.z = 0;

        Quaternion camRotModifier = Quaternion.Euler(camAnchorEuler);

        rotModifier = camRotModifier * rotModifier;
        posModifier = camRotModifier * posModifier;

        if (!IsBlocksBlocked(currentBlock, posModifier, rotModifier))
        {
            currentBlock.Rotate(rotModifier);
            currentBlock.transform.position += posModifier;
        }
        rotModifier = Vector3.zero;
        posModifier = Vector3.zero;
    }

    public void Move2Bottom()
    {
        for (int i = (int)currentBlock.Position.y; i >= 0; i--)
        {
            if (!IsBlocksBlocked(currentBlock, Vector3.down * i, Vector3.zero))
            {
                posModifier = Vector3.down * i;
                UpdateBlockTransform();
                break;
            }
        }
    }

    public void MoveX(float value)
    {
        posModifier.x = value;
        UpdateBlockTransform();
    }
    public void MoveZ(float value)
    {
        posModifier.z = value;
        UpdateBlockTransform();
    }
    public void RotateX(float value)
    {
        rotModifier.x = value;
        UpdateBlockTransform();
    }
    public void RotateY(float value)
    {
        rotModifier.y = value;
        UpdateBlockTransform();
    }
    public void RotateZ(float value)
    {
        rotModifier.z = value;
        UpdateBlockTransform();
    }

    void OnGUI()
    {
        if (currentBlock == null)
            return;
        GUI.Label(new Rect(0, 0, 100, 500),
            string.Format("Current Block: {0} \n Internal Block 1: X: {1} Y: {2} Z {3} \n Internal Block 2: X: {4} Y: {5} Z {6} \n Internal Block 3: X: {7} Y: {8} Z {9} \n Internal Block 4: X: {10} Y: {11} Z {12}",
            currentBlock.name,
            currentBlock.InternalBlocks[0].position.x, currentBlock.InternalBlocks[0].position.y, currentBlock.InternalBlocks[0].position.z,
            currentBlock.InternalBlocks[1].position.x, currentBlock.InternalBlocks[1].position.y, currentBlock.InternalBlocks[1].position.z,
            currentBlock.InternalBlocks[2].position.x, currentBlock.InternalBlocks[2].position.y, currentBlock.InternalBlocks[2].position.z,
            currentBlock.InternalBlocks[3].position.x, currentBlock.InternalBlocks[3].position.y, currentBlock.InternalBlocks[3].position.z));

    }

    /*
    float buttonSize = 25;
	void OnGUI()
	{
        Vector3 rotModifier = Vector3.zero;
        Vector3 posModifier = Vector3.zero;

        //style.onNormal.background = Resources.Load<Texture2D>("Sprites\\Resources\\ZAxisRight.png");
		if (GUI.Button (new Rect (5, Screen.height -100 - 5 - buttonSize, buttonSize, buttonSize), "")) {
            rotModifier += (Vector3.forward * 90);
		}
		if (GUI.Button (new Rect (5 + buttonSize, Screen.height -100 - 5 - buttonSize - buttonSize, buttonSize, buttonSize), "|")) {
            rotModifier += (Vector3.right * 90);
		}
		if (GUI.Button (new Rect (5 + buttonSize + buttonSize, Screen.height -100 - 5 - buttonSize, buttonSize, buttonSize), "-")) {
            rotModifier += (Vector3.back * 90);
		}
		if (GUI.Button (new Rect (5 + buttonSize, Screen.height -100 - 5 - buttonSize, buttonSize, buttonSize), "|")) {
            rotModifier += (Vector3.left * 90);
		}
		if (GUI.Button (new Rect (5 + buttonSize + buttonSize, Screen.height -100 - 5 - buttonSize - buttonSize, buttonSize, buttonSize), "/-")) {
            rotModifier += (Vector3.up * 90);
		}
		if (GUI.Button (new Rect (5, Screen.height -100 - 5 - buttonSize - buttonSize, buttonSize, buttonSize), "-\\")) {
            rotModifier += (Vector3.down * 90);
		}
		
		if (GUI.Button (new Rect (5, Screen.height - 5 - buttonSize, buttonSize, buttonSize), "-")) {
            posModifier = Vector3.left;
		}
		if (GUI.Button (new Rect (5 + buttonSize, Screen.height - 5 - buttonSize, buttonSize, buttonSize), "|")) {
            posModifier = Vector3.back;
        }
		if (GUI.Button (new Rect (5 + buttonSize, Screen.height - 5 - buttonSize - buttonSize, buttonSize, buttonSize), "|")) {
            posModifier = Vector3.forward;
        }
		if (GUI.Button (new Rect (5 + buttonSize + buttonSize, Screen.height - 5 - buttonSize, buttonSize, buttonSize), "-")) {
            posModifier = Vector3.right;
        }

        if (rotModifier + posModifier == Vector3.zero || currentBlock == null)
            return;

        if (!IsBlocksBlocked(currentBlock, posModifier, rotModifier))
        {
            currentBlock.Rotate(rotModifier);
            currentBlock.transform.position += posModifier;
        }
    }*/
}
