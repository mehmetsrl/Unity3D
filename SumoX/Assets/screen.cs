using UnityEngine;
using System.Collections;

public class screen : MonoBehaviour {
	public Texture buton1;
	public Texture buton2;
	public bool a=false;
	public bool b=false;
	// Use this for initialization
	void Start () {
	print("x:" + Screen.width);
	print("y:" + Screen.height);
	
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.touchCount==1){
	   if (Input.GetTouch(0).phase ==TouchPhase.Stationary || Input.GetTouch(0).phase ==TouchPhase.Moved){
		
			
			if(Input.touches[0].position.x>10 && Input.touches[0].position.x<110 && Input.touches[0].position.y<Screen.height/2+50 && Input.touches[0].position.y>Screen.height/2-50)
			{
				print("btn 1");
					a=true;
			}else 
					a=false;
			
			if(Input.touches[0].position.x>Screen.width-110 && Input.touches[0].position.x<Screen.width-10 && Input.touches[0].position.y<Screen.height/2+50 && Input.touches[0].position.y>Screen.height/2-50)
			{
				print("btn 2");
					b=true;
			}else 
					b=false;
			
			
		}}
	else if(Input.touchCount==2){
		
		if (Input.GetTouch(0).phase ==TouchPhase.Stationary || Input.GetTouch(0).phase ==TouchPhase.Moved){
		
			
			if(Input.touches[0].position.x>10 && Input.touches[0].position.x<110 && Input.touches[0].position.y<Screen.height/2+50 && Input.touches[0].position.y>Screen.height/2-50)
			{
				print("btn 1");
					a=true;
			}else 
					a=false;
			
			if(Input.touches[0].position.x>Screen.width-110 && Input.touches[0].position.x<Screen.width-10 && Input.touches[0].position.y<Screen.height/2+50 && Input.touches[0].position.y>Screen.height/2-50)
			{
				print("btn 2");
					b=true;
			}else 
					b=false;
			
			}	
				
		if (Input.GetTouch(1).phase ==TouchPhase.Stationary || Input.GetTouch(1).phase ==TouchPhase.Moved){
		
			
			if(Input.touches[1].position.x>10 && Input.touches[1].position.x<110 && Input.touches[1].position.y<Screen.height/2+50 && Input.touches[1].position.y>Screen.height/2-50)
			{
				print("btn 1");
					a=true;
			}else 
					a=false;
			
			if(Input.touches[1].position.x>Screen.width-110 && Input.touches[1].position.x<Screen.width-10 && Input.touches[1].position.y<Screen.height/2+50 && Input.touches[1].position.y>Screen.height/2-50)
			{
				print("btn 2");
					b=true;
			}else 
					b=false;
			
			
			}
				
	 	}
		
		if(Input.touchCount==2 && a==true && b==true)
		{
			print ("çift tıklı");
		}
	}
	
	}
