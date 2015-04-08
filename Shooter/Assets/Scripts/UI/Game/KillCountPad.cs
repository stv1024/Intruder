using UnityEngine;

/// <summary>
/// 杀敌数板子
/// </summary>
public class KillCountPad : MonoBehaviour
{

    public UILabel LblKillCount;

    public void RefreshKillCount(int killCount)
    {
        LblKillCount.text = killCount.ToString();
    }

    public void ResetTo(int killCount)
    {
        LblKillCount.text = killCount.ToString();
    }
}