
using UnityEngine;

public class PowerPlants : BuildingModel {
    Dimention2 size = new Dimention2 (2,3);
    public PowerPlants()
    {
        type = modelType.PowerPlant;
        color = Color.yellow;
        buildingSize = size;
        coverImage = Resources.Load<Texture>("Images/plant2");
        image = Resources.Load<Texture>("Images/plant1");
        info = new Info(type.ToString(), image);
        actions = new InfoEventAction[] {};
        base.InitBuildingModel();
    }
}
