using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public interface IGameBoardController
{
    void SetPosition(Vector3 position);
    void SetParent(Transform transform);
    void InitGameBoardItem(object sender,ClickedEventArgs e);
    void PlaceGameBoardItem(object sender, ClickedEventArgs e);
    void MoveGameBoardItem(object sender, MovedEventArgs e);
    void Destroy();
}



public class GameBoardController : IGameBoardController
{
    private readonly IModel model;
    private readonly IGameBoardView view;


    public GameBoardController(IModel model, IGameBoardView view)
    {
        this.model = model;
        this.view = view;


        view.OnClicked += HandleClicked;
        view.OnClickInFeaturePanel+=HandleClickInFeaturePanel;
        view.OnPositionChanged += HandlePositionChanged;

        model.OnCoordinatesChanged += HandleCoordinatesChanged;
        model.OnParentChanged += HandleParentChanged;


        model.OnTilesChanged += HandleTilesChanged;

    }

    private void HandleClicked(object sender, ClickedEventArgs e)
    {



        // Create the controller
        var controllerFactory = new InfoControllerFactory(model, InfoPanel.instance.infoView);
        //InfoPanel.instance.infoController = controllerFactory.InfoController;
        //InfoPanel.instance.infoController.ShowInfo(view, e);
        controllerFactory.InfoController.ShowInfo(view, e);
    }

    public void HandleClickInFeaturePanel(object sender, InfoEventArgs e)
    {
        if(e.action==InfoEventAction.spawnSoldier){

            // Create the soldier model
            var modelFactory = new ModelFactory(modelType.Soldier);
            var soldierModel = modelFactory.Model;

            // Create the view
            var soldierViewFactory = new SoldierViewFactory();
            var soldierView = soldierViewFactory.View;

            // Create the controller
            var controllerFactory = new SoldierControllerFactory(soldierModel, soldierView);


            var eventArgs = new SpawnEventArgs(getAvailableCoordAround());
            controllerFactory.SoldierController.SpawnSoldier(this,eventArgs);
        }
    }

    Dimention2 getAvailableCoordAround()
    {
        Dimention2 coord00 = model.Tiles[0, 0].coord;
        Dimention2 targetCoord;

        for (int rad = 1; rad < Mathf.Max(GameBoard.instance.grid.gridSize.x - model.Size.x, GameBoard.instance.grid.gridSize.y - model.Size.y); rad++)
            for (int i = -1 * rad; i < model.Size.x + rad; i++)
                for (int j = -1 * rad; j < model.Size.y + rad; j++)
                    if (i == -1 * rad || i == model.Size.x + rad - 1 || j == -1 * rad || j == model.Size.y + rad - 1)
                    {
                        targetCoord = new Dimention2(coord00.x + i, coord00.y + j);
                        if (targetCoord.x > -1 && targetCoord.x < GameBoard.instance.grid.gridSize.x && targetCoord.y > -1 && targetCoord.y < GameBoard.instance.grid.gridSize.y)
                            if (GameBoard.instance.grid.tiles[targetCoord.x, targetCoord.y] != null)
                                if (GameBoard.instance.grid.tiles[targetCoord.x, targetCoord.y].IsWalkable) return targetCoord;
                    }
        return Dimention2.invalid;
    }


    private void HandleCoordinatesChanged(object sender, CoordsChangedEventArgs e)
    {
        bool suitableToPlace = GameBoard.instance.grid.checkTilesArePlacable(model.Tiles);
        if (suitableToPlace)
            foreach (Transform child in view.RT.transform)
            {
            if(child.tag=="Tile")
                    child.GetComponent<Image>().color = model.Color;
            }
        else
            foreach (Transform child in view.RT.transform)
            {
                if(child.tag == "Tile")
                    child.GetComponent<Image>().color = Color.grey;
            }
    }

    private void HandlePositionChanged (object sender, ClickedEventArgs e){
        updateGridTileCoords();
        //bool suitableToPlace = GameBoard.instance.grid.checkTilesArePlacable(model.Tiles);
    }

    private void HandleTilesChanged(object sender,TilesChangedEventArgs e){
        foreach (Tile t in e.tiles)
            t.rt.transform.parent = view.RT.transform;
    }


    private void HandleParentChanged(object sender, ParentChangedEventArgs e)
    {
        SyncParent();
    }

    private void SyncParent()
    {
//        view.ParentTransform = model.ParentTransform;
    }

    public void InitGameBoardItem(object sender,ClickedEventArgs e){
        Tile[,] tiles = new Tile[model.Size.x,model.Size.y];
        for (int i = 0; i < model.Size.x; i++){
            for (int j = 0; j < model.Size.y; j++)
            {
                GameObject go = UnityEngine.Object.Instantiate(GameBoard.instance.buildingTile, GameBoard.instance.playground.transform);
                go.tag = "Tile";
                go.GetComponent<Image>().color=model.Color;
                tiles[i, j] = new Tile(go.GetComponent<RectTransform>(), model.Size);
            }
        }
        model.Tiles = tiles;
        var totalSize = new Vector2(model.Size.x * GameBoard.instance.grid.tileSize.x * GameBoard.instance.playground.rt.rect.width,
                                            model.Size.y * GameBoard.instance.grid.tileSize.y * GameBoard.instance.playground.rt.rect.height);
        
        //view.RT.GetComponent<RawImage>().texture=model.CoverImage;
        view.RT.sizeDelta = totalSize;
        view.RT.transform.parent = GameBoard.instance.rt.transform;
        view.isMovable = false;

        placeTiles();

        placeCoverImage();
    }


    void updateGridTileCoords(){
        Tile[,] tilesOnGrid = model.Tiles;
        Vector2 relativePos = (tilesOnGrid[0, 0].rt.position - GameBoard.instance.grid.tiles[0, 0].rt.position);
        Dimention2 pivotCoord00 = new Dimention2((int)Mathf.Round(relativePos.x / GameBoard.instance.tileSize.x),(int)Mathf.Round(relativePos.y / GameBoard.instance.tileSize.y));
        //Debug.Log(pivotCoord00);

        foreach (Tile t in tilesOnGrid)
            t.coord = Dimention2.invalid;

        if (pivotCoord00.x < 0 || pivotCoord00.y < 0 || (pivotCoord00.x + tilesOnGrid.GetLength(0))>GameBoard.instance.grid.gridSize.x|| (pivotCoord00.y + tilesOnGrid.GetLength(1)) > GameBoard.instance.grid.gridSize.y)
            return;
        for (int i = 0; i < tilesOnGrid.GetLength(0);i++){
            for (int j = 0; j < tilesOnGrid.GetLength(1);j++){
                tilesOnGrid[i, j].coord = new Dimention2( (pivotCoord00.x + i), (pivotCoord00.y + j));
            }
        }
        model.Tiles= tilesOnGrid;
        model.Coords = GameBoard.GetGridCoord(model.Tiles);
    }

    void placeTiles(){
        Vector2 anchor = Vector2.zero;
        Vector2 tileSize = new Vector2(GameBoard.instance.grid.tileSize.x * GameBoard.instance.playground.rt.rect.width / view.RT.sizeDelta.x,
                                       GameBoard.instance.grid.tileSize.y * GameBoard.instance.playground.rt.rect.height / view.RT.sizeDelta.y);
        for (int i = 0; i < model.Size.x; i++)
        {
            for (int j = 0; j < model.Size.y; j++)
            {
                model.Tiles[i, j].rt.anchorMin = anchor;
                anchor += tileSize;
                model.Tiles[i, j].rt.anchorMax = anchor;
                anchor -= new Vector2(tileSize.x, 0);

                model.Tiles[i, j].rt.sizeDelta = Vector2.zero;
                model.Tiles[i, j].rt.offsetMin = Vector2.zero;
                model.Tiles[i, j].rt.offsetMax = Vector2.zero;
            }

            anchor = new Vector2(anchor.x, 0);
            anchor += new Vector2(tileSize.x, 0);
        }
    }

    void placeCoverImage(){
        GameObject coverImg = new GameObject("CoverImage" , typeof(RectTransform), typeof(RawImage));

        RectTransform coverImgRt = coverImg.GetComponent<RectTransform>();
        coverImgRt.transform.parent = view.RT;
        coverImgRt.anchorMin = Vector2.zero;
        coverImgRt.anchorMax = Vector2.one;

        coverImgRt.offsetMin = Vector2.zero;
        coverImgRt.offsetMax = Vector2.zero;
        coverImgRt.sizeDelta = Vector2.zero;
        coverImgRt.GetComponent<RawImage>().texture=model.CoverImage;
    }

    public void PlaceGameBoardItem(object sender, ClickedEventArgs e)
    { 
        //Debug.Log(model.Coords);
        bool suitableToPlace = GameBoard.instance.grid.checkTilesArePlacable(model.Tiles);

        if (suitableToPlace)
        {
            Vector2 snapDifference = Vector2.zero;
            GameBoard.instance.grid.snapToGrid(model.Tiles,ref snapDifference);
            view.RT.anchoredPosition += snapDifference;
            foreach (Transform child in view.RT.transform)
            {
                if (child.tag == "Tile")
                    child.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/borderedTile");
            }
        }
        else
            Destroy();
    }


    public void MoveGameBoardItem(object sender, MovedEventArgs e)
    {
        view.Position = e.position-new Vector2(GameBoard.instance.rt.anchorMin.x*Screen.width,0);
    }


    public void SetPosition(Vector3 position)
    {
        view.Position = position;
    }

    public void SetParent(Transform transform)
    {
        model.ParentTransform = transform;
    }

    bool haveIntersection(Rect rect)
    {
        return false;

    }

    public void Destroy(){

        view.OnClicked -= HandleClicked;
        view.OnPositionChanged -= HandlePositionChanged;

        model.OnCoordinatesChanged -= HandleCoordinatesChanged;
        model.OnParentChanged -= HandleParentChanged;
        model.OnTilesChanged -= HandleTilesChanged;

        view.Destroy();
    }
}