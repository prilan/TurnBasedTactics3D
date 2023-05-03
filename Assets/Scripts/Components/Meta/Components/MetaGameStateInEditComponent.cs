public partial class MetaContext {

    public MetaEntity gameStateInEditEntity { get { return GetGroup(MetaMatcher.GameStateInEdit).GetSingleEntity(); } }

    public bool isGameStateInEdit {
        get { return gameStateInEditEntity != null; }
        set {
            var entity = gameStateInEditEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isGameStateInEdit = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

public partial class MetaEntity {

    static readonly GameStateInEditComponent gameStateInEditComponent = new GameStateInEditComponent();

    public bool isGameStateInEdit
    {
        get { return HasComponent(MetaComponentsLookup.GameStateInEdit); }
        set {
            if (value != isGameStateInEdit) {
                var index = MetaComponentsLookup.GameStateInEdit;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : gameStateInEditComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class MetaMatcher {

    static Entitas.IMatcher<MetaEntity> _matcherGameStateInEdit;

    public static Entitas.IMatcher<MetaEntity> GameStateInEdit {
        get {
            if (_matcherGameStateInEdit == null) {
                var matcher = (Entitas.Matcher<MetaEntity>)Entitas.Matcher<MetaEntity>.AllOf(MetaComponentsLookup.GameStateInEdit);
                matcher.componentNames = MetaComponentsLookup.componentNames;
                _matcherGameStateInEdit = matcher;
            }

            return _matcherGameStateInEdit;
        }
    }
}
