using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fairwood.Math;
using UnityEngine;
using UnityEditor;
using MathUtils = Fairwood.Math.MathUtils;

/// <summary>
/// Tester
/// </summary>
public class GameTester : EditorWindow
{
    [MenuItem("Morln/Game Tester")]
    static void Init()
    {
        var window = GetWindow<GameTester>("游戏测试器", true);
    }

    void Update()
    {
        Repaint();
    }

    private static float _enemyTimeScale = 1;
    void OnGUI()
    {
        
    }

    [MenuItem("Morln/测试Hash")]
    static void TestHas()
    {
        //var ori = DateTime.Today.ToUniversalTime().ToShortDateString();
        var log = "";
        var date = new DateTime(2014, 1, 1);
        var list = new List<string>();
        for (int i = 0; i < 365; i++)
        {
            var ori = MathUtils.GetUnifiedWxPromotionCode(date);
            list.Add(ori);
            log += ori + " " + date.ToString("MM/dd/yy") + "\n";
            date += new TimeSpan(1, 0, 0, 0);
        }
        var groups = list.GroupBy(x => x);
        foreach (var gr in groups)
        {
            if (gr.Count() > 1) Debug.Log(gr.ToList()[0]+"*"+gr.Count());
        }
        Debug.Log(log);

    }
}