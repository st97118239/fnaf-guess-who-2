using System;

[Flags]
public enum PlayerFlags
{
    None,
    CanInteract,
    CanPause
}

public static class PlayerFlagsExtensions
{
    public static bool HasFlag(this PlayerFlags flags, PlayerFlags flagToCheck)
    {
        return (flagToCheck & flags) > 0;
    }

    public static PlayerFlags AddFlag(this PlayerFlags flags, PlayerFlags flagToAdd)
    {
        return flags |= flagToAdd;
    }

    public static PlayerFlags RemoveFlag(this PlayerFlags flags, PlayerFlags flagToRemove)
    {
        return flags &= ~flagToRemove;
    }
}