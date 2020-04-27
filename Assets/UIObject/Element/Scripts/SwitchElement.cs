using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;

public class SwitchElement : MonoBehaviour
{
    [Header("屬性的個數")]
    [SerializeField]
    int ElementCount = 4;
    
    [SerializeField]
    Image _nowElementSprite;

    [Header("各屬性的圖片")]
    [SerializeField]
    Sprite _wind;

    [SerializeField]
    Sprite _spark;

    [SerializeField]
    Sprite _fire;

    [SerializeField]
    Sprite _water;

    [SerializeField]
    Sprite _ground;
    
    ElementData _windData;
    ElementData _fireData;
    ElementData _waterData;
    ElementData _groundData;
    ElementData _sparkData;

    //new Element = (上, 下, 左, 右)
    ElementDataSet _windElement;
    ElementDataSet _fireElement;
    ElementDataSet _waterElement;
    ElementDataSet _groundElement;
    ElementDataSet _sparkElement;

    //切換屬性
    public ElementSet SwitchElementType(ElementSet nowElement, int dir)
    {
        ElementData newElementData = _fireData; //預設火
        
        //1 風, 2 雷, 3 火, 4 水, 5 土
        if (nowElement == ElementSet.Wind)
        {
            newElementData = _windElement.SwitchElementType(dir);
        }
        else if (nowElement == ElementSet.Spark)
        {
            newElementData = _sparkElement.SwitchElementType(dir);
        }
        else if (nowElement == ElementSet.Fire)
        {
            newElementData = _fireElement.SwitchElementType(dir);
        }
        else if (nowElement == ElementSet.Water)
        {
            newElementData = _waterElement.SwitchElementType(dir);
        }
        else if (nowElement == ElementSet.Ground)
        {
            newElementData = _groundElement.SwitchElementType(dir);
        }

        _nowElementSprite.sprite = newElementData.Sprite;
        return newElementData.Element;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region 創建屬性 - 5屬性
        if (ElementCount == 5)
        {
            #region 創建屬性Data
            _windData = new ElementData(_wind, ElementSet.Wind);
            _sparkData = new ElementData(_spark, ElementSet.Spark);
            _fireData = new ElementData(_fire, ElementSet.Fire);
            _waterData = new ElementData(_water, ElementSet.Water);
            _groundData = new ElementData(_ground, ElementSet.Ground);
            #endregion

            #region 創建個別屬性
            _windElement = new ElementDataSet(_windData.Element, _sparkData, _fireData, _waterData, _groundData);
            _sparkElement = new ElementDataSet(_sparkData.Element, _windData, _fireData, _waterData, _groundData);
            _fireElement = new ElementDataSet(_fireData.Element, _windData, _sparkData, _waterData, _groundData);
            _waterElement = new ElementDataSet(_waterData.Element, _windData, _sparkData, _fireData, _groundData);
            _groundElement = new ElementDataSet(_groundData.Element, _windData, _sparkData, _fireData, _waterData);
            #endregion
        }
        #endregion

        #region 創建屬性 - 4屬性
        if (ElementCount == 4)
        {
            #region 創建屬性Data
            _windData = new ElementData(_wind, ElementSet.Wind); //無用 

            _sparkData = new ElementData(_spark, ElementSet.Spark);
            _fireData = new ElementData(_fire, ElementSet.Fire);
            _waterData = new ElementData(_water, ElementSet.Water);
            _groundData = new ElementData(_ground, ElementSet.Ground);
            #endregion

            #region 創建個別屬性
            _windElement = new ElementDataSet(_windData.Element, _sparkData, _fireData, _waterData, _groundData); //無用

            _sparkElement = new ElementDataSet(_sparkData.Element, _sparkData, _groundData, _waterData, _fireData);
            _fireElement = new ElementDataSet(_fireData.Element, _sparkData, _groundData, _waterData, _fireData);
            _waterElement = new ElementDataSet(_waterData.Element, _sparkData, _groundData, _waterData, _fireData);
            _groundElement = new ElementDataSet(_groundData.Element, _sparkData, _groundData, _waterData, _fireData);
            #endregion
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
    }
}
