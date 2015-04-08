using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

/// <summary>
/// 主控制器
/// </summary>
public class MainController : MonoBehaviour
{
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start()
    {
        //获取、检测数据

        //DownloadClientConfigSaleChannel();//下载客户端配置并加载。

        //关卡数据
        var path = MainData.AllLevelDataPath;
        var ta = Resources.Load<TextAsset>(path);
        if (ta)
        {
            MainData.LoadAllLevelData(ta.bytes);
        }
        else
        {
            Debug.LogError("没有关卡数据，无法正常进行游戏");
        }

        //永久数据
        ForeverDataHolder.PrepareData(true);
        
        MainData.CheckAllData();

        CheckConditionsAndStartLaunch();
    }

    void Update()
    {
        CheckConditionsAndStartLaunch();

        if (Input.GetKeyUp(KeyCode.A))
        {
            EntranceUI.Load(false);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            EntranceUI.Load();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            GameUI.Load();
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            EntranceUI.Load();
            GameUI.Load();
            EntranceUI.Load();
            GameUI.Load();
            EntranceUI.Load();
            GameUI.Load();
            EntranceUI.Load();
            GameUI.Load();
            EntranceUI.Load();
            GameUI.Load();
        }
    }

    public delegate bool LaunchConditionHandler();

    /// <summary>
    /// 在程序的最初Awake里添加条件，之后，条件全部满足时才会启动进入入口界面。
    /// 用于某些渠道需要在之前加入新的对话框等等。
    /// </summary>
    public readonly static List<LaunchConditionHandler> LaunchConditions = new List<LaunchConditionHandler>();

    /// <summary>
    /// 是否已经StartLaunch过了。
    /// </summary>
    private static bool _hasLaunched;

    /// <summary>
    /// 开始加载入口界面
    /// </summary>
    void CheckConditionsAndStartLaunch()
    {
        if (_hasLaunched) return;
        foreach (var launchCondition in LaunchConditions)
        {
            if (!launchCondition()) return;//有条件不满足就不能启动。
        }
        //满足所有条件，可以启动啦。
        _hasLaunched = true;
        //MainRoot.Goto(MainRoot.UIStateName.Entrance);TODO:
    }

    private void DownloadClientConfigSaleChannel()
    {
        var url = string.Format("http://morlnninja.qiniudn.com/ClientConfig.{0}.txt",
                                ClientInfoHolder.Instance.SaleChannel);
        DownloadMgr.Instance.DownString(url, result =>
        {
            switch (result.StatusCode)
            {
                case 200:
                    LoadClientConfig(result.Content);
                    break;
                default:
                    Debug.LogError("下载失败，准备用All的配置。url:" + result.Url + "; code:" + result.StatusCode);
                    DownloadClientConfigAll();
                    break;
            }
        });
    }

    private void DownloadClientConfigAll()
    {
        const string url = "http://morlnninja.qiniudn.com/ClientConfig.All.txt";
        DownloadMgr.Instance.DownString(url, result =>
            {
                switch (result.StatusCode)
                {
                    case 200:
                        LoadClientConfig(result.Content);
                        break;
                    default:
                        Debug.LogError("下载失败，只能使用默认配置。url:" + result.Url + "; code:" + result.StatusCode);
                        InitAlipay();
                        break;
                }
            });
    }

    
    void LoadClientConfig(string config)
    {
        var lines = config.Replace("\r", null).Split('\n');
        foreach (var line in lines)
        {
            var words = line.Split(',');
            if (words.Length < 2) continue;
            switch (words[0])
            {
                case "AlipayNotifyUrl":
                    ClientInfoHolder.Instance.AliNotifyUrl = words[1];
                    break;
                case "AliPid":
                    ClientInfoHolder.Instance.AliPid = words[1];
                    break;
                case "AliSid":
                    ClientInfoHolder.Instance.AliSid = words[1];
                    break;
                case "AliPkey":
                    ClientInfoHolder.Instance.AliPkey = words[1];
                    break;
            }
        }

        InitAlipay();
    }
    void InitAlipay()
    {
        AlipayPlugin.Init(ClientInfoHolder.Instance.AliPid, ClientInfoHolder.Instance.AliSid,
                          ClientInfoHolder.Instance.AliPkey);
    }
}