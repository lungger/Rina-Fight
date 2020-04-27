using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOutline : MonoBehaviour
{
    Outline _outline;
    Color _initColor;

    public void SelectButton()
    {
        _outline.effectColor = _initColor;
    }

    public void CancelSelectButton()
    {
        _outline.effectColor = new Color(_initColor.r, _initColor.g, _initColor.b, 0);
    }

    // Start is called before the first frame update
    void Awake()
    {
        _outline = gameObject.GetComponent<Outline>();
        _initColor = _outline.effectColor;
        CancelSelectButton();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
