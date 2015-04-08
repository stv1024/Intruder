using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 登录界面
/// </summary>
public class EntranceUI : BaseUI
{
    #region 单例UI通用

    public static EntranceUI Instance { get; private set; }

    private static GameObject Prefab
    {
        get
        {
            var go = Resources.Load("UI/EntranceUI") as GameObject;
            return go;
        }
    }

    /// <summary>
    /// 实例化。开始切换时调用。不会重复加载
    /// </summary>
    /// <returns></returns>
    public static EntranceUI Load(bool useAnimation = true)
    {
        if (Instance) return Instance;//不重复加载
        if (!Prefab) return null;
        var ui = MainRoot.LoadUI(Prefab, useAnimation);
        Instance = ui.GetComponent<EntranceUI>();
        Instance.ResetUIRange();
        Instance.Initialize();
        if (useAnimation) Instance.DelayEnterStageCoroutine();
        return Instance;
    }

    public override void ExitStage(bool useAnimation)
    {
        Instance = null;//成为游离态
        base.ExitStage(useAnimation);
    }
    #endregion

    private GameObject _bg;

    public GameObject[] HideWhenOffStageObjs;
    public GameObject BtnStart;

    private void Start()
    {
        if (!_bg)
        {
            const string path = "UI/Entrance/First_UI";
            var prefab = Resources.Load<GameObject>(path);
            if (prefab)
            {
                _bg = PrefabHelper.InstantiateAndReset(prefab, transform);
                _bg.transform.localPosition = _bg.transform.localPosition.SetV3Z(500);
                var anmtr = _bg.GetComponentInChildren<Animator>();
                if (anmtr) anmtr.SetTrigger("First_Appear");
            }
            else
            {
                Debug.LogError("找不到Prefab@" + path);
            }
        }
    }

    //protected override System.Collections.IEnumerator OnStageCoroutine()
    //{
    //    yield break;
    //}

    //protected override System.Collections.IEnumerator OffStageEffectCoroutine()
    //{
    //    //if (_bg)
    //    //{
    //    //    var anmtr = _bg.GetComponentInChildren<Animator>();
    //    //    if (anmtr) anmtr.SetTrigger("First_Disappear");
    //    //}
    //    //foreach (var go in HideWhenOffStageObjs)
    //    //{
    //    //    if (go) go.SetActive(false);
    //    //}
    //    yield break;
    //}

    public void OnStartClick()
    {
        if (!BtnStart.activeInHierarchy) return;//防止点击两次
        if (_bg)
        {
            var anmtr = _bg.GetComponentInChildren<Animator>();
            if (anmtr) anmtr.SetTrigger("First_Disappear");
        }
        foreach (var go in HideWhenOffStageObjs)
        {
            if (go) go.SetActive(false);
        }
        Invoke("StartGotoGame", 0.96f);
    }

    void StartGotoGame()
    {
        //MainRoot.Goto(MainRoot.UIStateName.Game);
        //GameManager.PreloadAllGameAssets();
        GameManager.Instance.RestartGame();
        //GameManager.Instance.Invoke("RestartGame", 1.5f);//TODO:测试
    }

    public void OnUpgradeClick()
    {
        //MainRoot.Goto(MainRoot.UIStateName.Upgrade);
    }

    public override bool OnEscapeClick()
    {
        Application.Quit();
        return true;
    }
}