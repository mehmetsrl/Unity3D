using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPanelView
{
    event EventHandler<ClickedEventArgs> OnClicked;
    event EventHandler<ClickedEventArgs> OnClickedDown;
    event EventHandler<ClickedEventArgs> OnClickedUp;
    event EventHandler<MovedEventArgs> OnMove;

	Vector3 Position { set; }
    Transform ParentTransform { set; }
    RectTransform RT { get; }
}


public class PanelView : MonoBehaviour, IPanelView
{
    public event EventHandler<ClickedEventArgs> OnClicked = (sender, e) => { };
    public event EventHandler<ClickedEventArgs> OnClickedDown = (sender, e) => { };
    public event EventHandler<ClickedEventArgs> OnClickedUp = (sender, e) => { };
    public event EventHandler<MovedEventArgs> OnMove = (sender, e) => { };

	public Vector3 Position { set { transform.position = value; } }

    public Transform ParentTransform { set { transform.SetParent(value, false); } }

    RectTransform rt;

    void Awake(){
        rt = GetComponent<RectTransform>();

    }
    public RectTransform RT{
        get{
            return rt;
        }
    }

    bool grabbed = false;

    private void Update()
    {
        if(grabbed){
            var eventArgs = new MovedEventArgs(Input.mousePosition);
            OnMove(this, eventArgs);
        }
    }


    public void onClick()
    {
        var eventArgs = new ClickedEventArgs(Input.mousePosition);
		OnClicked(this, eventArgs);
	}
    public void onClickDown()
    {
        grabbed = true;
        var eventArgs = new ClickedEventArgs(Input.mousePosition);
        OnClickedDown(this, eventArgs);
    }
    public void onClickUp()
    {
        grabbed = false;
        var eventArgs = new ClickedEventArgs(Input.mousePosition);
        OnClickedUp(this, eventArgs);
    }
}