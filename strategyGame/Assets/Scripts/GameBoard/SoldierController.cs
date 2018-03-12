using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public interface ISoldierController
{
    void SetParent(Transform transform);
    void SpawnSoldier(object sender, SpawnEventArgs e);
    void MoveSoldierItem(object sender, MovedEventArgs e);
    void Destroy();
}



public class SoldierController : ISoldierController
{
    private readonly IModel model;
    private readonly IGameBoardView view;
    InfoControllerFactory controllerFactory;
    PathFinder pathFinder;
    List<Dimention2> route;

    public SoldierController(IModel model, IGameBoardView view)
    {
        this.model = model;
        this.view = view;
        route = new List<Dimention2>();

        view.OnClicked += HandleClicked;
        view.OnClickedUp += HandleClickedUp;
        view.OnClickedDown += HandleClickedDown;
        view.OnMove += HandleOnMove;
        view.OnClickInFeaturePanel += HandleClickInFeaturePanel;
        view.OnPositionChanged += HandlePositionChanged;
        view.MoveToCoordinates += HandleMoveToCoords;
        view.MoveToCoordinatesOnPath += HandleMoveToCoordsOnPath;
        view.Wandering += HandleWandering;

        model.OnCoordinatesChanged += HandleCoordinatesChanged;
        model.OnParentChanged += HandleParentChanged;
        model.OnTilesChanged += HandleTilesChanged;

    }

    private void HandleClicked(object sender, ClickedEventArgs e)
    {

        // Create the controller
        controllerFactory = new InfoControllerFactory(model, InfoPanel.instance.infoView);

        controllerFactory.InfoController.ShowInfo(view, e);
    }
    private void HandleClickedDown(object sender, ClickedEventArgs e)
    {
    }
    private void HandleClickedUp(object sender, ClickedEventArgs e)
    {
        if(Input.GetMouseButtonUp(0)){
            GameBoard.instance.setSelectedGameBoardView(view);
        }
    }
    private void HandleOnMove(object sender, MovedEventArgs e)
    {


    }

    public void HandleClickInFeaturePanel(object sender, InfoEventArgs e)
    {
        switch(e.action){
            case InfoEventAction.destroy:
                Destroy();
                break;
        }
    }

    private void HandlePositionChanged(object sender, ClickedEventArgs e)
    {
    }
    private void HandleCoordinatesChanged(object sender, CoordsChangedEventArgs e)
    {
        Vector2 position = GameBoard.GetPositionFromCoords(model.Coords, GameBoard.instance.grid);
        view.Position = position;
    }

    private void HandleMoveToCoords(object sender, MoveToCoordinatesEventArgs e)
    {

        var searchParameters = new SearchParameters(model.Coords, e.coords,GameBoard.instance.grid);
        pathFinder=new PathFinder(searchParameters);
        route.Clear();
        route = pathFinder.FindPath();
        view.setRoute(route,model.Speed);
    }

    private void HandleMoveToCoordsOnPath(object sender, MoveToCoordinatesEventArgs e)
    {
        GameBoard.instance.grid.tiles[model.Coords.x, model.Coords.y].IsWalkable = true;
        model.Coords = e.coords;
        GameBoard.instance.grid.tiles[model.Coords.x, model.Coords.y].IsWalkable = false;
    }

    private void HandleWandering(object sender, EventArgs e){
//        Debug.Log("wandering");
        IEnumerable<Dimention2> adjacentCoords = PathFinder.GetAdjacentLocations(model.Coords);

        var random = new System.Random();
        var shuffledCoords = adjacentCoords.OrderBy(i => random.Next()).ToList();

        foreach(Dimention2 coord in shuffledCoords){
            if(coord.x>-1&&coord.x<GameBoard.instance.grid.gridSize.x && coord.y > -1 && coord.y < GameBoard.instance.grid.gridSize.y)
            if(GameBoard.instance.grid.tiles[coord.x, coord.y]!=null)
                if(GameBoard.instance.grid.tiles[coord.x,coord.y].IsWalkable)
                    view.moveToCoordinates(coord);
        }
    }

    private void HandleParentChanged(object sender, ParentChangedEventArgs e)
    {
        
    }
    private void HandleTilesChanged(object sender, TilesChangedEventArgs e)
    {
        foreach (Tile t in e.tiles)
            t.rt.transform.parent = view.RT.transform;
    }

    public void SpawnSoldier(object sender, SpawnEventArgs e)
    {

        Tile[,] tiles = new Tile[model.Size.x, model.Size.y];
        for (int i = 0; i < model.Size.x; i++)
        {
            for (int j = 0; j < model.Size.y; j++)
            {
                //GameObject go = new GameObject(view.RT.name+"Tile"+i+j,typeof(RectTransform), typeof(RawImage));
                GameObject go = UnityEngine.Object.Instantiate(GameBoard.instance.buildingTile, GameBoard.instance.playground.transform);
                go.tag = "Tile";

                go.GetComponent<Image>().color = model.Color;
                //go.GetComponent<RawImage>().material=
                tiles[i, j] = new Tile(go.GetComponent<RectTransform>(), model.Size);
            }
        }
        model.Tiles = tiles;
        model.Coords = e.position;
        GameBoard.instance.grid.tiles[e.position.x, e.position.y].IsWalkable = false;
        var totalBuildingSize = new Vector2(model.Size.x * GameBoard.instance.grid.tileSize.x * GameBoard.instance.playground.rt.rect.width,
                                            model.Size.y * GameBoard.instance.grid.tileSize.y * GameBoard.instance.playground.rt.rect.height);
        

        view.RT.sizeDelta = totalBuildingSize;
        view.RT.transform.parent = GameBoard.instance.rt.transform;
        //view.RT.GetComponent<RawImage>().texture = model.CoverImage;

        //view.Position= GameBoard.GetPositionFromCoords(model.Coords, GameBoard.instance.grid);

        Vector2 position = GameBoard.GetPositionFromCoords(model.Coords, GameBoard.instance.grid);
        view.Position = position;
        view.isWandering = true;
        view.isMovable = true;
        placeTiles();
        placeCoverImage();

    }


    void updateGridTileCoords(Tile[,] tilesOnGrid)
    {

        Vector2 relativePos = (model.Tiles[0, 0].rt.position - GameBoard.instance.grid.tiles[0, 0].rt.position);
        Dimention2 pivotCoord00 = new Dimention2 ( (int)Mathf.Round(relativePos.x / GameBoard.instance.tileSize.x), (int)Mathf.Round(relativePos.y / GameBoard.instance.tileSize.y) );
        //Debug.Log(pivotCoord00);

        foreach (Tile t in tilesOnGrid)
            t.coord = new Dimention2 { x = -1, y = -1 };

        if (pivotCoord00.x < 0 || pivotCoord00.y < 0 || (pivotCoord00.x + tilesOnGrid.GetLength(0)) > GameBoard.instance.grid.gridSize.x || (pivotCoord00.y + tilesOnGrid.GetLength(1)) > GameBoard.instance.grid.gridSize.y)
            return;

        for (int i = 0; i < tilesOnGrid.GetLength(0); i++)
        {
            for (int j = 0; j < tilesOnGrid.GetLength(1); j++)
            {
                tilesOnGrid[i, j].coord = new Dimention2 { x = (pivotCoord00.x + i), y = (pivotCoord00.y + j) };
            }
        }
    }

    void placeTiles()
    {
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

    void placeCoverImage()
    {
        GameObject coverImg = new GameObject("CoverImage", typeof(RectTransform), typeof(RawImage));

        RectTransform coverImgRt = coverImg.GetComponent<RectTransform>();
        coverImgRt.transform.parent = view.RT;
        coverImgRt.anchorMin = Vector2.zero;
        coverImgRt.anchorMax = Vector2.one;

        coverImgRt.offsetMin = Vector2.zero;
        coverImgRt.offsetMax = Vector2.zero;
        coverImgRt.sizeDelta = Vector2.zero;
        coverImgRt.GetComponent<RawImage>().texture = model.CoverImage;

        foreach (Transform child in view.RT.transform)
        {
            if (child.tag == "Tile")
                child.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/borderedTile");
        }
    }


    public void PlaceSoldier(object sender, ClickedEventArgs e)
    {
        //view.Place(e);
        bool suitableToPlace = GameBoard.instance.grid.checkTilesArePlacable(model.Tiles);
        if (suitableToPlace)
        {
            Vector2 snapDifference = Vector2.zero;
            GameBoard.instance.grid.snapToGrid(model.Tiles, ref snapDifference);
            view.RT.anchoredPosition += snapDifference;
        }
        else
            Destroy();

    }
    public void MoveSoldierItem(object sender, MovedEventArgs e)
    {
        //view.Position = e.position;

        updateGridTileCoords(model.Tiles);

        //bool suitableToPlace = Soldier.instance.grid.checkTilesArePlacable(view.Tiles);
    }


    public void SetParent(Transform transform)
    {
        //model.ParentTransform = transform;
    }

    bool haveIntersection(Rect rect)
    {
        return false;

    }

    public void Destroy()
    {

        view.OnClicked -= HandleClicked;
        view.OnClickedUp -= HandleClickedUp;
        view.OnClickedDown -= HandleClickedDown;
        view.OnMove -= HandleOnMove;
        view.OnPositionChanged += HandlePositionChanged;
        view.MoveToCoordinates -= HandleMoveToCoords;
        view.MoveToCoordinatesOnPath -= HandleMoveToCoordsOnPath;
        view.Wandering -= HandleWandering;

        model.OnCoordinatesChanged += HandleCoordinatesChanged;
        model.OnParentChanged -= HandleParentChanged;
        model.OnTilesChanged -= HandleTilesChanged;

        view.Destroy();
        controllerFactory.InfoController.Destroy();
        GameBoard.instance.grid.tiles[model.Coords.x,model.Coords.y].IsWalkable = true;
    }

}