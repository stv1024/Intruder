using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.UI.Foreground.Leaderboard;
using Fairwood.Math;
using UnityEngine;
using SimpleJson;
using ValueType = SimpleJson.ValueType;

public class LeaderboardPanel : BaseTempSingletonPanel
{
    #region 单例面板通用

    private static LeaderboardPanel _instance;

    public static LeaderboardPanel Instance
    {
        get { return _instance; }
        private set
        {
            if (_instance && value)
            {
                Debug.LogError("more than 1 LeaderboardPanel instance now!");
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
            var go = Resources.Load("UI/Foreground/LeaderboardPanel") as GameObject;
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

    public GameObject SlotTemplate;

    public UIGrid Grid;

    public List<LeaderboardSlot> SlotList = new List<LeaderboardSlot>();

    public LeaderboardSlot MySlot;

    public UIInput IptNickname;

    public GameObject GrpNoNickname;

    protected override void Initialize()
    {
        var nickname = "";//ForeverDataHolder.Nickname;
        if (string.IsNullOrEmpty(nickname))
        {
            GrpNoNickname.SetActive(true);
            MySlot.LblNickname.text = "请输入昵称";
        }
        else
        {
            GrpNoNickname.SetActive(false);
            MySlot.LblNickname.text = nickname;
        }
        StartCoroutine(Fetch());
    }

    void Refresh(List<LeaderboardItem> leaderboardItems, LeaderboardItem myItem)
    {
        if (leaderboardItems != null)
        {
            var count = Mathf.Min(10, leaderboardItems.Count);
            SlotTemplate.SetActive(false);
            for (int i = 0; i < count; i++)
            {
                var go = PrefabHelper.InstantiateAndReset(SlotTemplate, Grid.transform);
                go.SetActive(true);
                go.name = SlotTemplate.name + i.ToString("000");
                var slot = go.GetComponent<LeaderboardSlot>();
                SlotList.Add(slot);
                var item = leaderboardItems[i];
                slot.Refresh(item.Rank, item.Nickname, item.Score);
            }
        }
        Grid.repositionNow = true;

        if (myItem != null)
        {
            MySlot.Refresh(myItem.Rank, myItem.Nickname, myItem.Score);
            var nickname = "";// ForeverDataHolder.Nickname;
            if (string.IsNullOrEmpty(nickname))
            {
                GrpNoNickname.SetActive(true);
                MySlot.LblNickname.text = "请输入昵称";
            }
            else
            {
                GrpNoNickname.SetActive(false);
                MySlot.LblNickname.text = nickname;
            }
        }
    }

    public void OnSubmitClick()
    {
        //if (!string.IsNullOrEmpty(ForeverDataHolder.Nickname))
        //{
        //    return; //有昵称就不能修改了
        //}
        var nickname = IptNickname.value;
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogError("昵称为空");
            return;
        }
        //ForeverDataHolder.Nickname = nickname;
        GrpNoNickname.SetActive(false);
        MySlot.LblNickname.text = nickname;
        //上传成绩到服务器
        StartCoroutine(Submit());
    }

    public override bool OnEscapeClick()
    {
        UnloadInterface();
        return true;
    }

    class LeaderboardItem
    {
        public int Rank;
        public string Nickname;
        public int Score;

        public LeaderboardItem(JsonNode json)
        {
            var subNodeNames = json.SubNodeNames;
            if (subNodeNames.Contains("rank"))
            {
                var node = json["rank"];
                var valueType = node.ValueType;
                if (valueType == ValueType.Int || valueType == ValueType.Long)
                {
                    Rank = (int)node.Value;
                }
            }
            else
            {
                Debug.LogError("json里没有rank");
            }
            if (subNodeNames.Contains("nickname"))
            {
                var node = json["nickname"];
                var valueType = node.ValueType;
                if (valueType == ValueType.String)
                {
                    Nickname = node.Value as string;
                }
            }
            else
            {
                Debug.LogError("json里没有nickname");
            }
            if (subNodeNames.Contains("score"))
            {
                var node = json["score"];
                var valueType = node.ValueType;
                if (valueType == ValueType.Int || valueType == ValueType.Long || valueType == ValueType.Double)
                {
                    Score = (int)node.Value;
                }
            }
            else
            {
                Debug.LogError("json里没有score");
            }
        }
    }

    #region 通信模块

    private const string UrlPrefix = "http://morlnninja.qiniudn.com/sampleleader.";
    IEnumerator Fetch()
    {
        //var www = new WWW(UrlPrefix + "fetch?device_id=" + Setting.DeviceUID);//TODO:恢复
        var www = new WWW(UrlPrefix+"fetch00.txt");
        yield return www;
        if (www.error != null)
        {
            Debug.LogError("获取排行榜WWWW失败Error:" + www.error);
            yield break;
        }
        var text = www.text;
        try
        {
            var list = new List<LeaderboardItem>();
            var json = JsonNode.FromJson(text);
            var subNodeNames = json.SubNodeNames;
            if (subNodeNames.Contains("leaderboard_list"))
            {
                var jsonlist = json["leaderboard_list"];
                var count = jsonlist.ElementNodeCount;
                for (var i = 0; i < count; i++)
                {
                    var item = jsonlist[i];
                    list.Add(new LeaderboardItem(item));
                }

            }
            else
            {
                Debug.LogError("json里没有leaderboard_list");
            }
            LeaderboardItem myItem = null;
            if (subNodeNames.Contains("player"))
            {
                var item = json["player"];
                myItem = new LeaderboardItem(item);
            }
            else
            {
                Debug.LogError("json里没有leaderboard_list");
            }
            Refresh(list, myItem);
        }
        catch (Exception)
        {
            
            throw;
        }
    }
    
    /// <summary>
    /// 提交分数。刷新最高分时 和 提交昵称时 调用。
    /// </summary>
    /// <returns></returns>
    public static IEnumerator Submit()
    {
        //var www =
        //    new WWW(UrlPrefix + "submit?device_id=" + Setting.DeviceUID + "&nickname=" + ForeverDataHolder.Nickname +
        //            "&score=" + ForeverDataHolder.HighestScore);
        //yield return www;
        //if (www.error != null)
        //{
        //    Debug.LogError("提交成绩WWWW失败Error:" + www.error);
        //    yield break;
        //}
        //var text = www.text;
        //Debug.Log("提交成绩结果" + text);
        yield break;
    }

    #endregion
}
/*
获取
排行榜
	接口
		http://.../ninja/fetch
		示例http://.../ninja/fetch?device_id=AF32C-765DE&count=20
	请求参数
		string device_id
		int ?offset
			index从第几个开始，可选，默认0
		int ?count = 10
			数量，可选，默认10个/服务端可设置最大值，如20个/返回的数量不需要与count一致
	反馈参数
		[] leaderboard_list
			int rank
				名次，从1开始
			string nickname
				昵称
			int score
				分数
		示例
			{
	"leaderboard_list": [{"rank":1, "nickname":"大顽皮", "score":324},
	{"rank":2, "nickname":"忍术天王", "score":303}]
}
*/