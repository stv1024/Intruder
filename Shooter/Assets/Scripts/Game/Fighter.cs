using Assets.Scripts.Game;
using UnityEngine;

/// <summary>
/// 战机
/// </summary>
public class Fighter : InGameObject
{
    #region 初始数值

    public int ID;

    public override string ArtContentPath
    {
        get { return "Units/" + ID; }
    }

    #endregion

    #region 当前数值

    public int Hp;

    #endregion

    #region 场景中的配置

    public Shooter[] Shooters;//枪口

    private GameObject _bulletTemplate;//都是临时简单方案，将会自动搜索名为Bullet的子物体

    #endregion

    public InGameObject Target;

    protected override void Awake()
    {
        base.Awake();

        var bulletTra = transform.Find("Bullet");
        if (bulletTra)
        {
            _bulletTemplate = bulletTra.gameObject;
            _bulletTemplate.SetActive(false);
        }

        Shooters = GetComponentsInChildren<Shooter>(true);
        foreach (var shooter in Shooters)
        {
            shooter.Owner = this;
        }
    }

    protected virtual void Update()
    {
        UpdateAI();
        if (Time.frameCount % 20 == 0) Fire();
    }

    protected virtual void UpdateAI(){}

    protected void Fire()
    {
        if (!_bulletTemplate) return;

        foreach (var shooter in Shooters)
        {
            shooter.Shoot(_bulletTemplate);
        }
    }

    #region 受击

    public virtual void BeAttacked(ICanAttack attacker)
    {
        if (attacker == null) return;
        var bullet = attacker as Bullet;
        if (bullet)
        {
            BeDamaged(attacker.Damage);
        }
    }

    protected virtual void BeDamaged(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            //Hp = 0;
            //Destroy(gameObject, 1f);
        }
    }

    #endregion
}