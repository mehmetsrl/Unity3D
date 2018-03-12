using UnityEngine;
using System.Collections;

public class SumoMoves: MonoBehaviour {
//Variables	
	private float currentRotationSpeed; // karakterin dönme hızı
	public Vector2 size,offset;			// sprite hareket değişkenleri
	
	public float   colums=2;			//sprite kolondaki karakter sayısı
	public float  rows=1;				//sprite satırdaki karakter sayısı
	
	public Texture yuru,dur;			//1. sprite yürüme ve durma textureları
	public Texture yuru2,dur2;			//2. sprite yürüme ve durma textureları

	public float fps=10f;				//karakter hareket hızı
	
	public bool right=false;				// input var mı yok mu 
	public bool left=false;
	
	public float energyLimit=50;
	
	public float energyCurrent1=0;
	public float energyCurrent2=0;
	
	
	
	
	
	void Start () {
		
		
	}
	
	
	
void Update () {
		
		
	if(Input.touchCount==0){// eğer dokunma yoksa 
			right=false;		// inputlar değer almayacaklar 
			left=false;
		}
		
	if(Input.touchCount==1){// eğer dokunma 1 tane ise 
	   if (Input.GetTouch(0).phase ==TouchPhase.Stationary || Input.GetTouch(0).phase ==TouchPhase.Moved){ // parmak hareketimiz varsa veya dokunur pozisyonda haeketsizse...
		
			
			if(Input.touches[0].position.x>10 && Input.touches[0].position.x<(Screen.width/2-20) && Input.touches[0].position.y<(Screen.height-10) && Input.touches[0].position.y>10)// dokunmanın istediğimiz bölgede olup oolmadığını kontrol eder
			{
				right=true;
				left=false;
			}
			
			if(Input.touches[0].position.x<Screen.width-10 && Input.touches[0].position.x>(Screen.width/2+10) && Input.touches[0].position.y<(Screen.height-10) && Input.touches[0].position.y>10)// dokunmanın istediğimiz bölgede olup oolmadığını kontrol eder
			{
				left=true;
				right=false;
			}
			
			
		}
	}
	else if(Input.touchCount==2){ // eğer ekrandaki dokunma sayısı 2 tane ise 
		
		if ((Input.GetTouch(0).phase ==TouchPhase.Stationary || Input.GetTouch(0).phase ==TouchPhase.Moved)&&(Input.GetTouch(1).phase ==TouchPhase.Stationary || Input.GetTouch(1).phase ==TouchPhase.Moved)){ // her iki parmağı da kontrol eder
		
			
			if(Input.touches[0].position.x>10 && Input.touches[0].position.x<(Screen.width/2-20) && Input.touches[0].position.y<(Screen.height-10) && Input.touches[0].position.y>10)// dokunmanın istediğimiz bölgede olup oolmadığını kontrol eder
			{
				right=true;
				left=false;
			if(Input.touches[1].position.x<Screen.width-10 && Input.touches[1].position.x>(Screen.width/2+10) && Input.touches[1].position.y<(Screen.height-10) && Input.touches[1].position.y>10)// dokunmanın istediğimiz bölgede olup oolmadığını kontrol eder
			{
				left=true;
				
			}
			}else if(Input.touches[0].position.x<Screen.width-10 && Input.touches[0].position.x>(Screen.width/2+10) && Input.touches[0].position.y<(Screen.height-10) && Input.touches[0].position.y>10)// dokunmanın istediğimiz bölgede olup oolmadığını kontrol eder
			{
						
			
				left=true;
				right=false;
			if(Input.touches[1].position.x>10 && Input.touches[1].position.x<(Screen.width/2-20) && Input.touches[1].position.y<(Screen.height-10) && Input.touches[1].position.y>10)// dokunmanın istediğimiz bölgede olup oolmadığını kontrol eder
			{
				right=true;
				
			}
			}
			
			
			
			
			}
				
	 	}
		
		
		
		
		
		
	
		
	if(transform.name=="player1"){ // eğer üzerinde olduğu nesnenin adı player birse ve buton bas
		if(right==true||Input.GetKey(KeyCode.LeftControl))
		{
			fps=10f;
			currentRotationSpeed=0;
			transform.Translate(Vector3.down*(100+energyCurrent1/5)*Time.deltaTime);
			colums=2;
			rows=1;
			renderer.material.SetTexture("_MainTex",dur);
			if(energyCurrent1 > 0){energyCurrent1-=3;}
		}
		else if(right==false)
		{
			fps=4f;
			currentRotationSpeed=150;
			colums=4;
			rows=1;
			renderer.material.SetTexture("_MainTex",yuru);
			if(energyCurrent1<energyLimit){energyCurrent1++;}
		}
	}
		
	if(transform.name=="player2"){
		if(left==true||Input.GetKey(KeyCode.Space))
		{
			fps=10f;
			currentRotationSpeed=0;
			transform.Translate(Vector3.down*(100+energyCurrent2/5)*Time.deltaTime);
			colums=2;
			rows=1;
			renderer.material.SetTexture("_MainTex",dur2);
			if(energyCurrent2 > 0){energyCurrent2-=3;}
		} else if(left==false)
		{
			fps=4f;
			currentRotationSpeed=150;
			colums=4;
			rows=1;
			renderer.material.SetTexture("_MainTex",yuru2);
			if(energyCurrent2<energyLimit){energyCurrent2++;}
		}
	}
		
		
		
		float index = Time.time*fps;
		index=index%colums;
		index=System.Convert.ToInt32(index);
		size= new Vector2(1.0f/colums,1.0f/rows);
		float columIndex=index%colums;
		float rowIndex=index/rows;
		
		offset=new Vector2(columIndex*size.x,1.0f-size.y-rowIndex*size.y);
		renderer.material.SetTextureOffset("_MainTex",offset);
		renderer.material.SetTextureScale("_MainTex",size);
		
		
		
		
		float rotationalSpeed=currentRotationSpeed*Time.deltaTime;
		transform.Rotate(Vector3.up * rotationalSpeed,Space.World);
		
	}
  void OnTriggerExit(Collider other) {
        transform.position= new Vector3(0,0.5f,0);
    }

}

