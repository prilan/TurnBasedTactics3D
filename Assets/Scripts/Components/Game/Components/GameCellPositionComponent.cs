public partial class GameEntity {

    public CellPositionComponent cellPosition { get { return (CellPositionComponent)GetComponent(GameComponentsLookup.CellPosition); } }
    public bool hasCellPosition { get { return HasComponent(GameComponentsLookup.CellPosition); } }

    public void AddCellPosition(Int2 newValue) {
        var index = GameComponentsLookup.CellPosition;
        var component = CreateComponent<CellPositionComponent>(index);
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCellPosition(Int2 newValue) {
        var index = GameComponentsLookup.CellPosition;
        var component = CreateComponent<CellPositionComponent>(index);
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveCellPosition() {
        RemoveComponent(GameComponentsLookup.CellPosition);
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherCellPosition;

    public static Entitas.IMatcher<GameEntity> CellPosition {
        get {
            if (_matcherCellPosition == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CellPosition);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCellPosition = matcher;
            }

            return _matcherCellPosition;
        }
    }
}
