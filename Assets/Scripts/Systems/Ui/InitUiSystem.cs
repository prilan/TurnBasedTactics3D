using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class InitUiSystem : IInitializeSystem {

	private readonly Contexts _contexts;
    private readonly UiContext _context;
    
    public InitUiSystem(Contexts contexts) {
        _contexts = contexts;
        _context = contexts.ui;
    }

    public void Initialize()
    {
        GameObject mainUiGo = Object.Instantiate(UiConfig.Instance.mainUi);

        mainUiGo.name = "MainUi";
        mainUiGo.transform.SetSiblingIndex(0);
        CreateMainUiRootEntity(mainUiGo);

        mainUiGo.SetActive(true);
    }

    private void CreateMainUiRootEntity(GameObject go)
    {
        UiEntity entity = _context.CreateEntity();
        entity.isMainUi = true;
        entity.isViewRoot = true;
        entity.isMainUiRoot = true;
        entity.AddView(go);
        go.Link(entity);
    }
}
