using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScreen : MonoBehaviour
{
    [Header("文字本體")]
    public Text MasterText;
    
    RectTransform _canvas_collider;

    RectTransform _textRect;

    [Header("飛行速度")]
    public float Speed = 300.0f;

    #region Setter
    public void SetSpeed(float minSpeed, float maxSpeed)
    {
        Speed = Random.Range(minSpeed, maxSpeed);
    }

    //直接指定Text
    public void SetText(string text)
    {
        MasterText.text = text;
    }

    //從彈幕資料庫中隨機選擇
    public void SetText(List<string> text_database)
    {
        int setTextIndex = Random.Range(0, text_database.Count);
        MasterText.text = text_database[setTextIndex];
    }

    //直接指定Font Size
    public void SetFontSize(int size)
    {
        MasterText.fontSize = size;
    }

    //從範圍中隨機選擇
    public void SetFontSize(int minSize, int maxSize)
    {
        MasterText.fontSize = Random.Range(minSize, maxSize);
    }

    //直接指定顏色
    public void SetColor(Color color)
    {
        MasterText.color = color;
    }

    //從顏色庫中隨機選擇
    public void SetColor(List<Color> color_database)
    {
        int setIndex = Random.Range(0, color_database.Count);
        MasterText.color = color_database[setIndex];
    }
    #endregion

    Vector2 GetXLeftRightPos(RectTransform rectTransform, Rect rect)
    {
        Vector2 pos = new Vector2(rectTransform.localPosition.x + rect.xMin, rectTransform.localPosition.x + rect.xMax);
        return pos;
    }

    bool IsXInRange(float x, Vector2 range)
    {
        return range.x <= x && x <= range.y;
    }

    bool IsTextInCollider()
    {
        // left, right
        Vector2 textXPos = GetXLeftRightPos(_textRect, _textRect.rect);
        Vector2 colliderXPos = GetXLeftRightPos(_canvas_collider, _canvas_collider.rect);

        return IsXInRange(textXPos.x, colliderXPos) || IsXInRange(textXPos.y, colliderXPos);
               //IsXInRange(colliderXPos.x, textXPos) || IsXInRange(colliderXPos.y, textXPos);
    }

    bool IsTextExitCollider()
    {
        Vector2 textXPos = GetXLeftRightPos(_textRect, _textRect.rect);
        Vector2 colliderXPos = GetXLeftRightPos(_canvas_collider, _canvas_collider.rect);

        float dx = _canvas_collider.position.x + 5.0f; //誤差
        bool NotInCollider = (!IsTextInCollider());
        bool rightCollider = (textXPos.y + dx) <= colliderXPos.x;

        //return (!IsTextInCollider()) && colliderXPos.y >= textXPos.y;
        return (!IsTextInCollider()) && (textXPos.y + dx) <= colliderXPos.x;
    }

    private void Awake()
    {
        if (MasterText == null)
            MasterText = gameObject.transform.GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _textRect = MasterText.GetComponent<RectTransform>();
        
        if (_canvas_collider == null)
            _canvas_collider = GameObject.Find("UICanvas").transform.GetComponent<RectTransform>();

        // Y軸的隨機Range [a, b]
        Vector2 posYRange = new Vector2(_canvas_collider.rect.yMin + (MasterText.preferredHeight / 2), _canvas_collider.rect.yMax - (MasterText.preferredHeight / 2));
        
        //preferredWidth  > 因 text 掛載 content size fitter 自適應寬度 因此用 preferredWidth 取實際text的寬
        Vector3 initPos = new Vector3(_canvas_collider.rect.xMax + (MasterText.preferredWidth / 2), Random.Range(posYRange.x, posYRange.y), 0.0f);
        
        MasterText.transform.localPosition = initPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position -= Vector3.right * Speed * Time.deltaTime;
        
        if (IsTextExitCollider())
        {
            Destroy(gameObject);
        }
    }

    #region 垃圾 Debug.Log 註解區
    //thisText = gameObject.transform.GetComponent<Text>();
    //thisText.text = initTextString;
    //thisText.fontSize = fontSize;

    //Debug.Log("size: " + thisText.preferredWidth + ", " + thisText.preferredHeight);
    //Debug.Log("CanvasY: " + new Vector2(canvas_collider.rect.yMin, canvas_collider.rect.yMax));
    //Debug.Log("Range: " + posYRange);
    //Debug.Log("zxcv: " + canvas_collider.position);

    //Debug.Log("Not In Collider: " + NotInCollider.ToString());
    // Debug.Log("right: " + rightCollider.ToString());
    #endregion
}
