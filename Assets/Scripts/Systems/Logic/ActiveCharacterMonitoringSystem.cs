using Entitas;
using System.Collections.Generic;

namespace Sources.Systems.Logic
{
    public class ActiveCharacterMonitoringSystem : ReactiveSystem<MetaEntity>
    {
        readonly Contexts _contexts;
        readonly MetaContext _metaContext;
        private IGroup<GameEntity> _characterEntitiesGroup;

        public ActiveCharacterMonitoringSystem(Contexts contexts) : base(contexts.meta)
        {
            _contexts = contexts;
            _metaContext = contexts.meta;
            _characterEntitiesGroup = contexts.game.GetGroup(GameMatcher.Character);
        }

        protected override ICollector<MetaEntity> GetTrigger(IContext<MetaEntity> context)
        {
            return context.CreateCollector(MetaMatcher.AnyOf(MetaMatcher.GameStateInEdit, MetaMatcher.GameStateInGame));
        }

        protected override bool Filter(MetaEntity entity)
        {
            return true;
        }

        protected override void Execute(List<MetaEntity> entities)
        {
            if (Contexts.sharedInstance.meta.isGameStateAnimating)
                return;

            GameEntity[] gemEnities = _characterEntitiesGroup.GetEntities();

            if (_metaContext.isGameStateInEdit) {
                SetEditMode(gemEnities);
            } else if (_metaContext.isGameStateInGame) {
                SetRunMode(gemEnities);
            } else {
                SetEditMode(gemEnities);
            }
        }

        private static void SetEditMode(GameEntity[] gemEnities)
        {
            foreach (GameEntity gameEntity in gemEnities) {
                gameEntity.isActiveCharacter = false;
                gameEntity.isSelected = false;
            }
        }

        private static void SetRunMode(GameEntity[] gemEnities)
        {
            for (int index = 0; index < gemEnities.Length; index++) {
                GameEntity gameEntity = gemEnities[index];
                if (index == 0) {
                    gameEntity.isActiveCharacter = true;
                    gameEntity.isSelected = true;
                }
            }
        }
    }
}