using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPage : MonoBehaviour
{
    [Header("前一頁")]
    [SerializeField]
    protected GameObject _prePage; //前一頁

    [Header("現在這一頁")]
    [SerializeField]
    protected GameObject _thisPage; //這一頁

    public virtual void Exit()
    {
        _prePage.SetActive(true);
        _thisPage.SetActive(false);
    }

    private void Awake()
    {
        if (_thisPage == null)
            _thisPage = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
