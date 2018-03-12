using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;


public class SearchParameters
{
    public Dimention2 StartLocation { get; set; }

    public Dimention2 EndLocation { get; set; }

    public Grid Grid{ get; set; }


    public SearchParameters(Dimention2 startLocation, Dimention2 endLocation, Grid grid)
    {
        this.StartLocation = startLocation;
        this.EndLocation = endLocation;
        this.Grid = grid;
    }
}


public class PathFinder
{
    private int width;
    private int height;
    private Tile[,] tiles;
    private Tile startTile;
    private Tile endTile;
    private SearchParameters searchParameters;

    public PathFinder(SearchParameters searchParameters)
    {
        this.searchParameters = searchParameters;
        InitializeTiles(searchParameters.Grid);
        this.startTile = this.tiles[searchParameters.StartLocation.x, searchParameters.StartLocation.y];
        this.startTile.State = TileState.Open;
        this.endTile = this.tiles[searchParameters.EndLocation.x, searchParameters.EndLocation.y];

        foreach(Tile t in searchParameters.Grid.tiles){
            t.ParentTile = null;
            t.State = TileState.Unasigned;
        }
    }

    public List<Dimention2> FindPath()
    {
        List<Dimention2> path = new List<Dimention2>();
        bool success = Search(startTile);
        if (success)
        {
            Tile tile = this.endTile;
            while (tile.ParentTile != null)
            {
                path.Add(tile.coord);
                tile = tile.ParentTile;
            }

            path.Reverse();
        }
        return path;
    }


    private void InitializeTiles(Grid grid)
    {
        this.width = grid.gridSize.x;
        this.height = grid.gridSize.y;
        this.tiles = grid.tiles;

    }

    private bool Search(Tile currentTile)
    {
        currentTile.State = TileState.Closed;
        List<Tile> nextTiles = GetAdjacentWalkableTiles(currentTile);

        nextTiles.Sort((tile1, tile2) => tile1.F.CompareTo(tile2.F));
        foreach (var nextTile in nextTiles)
        {
            if (nextTile.coord.x == this.endTile.coord.x&&nextTile.coord.y == this.endTile.coord.y)
            {
                return true;
            }
            else
            {
                if (Search(nextTile))
                    return true;
            }
        }

        return false;
    }

    private List<Tile> GetAdjacentWalkableTiles(Tile fromTile)
    {
        List<Tile> walkableTiles = new List<Tile>();
        IEnumerable<Dimention2> nextLocations = GetAdjacentLocations(fromTile.coord);

        foreach (var location in nextLocations)
        {
            int x = location.x;
            int y = location.y;

            if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                continue;

            Tile tile = this.tiles[x, y];
            tile.CalculateH(endTile.coord);

            if (!tile.IsWalkable)
                continue;
            
            if (tile.State == TileState.Closed)
                continue;
            
            if (tile.State == TileState.Open)
            {
                float traversalCost = Tile.GetTraversalCost(tile.coord, tile.ParentTile.coord);

                float gTemp = fromTile.G + traversalCost;
                if (gTemp < tile.G)
                {
                    tile.ParentTile = fromTile;
                    walkableTiles.Add(tile);
                }
            }
            else
            {
                tile.ParentTile = fromTile;
                tile.State = TileState.Open;
                walkableTiles.Add(tile);
            }
        }

        return walkableTiles;
    }

    public static IEnumerable<Dimention2> GetAdjacentLocations(Dimention2 fromLocation)
    {
        return new Dimention2[]
        {
            new Dimention2(fromLocation.x-1, fromLocation.y-1),
            new Dimention2(fromLocation.x-1, fromLocation.y  ),
            new Dimention2(fromLocation.x-1, fromLocation.y+1),
            new Dimention2(fromLocation.x,   fromLocation.y+1),
            new Dimention2(fromLocation.x+1, fromLocation.y+1),
            new Dimention2(fromLocation.x+1, fromLocation.y  ),
            new Dimention2(fromLocation.x+1, fromLocation.y-1),
            new Dimention2(fromLocation.x,   fromLocation.y-1)
        };
    }
}