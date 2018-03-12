using UnityEngine;
using System.Collections;
using System;

public class tasi2 : MonoBehaviour {
    Vector2 deltaPosition1;
    float xPos,yPos;
    float maxX= 2.8f;
    float maxY = 2.5f;
	// Use this for initialization
	void Start () {
        print(Screen.width);
        print(Screen.height);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount == 1){

           deltaPosition1 = Input.GetTouch(0).position;
          
           if (deltaPosition1.x > Screen.width / 2)
           {
               xPos = deltaPosition1.x - (Screen.width / 2);
               xPos = (((xPos * 100) / Screen.width)*maxX)/100;
           }
           else if (deltaPosition1.x < Screen.width / 2)
           {
               xPos = (Screen.width / 2) - deltaPosition1.x;
               xPos = (((xPos * 100) / Screen.width) * maxX) / 100;
           }

          
           
            
           if (deltaPosition1.y > Screen.height/ 2)
           {
               yPos = deltaPosition1.x - (Screen.height / 2);
               yPos =(((yPos * 100) / Screen.height)*maxY)/100;
           }
           else if (deltaPosition1.x < Screen.height / 2)
           {
               yPos = (Screen.height / 2) - deltaPosition1.x;
               yPos = (((yPos * 100) / Screen.height) * maxY) / 100;
           }

           transform.position = new Vector3(xPos, 0, transform.position.z);

           
           

        }

	}

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 20), "x:" + Convert.ToString(deltaPosition1.x));
        GUI.Box(new Rect(10, 40, 100, 20), "y:" + Convert.ToString(deltaPosition1.y));
    }

}
