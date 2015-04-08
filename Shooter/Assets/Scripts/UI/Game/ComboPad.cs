using UnityEngine;

/// <summary>
/// 显示Combo的Pad
/// </summary>
public class ComboPad : MonoBehaviour
{
    public UILabel LblCombo;

    /// <summary>
    /// Combo增加动画，如果没有ComboPad则会先显示Pad
    /// </summary>
    /// <param name="newCombo">当前Combo数值</param>
    public void ForceShowComboAdd(int newCombo)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        LblCombo.text = newCombo.ToString();
        LblCombo.animation.Play();
    }

    public void ImmediatelyHide()
    {
        gameObject.SetActive(false);
    }
}