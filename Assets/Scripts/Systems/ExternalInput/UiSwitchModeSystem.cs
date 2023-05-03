using Entitas;
using System.Collections.Generic;

namespace Sources.Systems.ExternalInput
{
    public class UiSwitchModeSystem : ReactiveSystem<MetaEntity>, IInitializeSystem
    {
        private Contexts _contexts;
        private MetaContext _metaContext;
        private UiEntity _mainUiEntity;
        private ClearMoveHintNoWayService _clearMoveHintNoWayService;

        public UiSwitchModeSystem(Contexts contexts) : base(contexts.meta)
        {
            _contexts = contexts;
            _metaContext = contexts.meta;
            _clearMoveHintNoWayService = new ClearMoveHintNoWayService(contexts);
        }

        protected override ICollector<MetaEntity> GetTrigger(IContext<MetaEntity> context)
        {
            return context.CreateCollector(MetaMatcher.AnyOf(MetaMatcher.GameStateInEdit, MetaMatcher.GameStateInGame));
        }

        protected override bool Filter(MetaEntity entity)
        {
            return true;
        }

        public void Initialize()
        {
            _mainUiEntity = _contexts.ui.mainUiRootEntity;
        }

        protected override void Execute(List<MetaEntity> entities)
        {
            if (Contexts.sharedInstance.meta.isGameStateAnimating)
                return;

            UiInteractionBehaviour uiInteractionBehaviour = _mainUiEntity.view.gameObject.GetComponent<UiInteractionBehaviour>();
            MainMenuController mainMenuController = uiInteractionBehaviour.GetComponent<MainMenuController>();

            if (_metaContext.isGameStateInEdit) {
                _clearMoveHintNoWayService.ClearMoveHint();
                _clearMoveHintNoWayService.ClearNoWay();
                SetEditMode(mainMenuController);
            } else if (_metaContext.isGameStateInGame) {
                SetRunMode(mainMenuController);
            } else {
                // По умолчанию редактирование
                SetEditMode(mainMenuController);
            }
        }

        private void SetEditMode(MainMenuController mainMenuController)
        {
            mainMenuController.SetEditModeActive(true);
            mainMenuController.SetRunModeActive(false);
        }

        private void SetRunMode(MainMenuController mainMenuController)
        {
            mainMenuController.SetEditModeActive(false);
            mainMenuController.SetRunModeActive(true);
        }
    }
}