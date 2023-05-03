public partial class GameContext {

    public GameEntity editActiveInstrumentEntity { get { return GetGroup(GameMatcher.EditActiveInstrument).GetSingleEntity(); } }
    public EditActiveInstrumentComponent editActiveInstrument { get { return editActiveInstrumentEntity.editActiveInstrument; } }
    public bool hasEditActiveInstrument { get { return editActiveInstrumentEntity != null; } }

    public GameEntity SetEditActiveInstrument(EditActiveInstrument editActiveInstrument) {
        if (hasEditActiveInstrument) {
            ReplaceEditActiveInstrument(editActiveInstrument);
            return editActiveInstrumentEntity;
        } else {
            var entity = CreateEntity();
            entity.AddEditActiveInstrumen(editActiveInstrument);
            return entity;
        }
    }

    public void ReplaceEditActiveInstrument(EditActiveInstrument newValue) {
        var entity = editActiveInstrumentEntity;
        if (entity == null) {
            entity = SetEditActiveInstrument(newValue);
        } else {
            entity.ReplaceEditActiveInstrumen(newValue);
        }
    }

    public void RemoveEditActiveInstrument() {
        editActiveInstrumentEntity.Destroy();
    }
}

public partial class GameEntity {

    public EditActiveInstrumentComponent editActiveInstrument { get { return (EditActiveInstrumentComponent)GetComponent(GameComponentsLookup.EditActiveInstrument); } }
    public bool hasEditActiveInstrument { get { return HasComponent(GameComponentsLookup.EditActiveInstrument); } }

    public void AddEditActiveInstrumen(EditActiveInstrument newValue) {
        var index = GameComponentsLookup.EditActiveInstrument;
        var component = CreateComponent<EditActiveInstrumentComponent>(index);
        component.editActiveInstrument = newValue;
        AddComponent(index, component);
    }

    public void ReplaceEditActiveInstrumen(EditActiveInstrument newValue) {
        var index = GameComponentsLookup.EditActiveInstrument;
        var component = CreateComponent<EditActiveInstrumentComponent>(index);
        component.editActiveInstrument = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveEditActiveInstrumen() {
        RemoveComponent(GameComponentsLookup.EditActiveInstrument);
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherEditActiveInstrument;

    public static Entitas.IMatcher<GameEntity> EditActiveInstrument
    {
        get {
            if (_matcherEditActiveInstrument == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.EditActiveInstrument);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherEditActiveInstrument = matcher;
            }

            return _matcherEditActiveInstrument;
        }
    }
}
