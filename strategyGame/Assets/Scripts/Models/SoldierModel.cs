using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoldierModel : IModel
{
    private modelType type;
    private Dimention2 coords;
    private Dimention2 size;
    private Color color;
    private Texture image;
    private Info info;
    public InfoEventAction[] actions;
    Tile[,] tiles;
    private Transform parentTransform;

    public Dimention2 Coords { get { return coords; } set {  coords = value; var eventArgs = new CoordsChangedEventArgs(coords); OnCoordinatesChanged(this,eventArgs); } }
    public Dimention2 Size { get { return size; } }
    public Color Color { get { return color; } set { color = value; } }
    public Info Info { get { return info; } set { info = value; } }
    public InfoEventAction[] Actions { get { return actions; } }


    public event EventHandler<CoordsChangedEventArgs> OnCoordinatesChanged;
    public event EventHandler<ParentChangedEventArgs> OnParentChanged;
    public event EventHandler<TilesChangedEventArgs> OnTilesChanged;


    public Texture coverImage = null;
    public Texture CoverImage { get { return coverImage; } }

    float movementSpeed = 2f;

    public float Speed { get { return movementSpeed; } }

    public Tile[,] Tiles
    {
        set
        {
            if (value != null)
            {
                tiles = value;
                foreach (Tile t in tiles)
                {
                    var eventArgs = new TilesChangedEventArgs(tiles);
                    OnTilesChanged(this, eventArgs);
                }
            }
        }
        get { return tiles; }
    }

    public modelType Type
    {
        get { return type; }
    }

    public Transform ParentTransform
    {
        get { return parentTransform; }
        set
        {
            if (parentTransform != value)
            {
                parentTransform = value;
                var eventArgs = new ParentChangedEventArgs();
                OnParentChanged(this, eventArgs);
            }
        }
    }

    public SoldierModel(){
        type = modelType.Soldier;
        size = new Dimention2 (1,1);
        color = Color.cyan;
        coverImage=Resources.Load<Texture>("Images/soldier1");
        image = coverImage;
        info = new Info(type.ToString(), image,"Orders");
        actions = new InfoEventAction[] { InfoEventAction.destroy };
        tiles = new Tile[Size.x, Size.y];

    }

}
