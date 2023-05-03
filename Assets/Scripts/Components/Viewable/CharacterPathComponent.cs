using Entitas;
using System.Collections.Generic;
using UnityEngine;

[Game]
public class CharacterPathComponent : IComponent
{
    public List<Int2> pathPositions = new List<Int2>();
}