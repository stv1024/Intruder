using System;
using Fairwood;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 枪口
/// </summary>
public class Shooter : MonoBehaviour
{
    public Fighter Owner;

    public float BulletSpeed = 200;

    public enum TrajectoryEnum
    {
        Straight,
        Sniper,
        Scatter,
    }

    public TrajectoryEnum Trajectory;

    public void Shoot(GameObject bulletTemplate)
    {
        switch (Trajectory)
        {
            case TrajectoryEnum.Straight:
                {
                    var bullet = PrefabHelper.InstantiateAndReset<Bullet>(bulletTemplate, transform);
                    bullet.transform.ResetTransform();
                    bullet.transform.parent = GameManager.Container;
                    bullet.transform.localScale = bulletTemplate.transform.localScale;
                    bullet.gameObject.SetActive(true);
                    bullet.Velocity = Owner.transform.up * BulletSpeed;
                }
                break;
            case TrajectoryEnum.Sniper:
                {
                    var bullet = PrefabHelper.InstantiateAndReset<Bullet>(bulletTemplate, transform);
                    bullet.transform.ResetTransform();
                    bullet.transform.parent = GameManager.Container;
                    bullet.transform.localScale = bulletTemplate.transform.localScale;
                    bullet.gameObject.SetActive(true);
                    if (Owner.Target)
                    {
                        bullet.Velocity = (Owner.Target.Position - bullet.Position).normalized * BulletSpeed;
                    }
                    else
                    {
                        bullet.Velocity = Owner.transform.up * BulletSpeed;
                    }
                }
                break;
            case TrajectoryEnum.Scatter:
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var bullet = PrefabHelper.InstantiateAndReset<Bullet>(bulletTemplate, transform);
                        bullet.transform.ResetTransform();
                        bullet.transform.parent = GameManager.Container;
                        bullet.transform.localScale = bulletTemplate.transform.localScale;
                        bullet.gameObject.SetActive(true);
                        bullet.Velocity =
                            (Owner.transform.up + Vector3.Cross(Owner.transform.up, new Vector3(0, 0, 1))*(i - 2)/5f)
                                .normalized * BulletSpeed;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Shooter.psd", true);
        Gizmos.DrawRay(transform.position, transform.up*0.1f);
    }
}