using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class InfiniteScroll : MonoBehaviour {


    public RectTransform[] slots;
    public ScrollRect sv;
    public RectTransform secondContent;
    bool isSwaping = false;

    void Awake(){
        sv = GetComponent<ScrollRect>();
        //secondContent.anchoredPosition = sv.content.anchoredPosition + new Vector2(0, sv.content.rect.height*-1);
        onScroll();
        slots = new RectTransform[sv.content.transform.childCount];
        for (int i = 0; i < sv.content.transform.childCount; i++)
            slots[i] = sv.content.transform.GetChild(i).GetComponent<RectTransform>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (sv.content.anchoredPosition.y < 0)
            isSwaping = true;

        /*
        if (Input.GetMouseButtonUp(0))
            if (isSwaping)
                swapContents();
        */
	}

    public void onScroll(){
        if(!isSwaping)
            secondContent.position = (Vector2)sv.content.position + new Vector2(0, (sv.content.rect.height + sv.content.GetComponent<LayoutGroup>().padding.bottom)* -1);
    }

    public void swapContents(){
        //secondContent.anchoredPosition = sv.content.anchoredPosition + new Vector2(0, (sv.content.rect.height + sv.content.GetComponent<LayoutGroup>().padding.bottom));
        RectTransform tempContent = sv.content;
        sv.content = secondContent;
        secondContent = tempContent;
        //sv.content.anchoredPosition = new Vector2(sv.content.anchoredPosition.x, sv.content.rect.height);
        //Debug.Log(new Vector2(sv.content.position.x, sv.content.rect.height));
        sv.content.position=new Vector2(sv.content.position.x, sv.content.position.y+sv.content.rect.height);
        //isSwaping = false;

    }
}
