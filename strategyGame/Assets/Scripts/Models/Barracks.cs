
using UnityEngine;

public class Barracks : BuildingModel {
    Dimention2 size = new Dimention2 (4,4);
    public Barracks()
    {
        type = modelType.Barrack;
        color = Color.red;
        buildingSize = size;
        coverImage = Resources.Load<Texture>("Images/barrack");
        image = coverImage;
        info= new Info(type.ToString(),image,"Production");
        actions = new InfoEventAction[]{ InfoEventAction.spawnSoldier};
        base.InitBuildingModel();
    }
}
