using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {
    public static InfoPanel instance;
    public RectTransform viewPort;
    [HideInInspector]
    public RectTransform rt;

    public IInfoView infoView;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        rt = GetComponent<RectTransform>();

        // Create the view
        var infoViewFactory = new InfoViewFactory();
        infoView = infoViewFactory.View;
    }
}
