//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputEntity {

    static readonly MouseOverUiComponent mouseOverUiComponent = new MouseOverUiComponent();

    public bool isMouseOverUi {
        get { return HasComponent(InputComponentsLookup.MouseOverUi); }
        set {
            if (value != isMouseOverUi) {
                var index = InputComponentsLookup.MouseOverUi;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : mouseOverUiComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class InputMatcher {

    static Entitas.IMatcher<InputEntity> _matcherMouseOverUi;

    public static Entitas.IMatcher<InputEntity> MouseOverUi {
        get {
            if (_matcherMouseOverUi == null) {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.MouseOverUi);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherMouseOverUi = matcher;
            }

            return _matcherMouseOverUi;
        }
    }
}