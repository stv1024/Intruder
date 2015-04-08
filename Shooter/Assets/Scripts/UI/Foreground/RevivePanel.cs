using System.Collections.Generic;
using Assets.Scripts.PayStrategy;
using Scripts.PayChannels;
using UnityEngine;

public class RevivePanel : BaseTempSingletonPanel
{
    #region 单例面板通用

    private static RevivePanel _instance;

    public static RevivePanel Instance
    {
        get { return _instance; }
        private set
        {
            if (_instance && value)
            {
                Debug.LogError("more than 1 RevivePanel instance now!");
                Destroy(_instance.gameObject);
            }
            _instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    protected override void OnDestroy()
    {
        Instance = null;
        base.OnDestroy();
    }

    private static GameObject Prefab
    {
        get
        {
            var go = Resources.Load("UI/Foreground/RevivePanel") as GameObject;
            return go;
        }
    }

    public static void Load()
    {
        if (Instance)
        {
            MainRoot.FocusPanel(Instance);
        }
        else
        {
            if (!Prefab)
            {
                Debug.LogWarning("找不到Panel的Prefab");
                return;
            }
            MainRoot.ShowPanel(Prefab);
        }
    }

    public static void UnloadInterface()
    {
        if (Instance) Instance.Unload();
    }

    #endregion

    public GameObject GrpTestHint;
    public UILabel LblTitle;
    public UILabel LblReviveCost;

    protected override void Initialize()
    {
        base.Initialize();

        var level = -1;//(修改MainData大力丸条件时记得修改这里的距离下一个大力丸的信息)
        if (RoundData.Level <= 4 && !ForeverDataHolder.ForeverData.ChangshengwanEatenList[0])
        {
            level = 4;
        }
        else if (RoundData.Level <= 7 && !ForeverDataHolder.ForeverData.ChangshengwanEatenList[1])
        {
            level = 7;
        }
        else if (RoundData.Level <= 9 && !ForeverDataHolder.ForeverData.ChangshengwanEatenList[1])
        {
            level = 9;
        }
        LblTitle.text = level > 0 ? string.Format("击败第{0}关BOSS可以使生命值上限永久+1", level) : null;
        LblReviveCost.text = "" + RoundData.NextRevivePrice;

        UMengPlugin.Event("revive_show", new Dictionary<string, object> {{"level", RoundData.Level}});
    }

    public void OnReviveClick()
    {
        //if (ForeverDataHolder.Coin >= RoundData.NextRevivePrice)
        //{
        //    ForeverDataHolder.Coin -= (int) RoundData.NextRevivePrice;
        //    OnRevivePayOk();
        //}
        //else
        //{
        //    Debug.Log("魂石不够复活，直接支付");
        //    new AlipayPayStrategy().Pay(new UserSelectedTrade("revive", "满血复活我的英雄", RoundData.NextRevivePrice)); //TODO:如何定价，如何描述更有吸引力
        //}
    }

    /// <summary>
    /// 为复活充值成功
    /// </summary>
    public void OnRevivePayOk()
    {
        //复活
        GameManager.Instance.RevivePlayer();

        Time.timeScale = 1f;

        if (RoundData.NextRevivePrice < 640)
            RoundData.NextRevivePrice *= 2;

        UnloadInterface();
    }

    public override void OnConfirmClick()
    {
        //放弃复活

        GameManager.Instance.Clear();
        //MainRoot.Goto(MainRoot.UIStateName.EndRound);
        //if (EndRoundUI.Instance)
        //    EndRoundUI.Instance.Refresh();
        base.OnConfirmClick();

        UMengPlugin.Event("revive_close", new Dictionary<string, object> { { "level", RoundData.Level } });
    }
}