using Assets.Scripts.Game;
using UnityEngine;

/// <summary>
/// 僚机
/// </summary>
public class WingFighter : Fighter
{
    public PlayerFighter PlayerFighter;

    private Vector2 _relPosToPlayerFighter;

    protected void Start()
    {
        _relPosToPlayerFighter = Position - PlayerFighter.Position;
    }

    protected override void Update()
    {
        base.Update();

        SetPositionWithoutPhysics(PlayerFighter.Position + _relPosToPlayerFighter);
    }

}