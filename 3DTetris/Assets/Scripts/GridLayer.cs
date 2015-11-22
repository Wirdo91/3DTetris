using UnityEngine;
using System.Collections;

public class GridLayer {

    private Transform[,] _layer;

    public Transform[,] Layer
    {
        get { return _layer; }
        set { _layer = value; IsFull(); }
    }

    public bool Full
    {
        get;
        private set;
    }

    public void SetTransform(int x, int z, Transform t)
    {
        _layer[x, z] = t;
        IsFull();
    }
    public void SetTransform(float x, float z, Transform t)
    {
        SetTransform((int)x, (int)z, t);
    }

    //TODO Needs to be able to change size of board
    public GridLayer()
    {
        Full = false;
        _layer = new Transform[8,8];
    }

    public void Despawn()
    {
        foreach (Transform item in _layer)
        {
            if (item != null)
                GameObject.Destroy(item.gameObject);
        }
    }

    public void UpdatePosition(int height)
    {
        foreach (Transform item in _layer)
        {
            if (item != null)
                item.position = new Vector3(item.position.x, height, item.position.z);
        }
    }

    bool IsFull()
    {
        foreach (Transform item in _layer)
        {
            if (item == null)
            {
                Full = false;
                return Full;
            }
        }
        Full = true;
        return Full;
    }
}
