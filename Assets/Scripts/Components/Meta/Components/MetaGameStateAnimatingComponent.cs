public partial class MetaContext {

    public MetaEntity gameStateAnimatingEntity { get { return GetGroup(MetaMatcher.GameStateAnimating).GetSingleEntity(); } }

    public bool isGameStateAnimating
    {
        get { return gameStateAnimatingEntity != null; }
        set {
            var entity = gameStateAnimatingEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isGameStateAnimating = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

public partial class MetaEntity {

    static readonly GameStateAnimatingComponent gameStateAnimatingComponent = new GameStateAnimatingComponent();

    public bool isGameStateAnimating {
        get { return HasComponent(MetaComponentsLookup.GameStateAnimating); }
        set {
            if (value != isGameStateAnimating) {
                var index = MetaComponentsLookup.GameStateAnimating;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : gameStateAnimatingComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class MetaMatcher {

    static Entitas.IMatcher<MetaEntity> _matcherGameStateAnimating;

    public static Entitas.IMatcher<MetaEntity> GameStateAnimating
    {
        get {
            if (_matcherGameStateAnimating == null) {
                var matcher = (Entitas.Matcher<MetaEntity>)Entitas.Matcher<MetaEntity>.AllOf(MetaComponentsLookup.GameStateAnimating);
                matcher.componentNames = MetaComponentsLookup.componentNames;
                _matcherGameStateAnimating = matcher;
            }

            return _matcherGameStateAnimating;
        }
    }
}
