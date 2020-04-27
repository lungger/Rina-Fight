using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleController : MonoBehaviour
{
    [SerializeField]
    Text Title;

    [SerializeField]
    string Title_Text; //這一頁的標題要顯示的文字

    // Start is called before the first frame update
    void Start()
    {
        Title.text = "選單";
    }

    // Update is called once per frame
    void Update()
    {
        if (Title.text != Title_Text)
        {
            Title.text = Title_Text;
        }
    }
}
