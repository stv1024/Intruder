using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 游戏里，斜二测显示的物体。
/// </summary>
public class InGameObject : MonoBehaviour
{
    /// <summary>
    /// 美术内容的地址，相对于Resources/ArtContents/。例如填写"Items/1101"。null则没有。
    /// </summary>
    public virtual string ArtContentPath { get { return null; } }

    public GameObject ArtContent;

    /// <summary>
    /// 单个物体控制时间，1正常，>1变快。
    /// </summary>
    public float TimeScale = 1;

    /// <summary>
    /// 物理层的Std坐标
    /// </summary>
    public Vector2 Position
    {
        get { return transform.localPosition; }
        set { rigidbody2D.MovePosition(value*0.01f); }
    }

    /// <summary>
    /// Position+ArtContent的位置，若无ArtContent则就是Position
    /// </summary>
    public Vector2 VisualPosition
    {
        get { return ArtContent ? Position + ArtContent.transform.localPosition.ToVector2() : Position; }
    }

    /// <summary>
    /// 标准坐标下的速度。如果没有刚体，则返回0或无效。
    /// </summary>
    public Vector2 Velocity
    {
        get { return rigidbody2D ? rigidbody2D.velocity * 100 : Vector2.zero; }
        set { if (rigidbody2D) rigidbody2D.velocity = value / 100; }
    }

    public void SetPositionWithoutPhysics(Vector2 pos)
    {
        transform.localPosition = pos;
    }

    protected virtual void Awake()
    {
        if (!ArtContent && !string.IsNullOrEmpty(ArtContentPath))
        {
            var prefab = Resources.Load<GameObject>("ArtContents/" + ArtContentPath);
            if (prefab)
            {
                ArtContent = PrefabHelper.InstantiateAndReset(prefab, transform);
            }
            else
            {
                Debug.LogError("没有prefab怎么显示出来？name:" + name);
            }
        }
    }

    protected virtual void UpdateVisual()
    {
        transform.localPosition = transform.localPosition.SetV3Z(0);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}