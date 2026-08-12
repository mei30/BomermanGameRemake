namespace Levels
{
    /// <summary>
    /// Classic Bomberman tile types. Extend as needed (e.g. PowerUp, Water, Ice for variants).
    /// </summary>
    public enum TileType
    {
        Floor,          // walkable, empty
        SolidWall,      // indestructible border/pillar
        Brick           // destructible block, may hide a power-up
    }
}