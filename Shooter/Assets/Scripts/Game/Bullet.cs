using Assets.Scripts.Game;
using UnityEngine;

/// <summary>
/// 子弹，用Layer区分敌我
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : InGameObject, ICanAttack
{
    public const int LAYER_PLAYER = 10;
    public const int LAYER_ENEMY = 14;

    #region 初始数值

    public int InitDamage;

    #endregion

    #region 当前数值

    public int Damage { get; private set; }

    #endregion


    protected override void Awake()
    {
        base.Awake();

        Damage = InitDamage;
    }

    protected virtual void Update()
    {
        //Debug.LogWarning("v:" + rigidbody2D.velocity.magnitude);
        //if (!GameManager.IsPointInsideBattleField(Position)) Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerStay2D(other);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("hit:" + other.name + "@" + Time.frameCount);
        //if (!enabled) return;
        //var fighter = other.GetComponent<Fighter>();
        //if (fighter)//是战机
        //{
        //    fighter.BeAttacked(this);
        //    enabled = false;
        //    Destroy(gameObject);
        //}
    }
}