using System;
using System.Collections.Generic;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 主角血条
/// </summary>
public class HpBar : MonoBehaviour
{
    public UIProgressBar SprHpBar;

    private const float EvolveSpeed = 1f;//一秒钟几格
    private float _curHp, _curHpMax;
    private int _tarHp, _tarHpMax;
    private const int HpPerGrid = 40;

    public UISprite Pad;
    public GameObject BloodTemplate;
    public UIGrid Grid;
    public List<UISprite> SprBloods = new List<UISprite>();

    void Awake()
    {
        BloodTemplate.SetActive(false);
    }
    public void SetImmediately(int hp, int hpMax)
    {
        _tarHp = hp;
        _tarHpMax = hpMax;
        _curHp = hp;
        _curHpMax = hpMax;
        RefreshBloodSprites();
    }
    public void EvolveTo(int hp, int hpMax)
    {
        _tarHp = hp;
        _tarHpMax = hpMax;
    }

    void Update()
    {
        var needRefresh = false;
        if (Math.Abs(_curHp - _tarHp) > 0.001f)
        {
            var delta = _tarHp - _curHp;
            if (delta >= 0)
            {
                _curHp  += Mathf.Min(delta, EvolveSpeed*RealTime.deltaTime*HpPerGrid);
            }
            else
            {
                _curHp -= Mathf.Min(-delta, EvolveSpeed * RealTime.deltaTime * HpPerGrid);
            }
            needRefresh = true;
        }
        if (Math.Abs(_curHpMax - _tarHpMax) > 0.001f)
        {
            var delta = _tarHpMax - _curHpMax;
            if (delta >= 0)
            {
                _curHpMax += Mathf.Min(delta, EvolveSpeed * RealTime.deltaTime * HpPerGrid);
            }
            else
            {
                _curHpMax -= Mathf.Min(-delta, EvolveSpeed * RealTime.deltaTime * HpPerGrid);
            }
            needRefresh = true;
        }
        if (needRefresh)
        {
            RefreshBloodSprites();
        }
    }

    void RefreshBloodSprites()//float 因为可能是平滑动画
    {
        var totalCount = Mathf.CeilToInt(_curHpMax * 1f / HpPerGrid);//如果上限不是40的倍数，也要支持
        if (totalCount > 100) return;//太大了，一定是在测试
        Pad.width = (int) (61 + Grid.cellWidth*totalCount);//底板也会动画
        var fullCount = Mathf.FloorToInt(_curHp/HpPerGrid);
        var neededCount = Mathf.CeilToInt(_curHp / HpPerGrid);//根据需要补全所有血条
        if (SprBloods.Count < neededCount)
        {
            while (SprBloods.Count < neededCount)
            {
                var go = PrefabHelper.InstantiateAndReset(BloodTemplate, Grid.transform);
                go.name = SprBloods.Count.ToString("000");
                go.SetActive(true);
                var spr = go.GetComponent<UISprite>();
                SprBloods.Add(spr);
            }
            Grid.repositionNow = true;
        }
        for (int i = 0; i < SprBloods.Count; i++)
        {
            if (i < fullCount)
            {
                SprBloods[i].fillAmount = 1;
                continue;
            }
            var overHp = _curHp - i * HpPerGrid;
            if (overHp > 0) SprBloods[i].fillAmount = overHp/HpPerGrid;
            else SprBloods[i].fillAmount = 0;
        }

    }
}