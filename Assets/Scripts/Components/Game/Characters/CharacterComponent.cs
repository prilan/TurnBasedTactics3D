using Entitas;
using UnityEngine;

[Game]
public class CharacterComponent : IComponent
{
    public Int2 cellPosition;
    public ActiveCharacterPlayer characterPlayer;
}