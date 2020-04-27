using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    Input_Manager input;

    [SerializeField]
    ExitPage exitController;

    [Header("目前頁面")]
    public int nowPage = 0;

    [Header("教學頁的圖片")]
    public List<Sprite> tutorialList;

    public int NowPage
    {
        get { return nowPage; }
        set
        {
            nowPage = value;
            if (nowPage >= tutorialList.Count)
            {
                nowPage = 0;
            }
            else if(nowPage < 0)
            {
                nowPage = tutorialList.Count - 1;
            }
        }
    }

    private void Awake()
    {
        if (image == null)
            image = ChildrenFinder.FindByName(gameObject, "Image_Tutorial", 0).GetComponent<Image>();
        if (exitController == null)
            exitController = gameObject.GetComponent<ExitPage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        nowPage = 0;
        image.sprite = tutorialList[NowPage];
    }

    // Update is called once per frame
    void Update()
    {
        if (input.IsKeyDown(input.Now.Arrow_Right, input.Last.Arrow_Right))
        {
            NowPage++;
            image.sprite = tutorialList[NowPage];
        }
        else if (input.IsKeyDown(input.Now.Arrow_Left, input.Last.Arrow_Left))
        {
            NowPage--;
            image.sprite = tutorialList[NowPage];
        }

        if (input.IsKeyDown(input.Now.Button_Special, input.Last.Button_Special))
        {
            if (exitController != null)
            {
                exitController.Exit();
            }
        }
    }
}
