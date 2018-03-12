using UnityEngine;
using System.Collections;

public class tasi : MonoBehaviour {
  public float _horizontaLimit = 3f, _verticalLimit = 2f, dragSpeed = 0.1f;
  Ray ray;
  RaycastHit rayCastHit;
  bool a;

    Transform cachedTransform;
    Vector3 startingPos;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            Vector2 deltaPosition1 = Input.GetTouch(0).deltaPosition;

            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    break;
                case TouchPhase.Moved:
                    DragObject(deltaPosition1);
                    break;

                case TouchPhase.Ended:
                   
                    a = false;
                    break;

            }

        }
	}


    void DragObject(Vector2 deltaPosition)
    {
        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        
        if(Physics.Raycast(ray,out rayCastHit))
        {
            if (a == false)
            {
                cachedTransform = rayCastHit.transform;
                startingPos =cachedTransform.position;
                a = true;
            }

                rayCastHit.transform.position = new Vector3(Mathf.Clamp((deltaPosition.x * dragSpeed) + cachedTransform.position.x, startingPos.x - _horizontaLimit, startingPos.x + _horizontaLimit),
                                               cachedTransform.position.y, Mathf.Clamp((deltaPosition.y * dragSpeed) + cachedTransform.position.y, startingPos.y - _verticalLimit, startingPos.y + _verticalLimit)
                                                );
            }
        
        

    }

}
