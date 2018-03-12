using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionPanel : MonoBehaviour {



    class PoolObject
    {
        internal static int count = 0;
        internal int ID;
        internal PanelController controller;

        internal PoolObject(PanelController controller)
        {

            this.controller = controller;
            ID = count;
            count++;
        }

    }

    List<PoolObject> poolObjs;
    int poolObjBaseIndex = PoolObject.count;
    int poolObjCurrentIndex = PoolObject.count;



    public static ProductionPanel instance = null;
    public InfiniteScroll InScrl;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        poolObjs = new List<PoolObject>();
    }

    PoolObject getPolObjByID(int id)
    {
        foreach (PoolObject po in poolObjs)
            if (po.ID == id) return po;

        return null;
    }

    private void Update()
    {
    }

    void updateDisplay(){

        //Debug.Log(PoolObject.count+"  "+poolObjBaseIndex);
        for (int i = poolObjBaseIndex; i < PoolObject.count; i++)
        {
            if (i - poolObjBaseIndex < InScrl.slots.Length)
            {
                poolObjs[i].controller.SetParent(InScrl.slots[i]);
            }

        }
    }

    public void onExitFromTop(RectTransform rt){

        rt.SetAsLastSibling();
    }

    public void onScroll(){
        Debug.Log(InScrl.sv.verticalScrollbar.value);
    }

    public void Place(PanelController controller)
    {
        poolObjs.Add(new PoolObject(controller));
        updateDisplay();

    }



}
