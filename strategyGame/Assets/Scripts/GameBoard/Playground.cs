using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour {
    public RectTransform rt;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
}
