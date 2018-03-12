using System;
using UnityEngine;

public interface IModelFactory
{
    IModel Model { get; }
}

public class ModelFactory : IModelFactory
{
    public IModel Model { get; private set; }

    public ModelFactory( modelType mType )
    {
        switch(mType){
            case modelType.Barrack:
                Model = new Barracks();
                break;
            case modelType.PowerPlant:
                Model = new PowerPlants();
                break;
            case modelType.Soldier:
                Model = new SoldierModel();
                break;
            default:
                //TODO handle
                break;
        }
    }

    public ModelFactory()
    {
    }
}

public interface IPanelViewFactory
{
    IPanelView View { get; }
}

public class PanelViewFactory : IPanelViewFactory
{
    public IPanelView View { get; private set; }
    public static int count = 0;

    public PanelViewFactory()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/buildingModelPanelPrefab");
        var instance = UnityEngine.Object.Instantiate(prefab);
        instance.name = "buildingModelPanelPrefab" + count++;
        View = instance.GetComponent<IPanelView>();
    }
}

public interface IPanelControllerFactory
{
    IPanelController PanelController { get; }
}

public class PanelControllerFactory : IPanelControllerFactory
{
    public IPanelController PanelController { get; private set; }

    public PanelControllerFactory(IModel model, IPanelView view)
    {
        PanelController = new PanelController(model, view);
    }

    public PanelControllerFactory() : this(new BuildingModel(), new PanelView())
    {
        
    }
}




public interface IGameBoardViewFactory
{
    IGameBoardView View { get; }
    void Destroy();
}

public class GameBoardViewFactory : IGameBoardViewFactory
{
    public IGameBoardView View { get; private set; }
    public static int count=0;
    public GameBoardViewFactory()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/buildingModelGameBoardPrefab");
        var instance = UnityEngine.Object.Instantiate(prefab);
        instance.name = "buildingModelGameBoardPrefab" + count++;
        View = instance.GetComponent<IGameBoardView>();
    }
    public void Destroy()
    {
        View.Destroy();
    }
}

public interface IGameBoardControllerFactory
{
    IGameBoardController GameBoardController { get; }
    void Destroy();
}

public class GameBoardControllerFactory : IGameBoardControllerFactory
{
    public IGameBoardController GameBoardController { get; private set; }

    public GameBoardControllerFactory(IModel model, IGameBoardView view)
    {
        GameBoardController = new GameBoardController(model, view);
    }

    public GameBoardControllerFactory() : this(new BuildingModel(), new GameBoardView())
    {

    }

    public void Destroy(){
        GameBoardController.Destroy();
    }


}







public interface IInfoViewFactory
{
    IInfoView View { get; }
}

public class InfoViewFactory : IInfoViewFactory
{
    public IInfoView View { get; private set; }
    public static int count = 0;

    public InfoViewFactory()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/InfoView");
        var instance = UnityEngine.Object.Instantiate(prefab);
        instance.name = "InfoView" + count++;
        View = instance.GetComponent<IInfoView>();
    }
}

public interface IInfoControllerFactory
{
    IInfoController InfoController { get; }
}

public class InfoControllerFactory : IInfoControllerFactory
{
    public IInfoController InfoController { get; private set; }

    public InfoControllerFactory(IModel model, IInfoView view)
    {
        InfoController = new InfoController(model, view);
    }

    public InfoControllerFactory() : this(new BuildingModel(), new InfoView())
    {

    }
}





public interface ISoldierViewFactory
{
    IGameBoardView View { get; }
}

public class SoldierViewFactory : ISoldierViewFactory
{
    public IGameBoardView View { get; private set; }
    public static int count = 0;

    public SoldierViewFactory()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/SoldierView");
        var instance = UnityEngine.Object.Instantiate(prefab);
        instance.name = "SoldierView" + count++;
        View = instance.GetComponent<IGameBoardView>();
    }
}

public interface ISoldierControllerFactory
{
    ISoldierController SoldierController { get; }
}

public class SoldierControllerFactory : ISoldierControllerFactory
{
    public ISoldierController SoldierController { get; private set; }

    public SoldierControllerFactory(IModel model, IGameBoardView view)
    {
        SoldierController = new SoldierController(model, view);
    }

    public SoldierControllerFactory() : this(new SoldierModel(), new GameBoardView())
    {

    }
}