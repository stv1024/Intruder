using UnityEngine;

/// <summary>
/// 玩家主战机
/// </summary>
public class PlayerFighter : Fighter
{
    public Claw Claw;

    /// <summary>
    /// 如果是Unused则发射，如何是伸长阶段，缩回
    /// </summary>
    public void ToggleClaw()
    {
        if (Claw.State == Claw.StateEnum.Unused) Claw.BeThrown();
        else if (Claw.State == Claw.StateEnum.Extend) Claw.ForceRetract();
    }
}