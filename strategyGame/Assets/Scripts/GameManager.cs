using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionChangedEventArgs : EventArgs
{
}
public class ParentChangedEventArgs : EventArgs
{
}
public class CoordsChangedEventArgs : EventArgs
{
    private Dimention2 coords;

    public CoordsChangedEventArgs(Dimention2 coords)
    {
        this.coords = coords;
    }
}
public class SpawnEventArgs : EventArgs
{
    public Dimention2 position;

    public SpawnEventArgs(Dimention2 position)
    {
        this.position = position;
    }
}
public class TilesChangedEventArgs : EventArgs
{
    public Tile[,] tiles;

    public TilesChangedEventArgs(Tile[,] tiles)
    {
        this.tiles = tiles;
    }
}

public class MoveToCoordinatesEventArgs : EventArgs
{
    public Dimention2 coords;

    public MoveToCoordinatesEventArgs(Dimention2 coords)
    {
        this.coords = coords;
    }
}

public class ClickedEventArgs : EventArgs
{
    public Vector2 position;

    public ClickedEventArgs(Vector2 position)
    {
        this.position = position;
    }
}
public class MovedEventArgs : EventArgs
{
    public Vector2 position;

    public MovedEventArgs(Vector2 position)
    {
        this.position = position;
    }
}

public enum InfoEventAction{spawnSoldier,closePanel,destroy,somethingElse}
public class InfoEventArgs : EventArgs
{
    public InfoEventAction action;

    public InfoEventArgs(InfoEventAction action)
    {
        this.action = action;
    }

}


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    //public InfiniteScroll InScrl;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    void Start(){

        InitGame();
    }

    public int numberOfBuilding = 10;
    modelType type = 0;
    List<modelType> modelsInPanel = new List<modelType>() { modelType.Barrack, modelType.PowerPlant };
    void InitGame()
    {
        
        for (int i = 0; i < numberOfBuilding;)
        {
            if (modelsInPanel.Contains(type))
            {
                // Create the barrack model
                var modelFactory = new ModelFactory(type);
                var model = modelFactory.Model;

                // Create the view
                var panelViewFactory = new PanelViewFactory();
                var view = panelViewFactory.View;

                // Create the controller
                var controllerFactory = new PanelControllerFactory(model, view);

                var controller = controllerFactory.PanelController;

                controller.PlaceItem();

                i++;
            }

            type += 1;
            type = (modelType)((int)type % (int)modelType.Count);

        }
    }

	// Update is called once per frame
	void Update () {
		
	}


}
