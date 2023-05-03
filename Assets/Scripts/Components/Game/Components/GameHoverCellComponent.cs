public partial class GameEntity {

    public HoverCellComponent hoverCell { get { return (HoverCellComponent)GetComponent(GameComponentsLookup.HoverCell); } }
    public bool hasHoverCell { get { return HasComponent(GameComponentsLookup.HoverCell); } }

    public void AddHoverCellPosition(Int2 newValue) {
        var index = GameComponentsLookup.HoverCell;
        var component = CreateComponent<HoverCellComponent>(index);
        component.hoverCellPosition = newValue;
        AddComponent(index, component);
    }

    public void ReplaceHoverCellPosition(Int2 newValue) {
        var index = GameComponentsLookup.HoverCell;
        var component = CreateComponent<HoverCellComponent>(index);
        component.hoverCellPosition = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveHoverCellPosition() {
        RemoveComponent(GameComponentsLookup.HoverCell);
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherHoverCellPosition;

    public static Entitas.IMatcher<GameEntity> HoverCell {
        get {
            if (_matcherHoverCellPosition == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.HoverCell);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherHoverCellPosition = matcher;
            }

            return _matcherHoverCellPosition;
        }
    }
}
