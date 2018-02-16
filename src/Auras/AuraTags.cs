using System;

[Flags]
public enum AuraTags
{
    None = 0,
    Beneficial = 1 << 0,
    Detrimental = 1 << 1,
    Magic = 1 << 2,
    Movement = 1 << 3,
    Disease = 1 << 4,
    Poison = 1 << 5,
    Burning = 1 << 6,
    Chilled = 1 << 7,
    Slow = 1 << 8,
    Root = 1 << 9,
    Freeze = 1 << 10,
    Petrification = 1 << 11,
    Stun = 1 << 12,
    Daze = 1 << 13,
    Vulnerability = 1 << 14,
    Resistance = 1 << 15,
    Immunity = 1 << 16,
}