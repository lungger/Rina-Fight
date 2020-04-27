using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

class ElementData
{
    public Sprite Sprite
    { get; }

    public ElementSet Element
    { get; }


    public ElementData(Sprite sprite, ElementSet element)
    {
        Sprite = sprite;
        Element = element;
    }
}

class ElementDataSet
{
    //各方向的屬性
    ElementData _up;
    ElementData _down;
    ElementData _left;
    ElementData _right;

    //Enum
    public ElementSet NowElement;

    public ElementDataSet(ElementSet nowElement, ElementData up, ElementData down, ElementData left, ElementData right)
    {
        NowElement = nowElement;
        _up = up;
        _down = down;
        _left = left;
        _right = right;
    }

    public ElementData SwitchElementType(string dir)
    {
        ElementData newElement = _up;
        if (dir == "up")
            newElement = _up;
        else if (dir == "down")
            newElement = _down;
        else if (dir == "left")
            newElement = _left;
        else if (dir == "right")
            newElement = _right;

        return newElement;
    }

    /// <summary> 1 上, 2 下, 3 左, 4 右 </summary>
    public ElementData SwitchElementType(int dir)
    {
        ElementData newElement = _up;
        if (dir == 1)
            newElement = _up;
        else if (dir == 2)
            newElement = _down;
        else if (dir == 3)
            newElement = _left;
        else if (dir == 4)
            newElement = _right;

        return newElement;
    }
}
