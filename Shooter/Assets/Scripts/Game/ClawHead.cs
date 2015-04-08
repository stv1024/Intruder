using UnityEngine;

/// <summary>
/// 捕猎器爪头
/// </summary>
public class ClawHead : MonoBehaviour
{
    public Claw Claw;

    /// <summary>
    /// 抓到猎物后猎物对准的位置。localPosition.x必须是0。
    /// </summary>
    public Transform ContentPosition;

    protected void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerStay2D(other); //受不了Unity的bug了，早知道就不用Physics2D了，两个方法都接收，确保不遗漏碰撞；同时确保不重复。
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        Debug.LogWarning("Claw head OTS2D@" + Time.frameCount);
        Claw.TouchedCollider2D(other);
    }
}