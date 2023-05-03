public partial class GameEntity {

    static readonly ActiveCharacterComponent activeCharacterComponent = new ActiveCharacterComponent();

    public bool isActiveCharacter
    {
        get { return HasComponent(GameComponentsLookup.ActiveCharacter); }
        set {
            if (value != isActiveCharacter) {
                var index = GameComponentsLookup.ActiveCharacter;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : activeCharacterCellComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherActiveCharacter;

    public static Entitas.IMatcher<GameEntity> ActiveCharacter
    {
        get {
            if (_matcherActiveCharacter == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ActiveCharacter);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherActiveCharacter = matcher;
            }

            return _matcherActiveCharacter;
        }
    }
}
