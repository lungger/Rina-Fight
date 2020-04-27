#define LocalKeyboardTest1
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;
public class OpenMenu : MonoBehaviour
{
    [SerializeField]
    GameObject Menu;

    [SerializeField]
    Input_Manager input;

    [SerializeField]
    Text title;

    bool isOpeningMenu = false;

    bool isKeyDown = false;

    bool IsOpenMenu()
    {
        /*if (joyInput.Now.Button_Menu)
        {
            if (isKeyDown)
                return false;
            isKeyDown = true;
            return true;
        }
        isKeyDown = false;
        return false;*/
        #if LocalKeyboardTest
        return Input.GetKeyDown(KeyCode.Space);
        #endif
        return input.IsKeyDown(input.Now.Button_Menu, input.Last.Button_Menu);
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOpenMenu() && title.text == "選單")
        {
            if (isOpeningMenu) //打開選單中
            {
                isOpeningMenu = false;
                Menu.SetActive(false);
                GameState.ReStartGame();
            }
            else
            {
                isOpeningMenu = true;
                Menu.SetActive(true);
                ChildrenFinder.FindByName(Menu, "Panel_MajorOptions", 0).GetComponent<SelectOptions>().Initialize();
                GameState.PauseGame();
            }
        }
    }
}
