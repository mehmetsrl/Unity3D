using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public interface IPanelController
{
    void PlaceItem();
    //void SetParent(Transform transform);
}



public class PanelController : IPanelController
{
    private readonly IModel model;
    private readonly IPanelView view;

    private IGameBoardController gameBoardController;


    private object grabbedObj;

    public PanelController(IModel model, IPanelView view)
    {
        this.model = model;
        this.view = view;

        view.RT.gameObject.GetComponent<RawImage>().color = model.Color;

        view.OnClicked += HandleClicked;
        view.OnClickedUp += HandleClickedUp;
        view.OnClickedDown += HandleClickedDown;
        view.OnMove += HandleOnMove;

        model.OnParentChanged += HandleParentChanged;

    }

    private void HandleClicked(object sender, ClickedEventArgs e)
    {
        
    }
    private void HandleClickedDown(object sender, ClickedEventArgs e)
    {

        grabbedObj = sender;

        // Create the barrack model
        var modelFactory = new ModelFactory(model.Type);
        var gameBoardModel = modelFactory.Model;

        // Create the view
        var gameBoardViewFactory = new GameBoardViewFactory();
        var gameBoardView = gameBoardViewFactory.View;

        // Create the controller
        var controllerFactory = new GameBoardControllerFactory(gameBoardModel, gameBoardView);
        gameBoardController = controllerFactory.GameBoardController;

        gameBoardController.InitGameBoardItem(this,e);


    }
    private void HandleClickedUp(object sender, ClickedEventArgs e)
    {
        if (gameBoardController != null)
        {
            gameBoardController.PlaceGameBoardItem(this, e);
            gameBoardController = null;
        }
    }
    private void HandleOnMove(object sender, MovedEventArgs e)
    {
        if (gameBoardController != null)
        {
            gameBoardController.MoveGameBoardItem(this, e);
        }
    }

    private void HandlePositionChanged(object sender, PositionChangedEventArgs e)
    {
        
    }

    private void HandleParentChanged(object sender, ParentChangedEventArgs e)
    {
        SyncParent();
    }

    private void SyncParent()
    {
        view.ParentTransform = model.ParentTransform;
    }

    public void PlaceItem(){
        ProductionPanel.instance.Place(this);
    }

    public void SetParent(Transform transform)
    {
        model.ParentTransform = transform;
    }

    bool haveIntersection(Rect rect)
    {
        return false;

    }
}