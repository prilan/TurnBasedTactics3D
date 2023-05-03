using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Utility;

namespace Sources.Systems.ExternalInput
{
    public sealed class ClickMoveCharacterSystem : ReactiveSystem<InputEntity> {

        readonly GameContext _gameContext;
        readonly MetaContext _metaContext;
        private IGroup<GameEntity> _characterEntitiesGroup;
        private IGroup<InputEntity> _keyEventGroup;

        public ClickMoveCharacterSystem(Contexts contexts) : base(contexts.input) {
            _gameContext = contexts.game;
            _metaContext = contexts.meta;
            _characterEntitiesGroup = contexts.game.GetGroup(GameMatcher.Character);
            _keyEventGroup = contexts.input.GetGroup(InputMatcher.AllOf(InputMatcher.KeyEvent, InputMatcher.KeyHeld));
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) {
            return context.CreateCollector(InputMatcher.AllOf(InputMatcher.ScreenPoint, InputMatcher.MouseEvent, InputMatcher.LeftMouseButtonUp).NoneOf(InputMatcher.MouseOverUi));
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasScreenPoint;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            if (_metaContext.isGameStateInEdit)
                return;

            InputEntity addToSelectionKeyEvent = _keyEventGroup.GetEntities().SingleOrDefault(e => e.keyEvent.value.keyCode == KeyCode.LeftShift);
            bool isAddToSelectionKeyHeld = addToSelectionKeyEvent != null && addToSelectionKeyEvent.isKeyHeld;
            InputEntity inputMouseEntity = entities.Single();

            if (CommonUtility.RaycastScreenPointToCell(inputMouseEntity.screenPoint.value, out GameEntity clickedCellEntity)) {

                GameEntity characterEntity = _characterEntitiesGroup.GetEntities().Where(character => character.isActiveCharacter).First();
                CharacterComponent characterComponent = (CharacterComponent)characterEntity.GetComponent(GameComponentsLookup.Character);
                Vector2 startCharacterPosition = characterComponent.cellPosition;

                List<Int2> pathPositions = _gameContext.characterPathEntity.CharacterPath.pathPositions;
                if (pathPositions.Any()) {
                    characterEntity.AddMoveCharacter(pathPositions);

                    _metaContext.isGameStateAnimating = true;
                }
            }
        }
    }
}
