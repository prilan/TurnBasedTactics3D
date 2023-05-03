using Entitas;

public enum ActiveCharacterPlayer
{
    CharacterA,
    CharacterB,
}

[Game]
public class ActiveCharacterDataComponent : IComponent
{
    public ActiveCharacterPlayer ActiveCharacter;
}