using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Dimention2
{
    public int x, y;
    public static Dimention2 invalid { get { return new Dimention2(-1, -1); } }

    public Dimention2(int x,int y){
        this.x = x;
        this.y = y;
    }

    public override string ToString(){
        return "x: " + x + "y: " + y;
    }
}

public enum TileState{Unasigned,Open,Closed};

[System.Serializable]
public class Tile
{
    private Tile parentTile;
    public Dimention2 coord;
    public RectTransform rt;

    public bool isPlacable = true;

    public bool IsWalkable { get; set; }

    public float G { get; private set; }

    public float H { get; private set; }

    public TileState State { get; set; }

    public float F
    {
        get { return this.G + this.H; }
    }

    public Tile ParentTile
    {
        get { return this.parentTile; }
        set
        {
            this.parentTile = value;
            if(this.parentTile!=null)
            this.G = this.parentTile.G + GetTraversalCost(this.coord, this.parentTile.coord);
        }
    }

    public Tile(RectTransform rt, Dimention2 coord){
        this.rt = rt;
        this.coord = coord;
        this.State = TileState.Unasigned;
        this.IsWalkable = true;
        this.G = 0;
    }

    public void CalculateH(Dimention2 endLocation){
        this.H = GetTraversalCost(coord, endLocation);
    }

    internal static float GetTraversalCost(Dimention2 location, Dimention2 otherLocation)
    {
        float deltaX = otherLocation.x - location.x;
        float deltaY = otherLocation.y - location.y;
        return (float)Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public override string ToString()
    { 
        return "  coord : " + coord+"  "+rt+ "  ";
    }
}

public class Grid{
    public Dimention2 gridSize;
    public GameObject tile;
    Playground playground;
    public Tile[,] tiles;

    public Vector2 tileSize = Vector2.zero;
    Vector2 anchor = Vector2.zero;

    public Grid(GameObject tile,Playground playground){
        this.playground = playground;
        this.tile = tile;
        tileSize = new Vector2(tile.GetComponent<RectTransform>().rect.width / GameBoard.instance.playground.rt.rect.width, 
                               tile.GetComponent<RectTransform>().rect.height / GameBoard.instance.playground.rt.rect.height);
        gridSize = new Dimention2 ((int)Mathf.Round(1 / tileSize.x), (int)Mathf.Round(1 / tileSize.y ));

        tiles = new Tile[gridSize.x, gridSize.y];
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Dimention2 coord = new Dimention2(i,j);
                tiles[i, j] = new Tile(UnityEngine.Object.Instantiate(tile, playground.transform).GetComponent<RectTransform>(), coord);
                tiles[i, j].rt.anchorMin = anchor;
                anchor += tileSize;
                tiles[i, j].rt.anchorMax = anchor;
                anchor -= new Vector2(tileSize.x, 0);
                tiles[i, j].rt.sizeDelta = Vector2.zero;
            }
            anchor = new Vector2(anchor.x, 0);
            anchor += new Vector2(tileSize.x, 0);
        }
    }

    public bool checkTilesArePlacable(Tile[,] buildingTiles)
    {
        bool isPlacableBuilding = true;

        for (int i = 0; i< buildingTiles.GetLength(0);i++)
            for (int j = 0; j < buildingTiles.GetLength(1);j++){
//                Debug.Log(i+"  "+j+" "+buildingTiles[i, j].coord);
                if(buildingTiles[i, j].coord.x < 0 || buildingTiles[i, j].coord.y < 0){

                    buildingTiles[i, j].isPlacable = false;
                    isPlacableBuilding = false;
                    continue;
                }

                if (!tiles[buildingTiles[i,j].coord.x, buildingTiles[i,j].coord.y].IsWalkable)
                {
                    buildingTiles[i, j].isPlacable = false;
                    isPlacableBuilding = false;
                }
                else buildingTiles[i, j].isPlacable = true;
            }
        return isPlacableBuilding;
    }

    public void snapToGrid(Tile[,] viewTiles, ref Vector2 positionDifference)
    {
        positionDifference= tiles[viewTiles[0, 0].coord.x, viewTiles[0, 0].coord.y].rt.position - viewTiles[0, 0].rt.position;
        for (int i = 0; i < viewTiles.GetLength(0); i++)
            for (int j = 0; j < viewTiles.GetLength(1); j++)
            {
                tiles[viewTiles[i, j].coord.x, viewTiles[i, j].coord.y].IsWalkable = false;
            }
    }
}


public class GameBoard : MonoBehaviour {
    public static GameBoard instance;
    //public Dimention2 gridSize;
    public GameObject tile,buildingTile;
    [HideInInspector]
    public Vector2 tileSize;
    public Playground playground;
    public Grid grid;

    [HideInInspector]
    public RectTransform rt;

    private IGameBoardView gbView;

    Vector2 anchor = Vector2.zero;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        rt = GetComponent<RectTransform>();
        tileSize = GameBoard.instance.tile.GetComponent<RectTransform>().sizeDelta;

        //grid = new Grid(gridSize, tile,playground);//Custom Size Grid
        grid = new Grid(tile, playground);
    }

    public static Dimention2 GetGridCoord(Tile[,] tile){
        if (tile == null) return Dimention2.invalid;
        return new Dimention2(tile[0,0].coord.x+tile.GetLength(0)/2,tile[0, 0].coord.y + tile.GetLength(1) / 2);
    }
    public static Dimention2 GetGridCoord(Vector2 position)
    {
        Vector3[] playgroundCorners = new Vector3[4];
        GameBoard.instance.playground.rt.GetWorldCorners(playgroundCorners);

        Vector2 playgroundPosMin = playgroundCorners[0];
        Vector2 playgroundPosMax = playgroundCorners[2];
        //Rect playgroundRect = new Rect(playgroundPosMin,(playgroundPosMax - playgroundPosMin));

        if (position.x < playgroundPosMin.x || position.y < playgroundPosMin.y ||
            position.x > playgroundPosMax.x || position.y > playgroundPosMax.y) 
            return Dimention2.invalid;
        Vector2 relativePos = (position-playgroundPosMin);

        return new Dimention2((int)(relativePos.x/GameBoard.instance.tileSize.x),(int)(relativePos.y / GameBoard.instance.tileSize.y));
    }

    public static Vector2 GetPositionFromCoords(Dimention2 coords,Grid grid){

        if (coords.x < 0 || coords.y < 0 || coords.x > grid.gridSize.x || coords.y > grid.gridSize.y) return Vector2.negativeInfinity;

        //return new Vector2(coords.x * grid.tileSize.x, coords.y * grid.tileSize.y);
        return new Vector2(coords.x * grid.tile.GetComponent<RectTransform>().rect.width+grid.tile.GetComponent<RectTransform>().rect.width/2
                           , coords.y * grid.tile.GetComponent<RectTransform>().rect.height+grid.tile.GetComponent<RectTransform>().rect.height/2);
        //return new Vector2(coords.x * grid.tileSize.x * Screen.width, coords.y * grid.tileSize.y * Screen.height);
    }


    public void setSelectedGameBoardView(IGameBoardView view){
        gbView = view;
    }
    public void onClickUp(){
        if(gbView!=null)
            gbView.moveToCoordinates(GetGridCoord(Input.mousePosition));
    }
}
