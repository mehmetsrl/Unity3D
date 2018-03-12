using UnityEngine;
using System.Collections;

public class BitmapAnimation : MonoBehaviour {
	
	public Vector2 size,offset;	
	private float currentRotationSpeed;
	
	public float rows=1;
	public float colums=2;
	public float fps=10f;
	public Texture dur;
	
	// Update is called once per frame
	void Update () {
	
		float index = Time.time*fps;
		index=index%colums;
		index=System.Convert.ToInt32(index);
		size= new Vector2(1.0f/colums,1.0f/rows);
		float columIndex=index%colums;
		float rowIndex=index/rows;
		
		offset=new Vector2(columIndex*size.x,1.0f-size.y-rowIndex*size.y);
		renderer.material.SetTextureOffset("_MainTex",offset);
		renderer.material.SetTextureScale("_MainTex",size);
		
		renderer.material.SetTexture("_MainTex",dur);
		
	}
}
