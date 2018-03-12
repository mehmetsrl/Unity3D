using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI(){
		
		
		GUI.Box(new Rect(Screen.width*.35f,Screen.height*.2f,Screen.width*.3f,Screen.height*.1f),"Single Player");
		
		GUI.Box(new Rect(Screen.width*.35f,Screen.height*.5f,Screen.width*.3f,Screen.height*.1f),"Tutorial");
		GUI.Box(new Rect(Screen.width*.35f,Screen.height*.65f,Screen.width*.3f,Screen.height*.1f),"Credits");
		
		
		if(GUI.Button(new Rect(Screen.width*.35f,Screen.height*.35f,Screen.width*.3f,Screen.height*.1f),"2 Player")){
			
			Application.LoadLevel(1);
			
			
		}
	}
	
}
