using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    enum ColorPosition
    {
        EDGE,
        FACE
    }

    [SerializeField]
    Transform[] _internalBlocks;

    [SerializeField]
    ColorPosition _colorPosition;

    [SerializeField]
    Color color;

    [SerializeField]
    int _point;

    public int Point
    {
        get { return _point; }
    }

    public Transform[] InternalBlocks
    {
        get { return _internalBlocks; }
    }

    public Vector3 Position
    {
        get { return this.transform.position; }
    }

    Vector3 _rotation = Vector3.zero;
    public Vector3 Rotation
    {
        get { return _rotation; }
    }
    
    public void Rotate(Vector3 rot)
    {
        _rotation += rot;
        foreach(Transform t in InternalBlocks)
        {
            t.localPosition = Quaternion.Euler(rot) * t.localPosition;
            //Since rotation results in weird values, each values is not rounded of;
            t.localPosition = new Vector3(Mathf.Round(t.localPosition.x), Mathf.Round(t.localPosition.y), Mathf.Round(t.localPosition.z));
        }
        //this.transform.eulerAngles = _rotation;
    }

    void Start ()
    {
        _internalBlocks = new Transform[4];
        FindChildBlocks();

        foreach (Renderer ren in this.GetComponentsInChildren<Renderer>())
        {
            switch(_colorPosition)
            {
                case ColorPosition.EDGE:
                    ren.materials[0].color = color;
                    ren.materials[1].color = Color.black;
                    break;
                case ColorPosition.FACE:
                    ren.materials[0].color = Color.black;
                    ren.materials[1].color = color;
                    break;
            }
        }
    }

    void FindChildBlocks()
    {
        if (this.transform.childCount > 0)
        {
            _internalBlocks = new Transform[this.transform.childCount];

            for (int i = 0; i < this.transform.childCount; i++)
            {
                _internalBlocks[i] = this.transform.GetChild(i);
            }
        }
    }
}
