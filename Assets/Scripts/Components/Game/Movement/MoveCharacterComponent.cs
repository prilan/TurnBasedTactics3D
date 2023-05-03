using Entitas;
using System.Collections.Generic;
using UnityEngine;

[Game]
public class MoveCharacterComponent : IComponent
{
    public List<Int2> pathCellPositions;
    public MoveCharacterData moveCharacterData;

    public partial class MoveCharacterData
    {
        public int moveStepIndex = -1;
        public int frameInStepIndex = -1;
    }
}
