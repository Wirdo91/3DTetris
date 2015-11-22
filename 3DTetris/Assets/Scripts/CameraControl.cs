using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	float XAngle = 0;
	float YAngle = 0;

	//float XAngleMove = 15.0f;
	//float YAngleMove = 15.0f;

    Touch input;

    //TODO Make 2d available
    //Orthographic camera
    //Remove movement on Z axis
    //Remove rotation on X & Y axis
    //Layer is full, only in one column set
	void Start ()
    {
	}

    public void MoveX(float angle)
    {
        XAngle += angle;
    }
    public void MoveY(float angle)
    {
        YAngle += angle;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved)
            {
                XAngle += input.deltaPosition.x;
                YAngle -= input.deltaPosition.y;
            }

            XAngle = XAngle > 360 ? XAngle - 360 : XAngle;
            XAngle = XAngle < 0 ? XAngle + 360 : XAngle;

            YAngle = Mathf.Clamp(YAngle, 0, 90);

            this.transform.eulerAngles = new Vector3(YAngle, XAngle, 0);
        }
    }
    /* Old GUI
    void OnGUI()
	{
		//Up
		if (GUI.Button (new Rect (Screen.width - 5 - 25 - 25, Screen.height - 5 - 25 - 25 -25, 25, 25), "|")) {
			YAngle += YAngleMove;
		}
		//Right
		if (GUI.Button (new Rect (Screen.width - 5 - 25, Screen.height - 5 - 25 - 25, 25, 25), "-")) {
			XAngle -= XAngleMove;
		}
		//Down
		if (GUI.Button (new Rect (Screen.width - 5 - 25 - 25, Screen.height - 5 - 25, 25, 25), "|")) {
			YAngle -= YAngleMove;
		}
		//Left
		if (GUI.Button (new Rect (Screen.width - 5 - 25 - 25 -25, Screen.height - 5 - 25 - 25, 25, 25), "-")) {
			XAngle += XAngleMove;
		}
		//Up & Right
		if (GUI.Button (new Rect (Screen.width - 5 - 25, Screen.height - 5 - 25 - 25 -25, 25, 25), "/")) {
			XAngle -= XAngleMove;
			YAngle += YAngleMove;
		}
		//Down & Right
		if (GUI.Button (new Rect (Screen.width - 5 - 25, Screen.height - 5 - 25, 25, 25), "\\")) {
			XAngle -= XAngleMove;
			YAngle -= YAngleMove;
		}
		//Down & Left
		if (GUI.Button (new Rect (Screen.width - 5 - 25 - 25 - 25, Screen.height - 5 - 25, 25, 25), "/")) {
			XAngle += XAngleMove;
			YAngle -= YAngleMove;
		}
		//Up & Left
		if (GUI.Button (new Rect (Screen.width - 5 - 25 -25 -25, Screen.height - 5 - 25 -25 -25, 25, 25), "\\")) {
			XAngle += XAngleMove;
			YAngle += YAngleMove;
		}

		XAngle = XAngle > 360 ? XAngle - 360 : XAngle;
		XAngle = XAngle < 0 ? XAngle + 360 : XAngle;

		YAngle = Mathf.Clamp (YAngle, 0, 90);

		this.transform.eulerAngles = new Vector3 (YAngle, XAngle, 0);
	}*/
}
