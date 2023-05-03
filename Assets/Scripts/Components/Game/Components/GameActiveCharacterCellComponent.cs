public partial class GameEntity {

    static readonly ActiveCharacterCellComponent activeCharacterCellComponent = new ActiveCharacterCellComponent();

    public bool isActiveCharacterCell
    {
        get { return HasComponent(GameComponentsLookup.ActiveCharacterCell); }
        set {
            if (value != isActiveCharacterCell) {
                var index = GameComponentsLookup.ActiveCharacterCell;
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

    static Entitas.IMatcher<GameEntity> _matcherActiveCharacterCell;

    public static Entitas.IMatcher<GameEntity> ActiveCharacterCell
    {
        get {
            if (_matcherActiveCharacterCell == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ActiveCharacterCell);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherActiveCharacterCell = matcher;
            }

            return _matcherActiveCharacterCell;
        }
    }
}
