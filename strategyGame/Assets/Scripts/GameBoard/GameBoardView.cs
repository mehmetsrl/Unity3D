using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public interface IGameBoardView
{
    event EventHandler<ClickedEventArgs> OnClicked;
    event EventHandler<ClickedEventArgs> OnClickedDown;
    event EventHandler<ClickedEventArgs> OnClickedUp;

    event EventHandler<EventArgs> Wandering;
    event EventHandler<MoveToCoordinatesEventArgs> MoveToCoordinatesOnPath;
    event EventHandler<MoveToCoordinatesEventArgs> MoveToCoordinates;
    event EventHandler<MovedEventArgs> OnMove;
    event EventHandler<ClickedEventArgs> OnDeselect;
    event EventHandler<ClickedEventArgs> OnPositionChanged;
    event EventHandler<InfoEventArgs> OnClickInFeaturePanel;

    Vector2 Position { set; }
    RectTransform RT { get; }
    void Init(Vector2 size,Vector2 coords);
    void Place(ClickedEventArgs e);
    void Destroy();
    void onClickInFeaturePanel(InfoEventAction action);
    void moveToCoordinates(Dimention2 targetCoord);
    void setRoute(List<Dimention2> route, float speed);
    bool isWandering { set; }
    bool isMovable { set; }
}


public class GameBoardView : MonoBehaviour, IGameBoardView
{
    public event EventHandler<ClickedEventArgs> OnClicked = (sender, e) => { };
    public event EventHandler<ClickedEventArgs> OnClickedDown = (sender, e) => { };
    public event EventHandler<ClickedEventArgs> OnClickedUp = (sender, e) => { };
    public event EventHandler<ClickedEventArgs> OnPositionChanged = (sender, e) => { };

    public event EventHandler<EventArgs> Wandering;
    public event EventHandler<MoveToCoordinatesEventArgs> MoveToCoordinates= (sender, e) => { };
    public event EventHandler<MoveToCoordinatesEventArgs> MoveToCoordinatesOnPath = (sender, e) => { };
    public event EventHandler<ClickedEventArgs> OnDeselect = (sender, e) => { };
    public event EventHandler<MovedEventArgs> OnMove = (sender, e) => { };
    public event EventHandler<InfoEventArgs> OnClickInFeaturePanel = (sender, e) => { };

    public Vector2 Position { set { rt.anchoredPosition = value; var eventArgs = new ClickedEventArgs(rt.anchoredPosition); OnPositionChanged(this, eventArgs); } }

    public Transform ParentTransform { set { transform.SetParent(value, false); } }

    bool hasWanderingMovement=true;
    bool movible = false;
    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
    public RectTransform RT{get{return rt;}}

    public bool isWandering
    {
        set
        {
            hasWanderingMovement = value;
        }
    }
    public bool isMovable
    {
        set
        {
            movible = value;
        }
    }

    void Start()
    {
        StartCoroutine(ProcessMove());
    }

    List<Dimention2> route = new List<Dimention2>();
    float speed = 1f;
    public void setRoute(List<Dimention2> route, float speed)
    {
        this.route = route;
        this.speed = speed;
    }

    IEnumerator ProcessMove()
    {
        while (movible)
        {
            //Debug.Log("route"+route.Count);
            while (route.Count > 0)
            {
                yield return new WaitForSeconds(1 / speed);
                var eventArgs = new MoveToCoordinatesEventArgs(route[0]);
                MoveToCoordinatesOnPath(this, eventArgs);
                route.RemoveAt(0);
            }
            yield return null;
        }
    }




    bool grabbed = false;

    private void Update()
    {
        if (movible&&hasWanderingMovement && !(route.Count > 0))
        {
            Wandering(this, new EventArgs());
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            grabbed = false;
            var eventArgs = new ClickedEventArgs(Input.mousePosition);
            OnDeselect(this, eventArgs);
        }

        if (grabbed)
        {
            var eventArgs = new MovedEventArgs(Input.mousePosition);
            OnMove(this, eventArgs);
        }
    }

    public void Init(Vector2 size,Vector2 position){
        rt.anchoredPosition = position;
        rt.sizeDelta=size;
        grabbed = true;
    }
    public void Place(ClickedEventArgs e)
    {
        rt.position = e.position;
        onClickUp();
    }

    public void onClick()
    {
        var eventArgs = new ClickedEventArgs(Input.mousePosition);
        OnClicked(this, eventArgs);
    }
    public void onClickDown()
    {
        //grabbed = true;
        var eventArgs = new ClickedEventArgs(Input.mousePosition);
        OnClickedDown(this, eventArgs);
    }
    public void onClickUp()
    {
        grabbed = false;
        var eventArgs = new ClickedEventArgs(Input.mousePosition);
        OnClickedUp(this, eventArgs);
    }

    public void onClickInFeaturePanel(InfoEventAction action)
    {

        var eventArgs = new InfoEventArgs(action);
        OnClickInFeaturePanel(this, eventArgs);
    }

    public void moveToCoordinates(Dimention2 targetCoords)
    {
        var eventArgs = new MoveToCoordinatesEventArgs(targetCoords);
        MoveToCoordinates(this, eventArgs);
    }




    public void Destroy(){

        //TODO clear tiles and set free grid location of its tiles
        Destroy(gameObject);
    }
}