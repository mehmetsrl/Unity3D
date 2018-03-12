using UnityEngine;
using System.Collections;

public class touch : MonoBehaviour {
	public GameObject but1;
	public GameObject but2;
	bool b1,b2;
	Ray ray;
	RaycastHit rayCastHit;
	// Use this for initialization
	void Start () {
	Input.multiTouchEnabled=enabled;
	gameObject.renderer.material.color = Color.red;
		b1=false;
		b2=false;
	}
	
	// Update is called once per frame
	void Update () {

		
		
		
		
		
		
		
		
		if(Input.touchCount==1){
		
		 if (Input.GetTouch(0).phase ==TouchPhase.Began){
			
			
				ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
			  
				
			
				if(Physics.Raycast(ray,out rayCastHit))
				{
					if(rayCastHit.transform.name=="button1"){
					  b1=true;
					  rayCastHit.transform.renderer.material.color = Color.white;}
					if(rayCastHit.transform.name=="button2"){
					  b2=true;
					  rayCastHit.transform.renderer.material.color = Color.black;}
				}
				
			
			}
			
			if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				if(b1==true){
				but1.transform.renderer.material.color = Color.red;
				b1=false;
				}
				if(b2==true){
				but2.transform.renderer.material.color = Color.red;
				b2=false;
				}
			}
		
		}
		
		if(Input.touchCount==2){
		if (Input.GetTouch(1).phase == TouchPhase.Began){
			
			
				ray = Camera.main.ScreenPointToRay(Input.touches[1].position);
			  
				
			
				if(Physics.Raycast(ray,out rayCastHit))
				{
					if(rayCastHit.transform.name=="button1"){
						 b1=true;
					  rayCastHit.transform.renderer.material.color = Color.white;}
					if(rayCastHit.transform.name=="button2"){
						 b2=true;
					  rayCastHit.transform.renderer.material.color = Color.black;}
				}
				
				
		
			}
		if(Input.GetTouch(1).phase == TouchPhase.Ended)
			{
				if(b1==true){
				but1.transform.renderer.material.color = Color.red;
				b1=false;
				}
				if(b2==true){
				but2.transform.renderer.material.color = Color.red;
				b2=false;
				}
			}
		}}
		

		}

	
	
	

