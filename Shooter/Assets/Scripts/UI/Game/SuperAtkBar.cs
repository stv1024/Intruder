using UnityEngine;

/// <summary>
/// 必杀能量槽和按钮
/// </summary>
public class SuperAtkBar : MonoBehaviour
{
    private static readonly Color ClrBar = new Color(113, 7, 22);

    public GameObject Bisha;

    public UISprite SprBar;

    public UISprite SprProgress;

    public GameObject PromptEffect;

    /// <summary>
    /// 能量+1。
    /// </summary>
    /// <param name="power"></param>
    /// <param name="powerMax">多少是满</param>
    public void AddOnPower(int power, int powerMax)
    {
        SprProgress.fillAmount = power*1f/powerMax;
        if (power >= powerMax)
        {
            Bisha.SetActive(true);
            SprBar.GetComponent<TweenColor>().PlayForward();
            PromptEffect.SetActive(true);
            collider.enabled = true;
        }
        else
        {
            Bisha.SetActive(false);
            SprBar.color = Color.black;
            PromptEffect.SetActive(false);
            collider.enabled = false;
        }
    }

    public void ClearPower()
    {
        Bisha.SetActive(false);
        SprBar.GetComponent<TweenColor>().enabled = false;
        PromptEffect.SetActive(false);
        SprProgress.fillAmount = 0;
        SprBar.color = Color.black;
        collider.enabled = false;
    }
}