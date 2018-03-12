using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public interface IInfoController
{
    void SetParent(Transform transform);
    void ShowInfo(object sender, ClickedEventArgs e);
    void HideInfo(object sender, ClickedEventArgs e);
    void Destroy();
    void HandleClickInFeaturePanel(object sender, InfoEventArgs e);
}


enum models{soldier,building}
public class InfoController : IInfoController
{
    private models modelType;
    private readonly IModel model;
    private readonly IInfoView view;

    
    private List<InfoEventAction> actions=new List<InfoEventAction>();

    private object grabbedObj;

    public InfoController(IModel model, IInfoView view)
    {
        this.model = model;
        modelType = models.building;
        this.view = view;
        view.OnClickInFeaturePanel+=HandleClickInFeaturePanel;
    }

    private void setActionButtons(IGameBoardView targetGameboardView){
        Vector2 anchor = new Vector2(0,1);


        foreach(InfoEventAction a in actions){

            GameObject btnGO = new GameObject("ActionButton_"+a, typeof(RectTransform), typeof(Image), typeof(Button));
            Text btnText = new GameObject("Text" , typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            btnText.text=a.ToString();
            btnText.font=Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            btnText.color = Color.black;
            btnText.alignment = TextAnchor.MiddleCenter;


            btnText.transform.parent = btnGO.transform;

            RectTransform btnTextRt=btnText.GetComponent<RectTransform>();

            btnTextRt.anchorMin = Vector2.zero;
            btnTextRt.anchorMax = Vector2.one;

            btnTextRt.offsetMin = Vector2.zero;
            btnTextRt.offsetMax = Vector2.zero;
            btnTextRt.sizeDelta = Vector2.zero;


            btnGO.transform.parent = view.FeaturePanelButtons.transform;

            Button btn = btnGO.GetComponent<Button>();
            btn.onClick.AddListener(delegate { view.onClickInFeaturePanel(a); targetGameboardView.onClickInFeaturePanel(a); });

            RectTransform btnRT =btnGO.GetComponent<RectTransform>();

            anchor -= new Vector2(0, view.ActionButtonSize.y);

            btnRT.anchorMin = anchor;
            anchor += view.ActionButtonSize;
            btnRT.anchorMax = anchor;
            anchor -= new Vector2(view.ActionButtonSize.x, 0);

            btnRT.offsetMin = Vector2.zero;
            btnRT.offsetMax = Vector2.zero;
            btnRT.sizeDelta = Vector2.zero;

            anchor = new Vector2(anchor.x, 1);
            anchor += new Vector2(view.ActionButtonSize.x, 0);
            if (anchor.x+view.ActionButtonSize.x > 1)
            {
                anchor = new Vector2(0, anchor.y-view.ActionButtonSize.y);
            }
        }
    }


    public void HandleClickInFeaturePanel(object sender, InfoEventArgs e)
    {
        
    }

    public void ShowInfo(object sender, ClickedEventArgs e){
        view.RT.transform.parent = InfoPanel.instance.viewPort.transform;


        view.Clear();
        actions  = model.Actions.ToList();
        view.Init(model.Info,(actions.Count>0), e);


        if((sender as IGameBoardView)!=null)
        setActionButtons(sender as IGameBoardView);
    }


    public void HideInfo(object sender, ClickedEventArgs e)
    {
        Destroy();
    }


    public void SetParent(Transform transform)
    {
        model.ParentTransform = transform;
    }

    bool haveIntersection(Rect rect)
    {
        return false;

    }

    public void Destroy(){
        view.Clear();
    }
}