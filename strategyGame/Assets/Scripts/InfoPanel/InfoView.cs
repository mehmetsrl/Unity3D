using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public interface IInfoView
{
    event EventHandler<InfoEventArgs> OnClickInFeaturePanel;

    Vector3 Position { set; }
    Transform ParentTransform { set; }
    RectTransform RT { get; }
    RectTransform FeaturePanelButtons { get; }
    Vector2 ActionButtonSize { get; }
    void Init(Info info,bool hasAction, ClickedEventArgs e);
    void Destroy();
    void onClickInFeaturePanel(InfoEventAction action);
    void Clear();
}


public class InfoView : MonoBehaviour, IInfoView
{


    public event EventHandler<InfoEventArgs> OnClickInFeaturePanel = (sender, e) => { };

    public Vector3 Position { set { transform.position = value; } }

    public Transform ParentTransform { set { transform.SetParent(value, false); } }

    public Text header, featureHeader;
    public RawImage image;
    Texture initialImageTexture;
    public RectTransform featurePanelButtons;
    public Vector2 actionButtonSize;

    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        initialImageTexture = image.texture;
    }
    public RectTransform RT
    {
        get
        {
            return rt;
        }
    }

    public Vector2 ActionButtonSize{
        get { return new Vector2(actionButtonSize.x,actionButtonSize.y); }
    }

    public RectTransform FeaturePanelButtons{
        get { return featurePanelButtons; }
    }


    private void Update()
    {
    }

    public void Init(Info info,bool hasAction, ClickedEventArgs e)
    {
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.sizeDelta = Vector2.zero;

        header.text = info.name;
        image.texture = info.infoImage;

        if (hasAction)
            featureHeader.text = info.featureTypeName;
        else
            featureHeader.text = null;
            

    }

    public void Clear()
    {
        foreach (Transform child in FeaturePanelButtons.transform)
        {
            Destroy(child.gameObject);
        }
        header.text = null;
        image.texture = initialImageTexture;
        featureHeader.text = null;
    }

    public void onClickInFeaturePanel(InfoEventAction action){

        var eventArgs = new InfoEventArgs(action);
        OnClickInFeaturePanel(this,eventArgs);
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
}