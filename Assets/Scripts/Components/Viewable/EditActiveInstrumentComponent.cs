using Entitas;
using Entitas.CodeGeneration.Attributes;

public enum EditActiveInstrument
{
    CharacterA,
    CharacterB,
    Obstacle,
}

[Game]
[Unique]
public class EditActiveInstrumentComponent : IComponent
{
    public EditActiveInstrument editActiveInstrument;
}