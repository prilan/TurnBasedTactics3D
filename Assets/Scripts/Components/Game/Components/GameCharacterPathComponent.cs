using System;
using System.Collections.Generic;

public partial class GameContext
{
    public GameEntity characterPathEntity { get { return GetGroup(GameMatcher.CharacterPath).GetSingleEntity(); } }

    public bool isCharacterPath
    {
        get { return characterPathEntity != null; }
        set
        {
            var entity = characterPathEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isCharacterPath = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

public partial class GameEntity {

    static readonly CharacterPathComponent characterPath = new CharacterPathComponent();

    public CharacterPathComponent CharacterPath => characterPath;

    public bool isCharacterPath {
        get { return HasComponent(GameComponentsLookup.CharacterPath); }
        set {
            if (value != isCharacterPath) {
                var index = GameComponentsLookup.CharacterPath;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : characterPath;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }

    public void SetCharacterPath(List<Int2> newValue) {
        var index = GameComponentsLookup.CharacterPath;
        var component = (CharacterPathComponent)GetComponent(index);
        component.pathPositions = newValue;
    }

    public void ClearCharacterPath()
    {
        var index = GameComponentsLookup.CharacterPath;
        var component = (CharacterPathComponent)GetComponent(index);
        component.pathPositions.Clear();
    }

    public void RemoveCharacterPath() {
        RemoveComponent(GameComponentsLookup.CharacterPath);
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherCharacterPathPosition;

    public static Entitas.IMatcher<GameEntity> CharacterPath
    {
        get {
            if (_matcherCharacterPathPosition == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CharacterPath);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCharacterPathPosition = matcher;
            }

            return _matcherCharacterPathPosition;
        }
    }
}
