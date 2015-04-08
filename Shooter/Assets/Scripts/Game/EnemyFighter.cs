using Assets.Scripts.Game;
using UnityEngine;

/// <summary>
/// 敌军
/// </summary>
public class EnemyFighter : Fighter
{
    public const int LAYER_ENEMY = 13;

    #region 初始数值

    public float Speed;

    #endregion

    public Vector2 ReadyAttackPosition;
    protected override void UpdateAI()
    {
        var step = Speed*Time.deltaTime;
        if (Vector2.Distance(Position, ReadyAttackPosition) <= step)
        {
            Position = ReadyAttackPosition;
            Velocity = Vector2.zero;
        }
        else
        {
            Velocity = (ReadyAttackPosition - Position).normalized*Speed;
        }
    }

    public void BeGrabbed(Claw claw)
    {
        //捕猎器与AI角力
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    OnTriggerStay2D(other);
    //}
    //void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log("hit:" + other.name);
    //}

    protected override void BeDamaged(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            Destroy(gameObject);
            EffectManager.CreateExplodeEffect(Position);
        }
    }
}