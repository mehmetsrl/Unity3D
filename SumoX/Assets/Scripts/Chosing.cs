using UnityEngine;
using System.Collections;

public class Chosing : MonoBehaviour {
	
	
	
	
	void Start(){
	
	
	}
	// Update is called once per frame
	void Update () {
		
	}
	
	
	
	
	
	public void OnTriggerEnter(Collider otherObject){
		
		if(otherObject.name=="Plane1"){
			
			renderer.material.SetTexture("_MainTex",otherObject.transform.renderer.material.mainTexture);
			
		}
		else if(otherObject.name=="Plane2"){
			
			
			
		}
		else if(otherObject.name=="Plane4"){
			
			
			
		}
		else{
			
			
			
		}
		
		
	}
	
	
	
}
