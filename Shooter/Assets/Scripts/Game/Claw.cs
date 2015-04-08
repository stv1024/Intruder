using Fairwood;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 捕猎器
/// </summary>
public class Claw : InGameObject
{
    public float FreeLength = 300;//自然长度

    public float K = 100;
    public float KBack = 30;
    public float B = 10;
    public float MinRetractSpeed = 50;//缩回时最小径向速度，确保快速收回

    /// <summary>
    /// 头部爪子
    /// </summary>
    public Transform Head;
    /// <summary>
    /// 手臂
    /// </summary>
    public SpriteRenderer Arm;
    /// <summary>
    /// 使用者战机
    /// </summary>
    public Fighter User;
    /// <summary>
    /// 猎物战机。一开始是null
    /// </summary>
    public Fighter Target;
    /// <summary>
    /// 爪头的位置，标准坐标
    /// </summary>
    public Vector2 HeadPos;

    public ClawHead ClawHead;

    private float _startThrowTime;

    public Vector2 Velocity;

    public enum SolutionEnum
    {
        /// <summary>
        /// 射出后达到最远点前都不改变x
        /// </summary>
        _1,
        /// <summary>
        /// x跟随主战机,完全由橡皮筋控制
        /// </summary>
        _2,
        /// <summary>
        /// 限制最小速度
        /// </summary>
        _3,
    }

    public enum StateEnum
    {
        Unused,
        Extend,
        Retract,
        
    }

    public StateEnum State;

    public bool Retracting { get { return State == StateEnum.Retract; } }//已达到最远点
    void Update()
    {
        //Position
        Position = User.Position;

        if (State == StateEnum.Unused) return;

        //Dynamics
        var vector = HeadPos - Position;
        var length = vector.magnitude;
        var accel = Vector2.zero;
        
        if (Retracting)
        {
            accel = length * KBack * -vector.normalized;
            accel -= B*Velocity;//空阻，也是确保收回
        }
        else
        {
            if (length > FreeLength)
            {
                accel = (length - FreeLength)*K*-vector.normalized;

                //accel.x = 0; //伸长阶段不左右偏移
            }
        }
        Velocity += accel * Time.deltaTime;
        if (State == StateEnum.Extend && Velocity.y <= 0)//开始缩回
        {
            State = StateEnum.Retract;
        }
        if (Retracting)
        {
            var normalSpeed = Vector2.Dot((HeadPos - Position).normalized, Velocity);
            if (normalSpeed > -MinRetractSpeed)//减除径向远离方向的速度，确保越来越近。TODO:如果故意绕圈呢
            {
                Velocity = Velocity - (HeadPos - Position).normalized*(normalSpeed + MinRetractSpeed);
            }
        }


        //Head
        HeadPos += Velocity * Time.deltaTime;

        vector = HeadPos - Position;
        transform.right = vector;
        Head.localPosition = transform.InverseTransformDirection(GameManager.Instance.TransformDirection(vector));
        if (Target)
        {
            Target.Position = GameManager.Instance.InverseTransformPoint(ClawHead.ContentPosition.position);
        }

        //Visual
        Arm.JoinTwoPoint(Vector2.zero, vector);

        //Check Physics
        if (Retracting)
        {
            if ((HeadPos - Position).magnitude < 20)
            {
                //收回
                State = StateEnum.Unused;
                if (Target)
                {
                    Destroy(Target.gameObject);
                    Target = null;
                }
                ResetToUnused();
            }
        }
    }

    public void TouchedCollider2D(Collider2D other)
    {
        if (Target) return;//有猎物就不要在处理碰撞了
        if (other.gameObject.layer == EnemyFighter.LAYER_ENEMY)
        {
            var ef = other.GetComponent<EnemyFighter>();
            if (ef)
            {
                State = StateEnum.Retract;
                ClawHead.collider2D.enabled = false;
                Target = ef;
                HeadPos = Target.Position +
                          (Position - Target.Position).normalized*ClawHead.ContentPosition.localPosition.y;
                Velocity = Vector2.zero;
                ef.BeGrabbed(this);
            }
            else
            {
                Debug.LogError("怎么可能没有脚本，检查bug");
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerStay2D(other); //受不了Unity的bug了，早知道就不用Physics2D了，两个方法都接收，确保不遗漏碰撞；同时确保不重复。
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        Debug.LogWarning("Claw OTS2D@" + Time.frameCount);
        if (other.gameObject.layer == EnemyFighter.LAYER_ENEMY) //是PickableItem
        {
            var ef = other.GetComponent<EnemyFighter>();
            if (ef)
            {
                ef.BeGrabbed(this);
            }
            else
            {
                Debug.LogError("怎么可能没有脚本，检查bug");
            }
        }
    }
    

    /// <summary>
    /// 发射出去
    /// </summary>
    public void BeThrown()
    {
        State = StateEnum.Extend;
        Position = User.Position;
        HeadPos = Position;
        _startThrowTime = GameManager.GameTime;
        Velocity = new Vector2(0, GetInitSpeed());
        if (Target)
        {
            Debug.LogError("怎么还抓着猎物，没处理，有bug！");
            Target = null;
        }
        ClawHead.collider2D.enabled = true;
    }

    public void ForceRetract()
    {
        if (State == StateEnum.Extend)
        {
            State = StateEnum.Retract;
        }
    }

    public void ResetToUnused()
    {
        State = StateEnum.Unused;
        Position = User.Position;
        HeadPos = Position;
        Velocity = Vector2.zero;
        ClawHead.collider2D.enabled = false;

        if (Target)
        {
            Debug.LogError("怎么还抓着猎物，没处理，有bug！");
            Target = null;
        }
        transform.right = Vector2.up;
        Head.localPosition = Vector2.zero;
        //Visual
        Arm.JoinTwoPoint(Vector2.zero, Vector2.zero);
    }

    public float InitSpeed;
    /// <summary>
    /// 爪头初速度
    /// </summary>
    /// <returns></returns>
    float GetInitSpeed()
    {
        return InitSpeed;
    }
}