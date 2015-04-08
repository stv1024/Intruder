﻿using UnityEngine;

public class RechargePanel : BaseTempSingletonPanel
{
    #region 单例面板通用

    private static RechargePanel _instance;

    public static RechargePanel Instance
    {
        get { return _instance; }
        private set
        {
            if (_instance && value)
            {
                Debug.LogError("more than 1 RechargePanel instance now!");
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
            var go = Resources.Load("UI/Foreground/RechargePanel") as GameObject;
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

    public override bool OnEscapeClick()
    {
        UnloadInterface();
        return true;
    }
}