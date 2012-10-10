namespace BattleShip.Core
{
    public enum ShotResult
    {
        IllegalPlayer,
        TargetHasNoLocation,
        OutsideTargetZone,
        Miss,
        Hit,
        GameAlreadyOver
    }
}