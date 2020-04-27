using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView_OpenOptions : MonoBehaviour
{
    [SerializeField]
    Input_Manager input;

    [SerializeField]
    GameObject panel;

    public bool isOpenMenu = false;

    bool canOpenCloseMenu = true;

    // Start is called before the first frame update
    void Start()
    {
        if (panel != null)
        {
            panel = ChildrenFinder.FindByName(gameObject, "Panel_Options", 0);
        }
        panel.transform.localScale = new Vector3(1, 0, 1);
        panel.GetComponent<SelectOptions>().CanSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.IsKeyDown(input.Now.Button_Menu, input.Last.Button_Menu) && panel.gameObject.activeInHierarchy)
        {
            if (canOpenCloseMenu)
            {
                if (isOpenMenu) //選單打開中 > 關閉選單
                {
                    isOpenMenu = false;
                    StartCoroutine("CloseMenu");
                    panel.GetComponent<SelectOptions>().CanSelect = false;
                }
                else
                {
                    isOpenMenu = true;
                    StartCoroutine("OpenMenu");
                    panel.GetComponent<SelectOptions>().CanSelect = true;
                    panel.GetComponent<SelectOptions>().SetNowSelectIndex(0);
                }
                canOpenCloseMenu = false;
            }
        }
    }

    IEnumerator OpenMenu()
    {
        yield return new WaitUntil(() => isOpenMenu);
        while(panel.transform.localScale.y < 1.0f)
        {
            panel.transform.localScale += new Vector3(0, Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }
        panel.transform.localScale = new Vector3(1, 1, 1);
        canOpenCloseMenu = true;
    }

    IEnumerator CloseMenu()
    {
        yield return new WaitUntil(() => !isOpenMenu);
        while (panel.transform.localScale.y > 0.0f)
        {
            panel.transform.localScale -= new Vector3(0, Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }
        panel.transform.localScale = new Vector3(1, 0, 1);
        canOpenCloseMenu = true;
    }
}
