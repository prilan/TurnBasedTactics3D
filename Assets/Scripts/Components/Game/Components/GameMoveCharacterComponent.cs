using System.Collections.Generic;
using UnityEngine;

public partial class GameEntity {

    public MoveCharacterComponent moveCharacter { get { return (MoveCharacterComponent)GetComponent(GameComponentsLookup.MoveCharacter); } }
    public bool hasMoveCharacter { get { return HasComponent(GameComponentsLookup.MoveCharacter); } }

    public void AddMoveCharacter(List<Int2> pathCellPositions) {
        var index = GameComponentsLookup.MoveCharacter;
        MoveCharacterComponent component = CreateComponent<MoveCharacterComponent>(index);
        component.pathCellPositions = pathCellPositions;
        component.moveCharacterData = new MoveCharacterComponent.MoveCharacterData();
        AddComponent(index, component);
    }

    public void RemoveMoveCharacter() {
        RemoveComponent(GameComponentsLookup.MoveCharacter);
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMoveCharacter;

    public static Entitas.IMatcher<GameEntity> MoveCharacter {
        get {
            if (_matcherMoveCharacter == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MoveCharacter);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMoveCharacter = matcher;
            }

            return _matcherMoveCharacter;
        }
    }
}
