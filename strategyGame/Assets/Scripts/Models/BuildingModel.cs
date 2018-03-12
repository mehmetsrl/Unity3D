using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.Events;


public interface IModel
{
    event EventHandler<CoordsChangedEventArgs> OnCoordinatesChanged;
    event EventHandler<ParentChangedEventArgs> OnParentChanged;
    event EventHandler<TilesChangedEventArgs> OnTilesChanged;

    Dimention2 Coords { get; set; }
    modelType Type { get; }
    Dimention2 Size { get; }
    Transform ParentTransform { get; set; }
    Color Color { get; set; }
    Info Info { get; set; }
    InfoEventAction[] Actions { get; }
    Tile[,] Tiles { get; set; }
    Texture CoverImage { get; }
    float Speed { get; }
}


public class Info
{
    public string name;
    public Texture infoImage;
    public string featureTypeName;
    //public InfoFeature infoFeat;

    public Info(string name, Texture infoImage, string featureTypeName)
    {
        this.name = name;
        this.infoImage = infoImage;
        this.featureTypeName = featureTypeName;
    }
    public Info(string name, Texture infoImage)
    {
        this.name = name;
        this.infoImage = infoImage;
        this.featureTypeName = "Features";
    }
}

public enum modelType { Barrack, PowerPlant, Soldier, Count }

public class BuildingModel : IModel
{

    public Info info;

    public string header = "";


    public string imgPath;

    public Texture image = null;
    public Texture coverImage = null;
    public Texture CoverImage{ get { return coverImage; }}

    public modelType type;

    public Color color;

    public InfoEventAction[] actions;

    protected Dimention2 buildingSize;
    private Dimention2 coords;
    private Transform parentTransform;
    public Tile[,] tiles;
    float movementSpeed = 2f;

    public event EventHandler<CoordsChangedEventArgs> OnCoordinatesChanged = (sender, e) => { };
    public event EventHandler<ParentChangedEventArgs> OnParentChanged = (sender, e) => { };
    public event EventHandler<TilesChangedEventArgs> OnTilesChanged = (sender, e) => { };

    public BuildingModel(){
        tiles = new Tile[Size.x, Size.y];
    }

    public float Speed { get { return movementSpeed; } }

    public InfoEventAction[] Actions
    {
        get { return actions; }
    }
    public Tile[,] Tiles
    {
        set
        {
            if (value != null&&tiles!=null)
            {
                tiles = value;
                var eventArgs = new TilesChangedEventArgs(tiles);
                OnTilesChanged(this, eventArgs);
            }
        }
        get { return tiles; }
    }

    public modelType Type
    {
        get { return type; }
    }

    public Dimention2 Size
    {
        get { return buildingSize; }
    }

    public Dimention2 Coords
    {
        get { return coords; }
        set
        {
            coords = value;
            var eventArgs = new CoordsChangedEventArgs(coords);
            OnCoordinatesChanged(this, eventArgs);
        }
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

    public Color Color
    {
        get { return color; }
        set
        {
            color = value;
        }
    }

    public Info Info
    {
        get { return info; }
        set
        {
            if (info != value)
                info = value;
        }
    }

    public void InitBuildingModel()
    {
        if (color == null) color = Color.gray;
        image = Resources.Load(imgPath) as Texture;
    }

}
