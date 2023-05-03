using System.Collections.Generic;
using System.Linq;
using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace Sources.Systems.Logic
{
    public sealed class EditInstrumentHandleSystem : ReactiveSystem<InputEntity>, IInitializeSystem
    {
        readonly InputContext _context;
        readonly GameContext _gameContext;
        readonly MetaContext _metaContext;
        private IGroup<GameEntity> _cellEntitiesGroup;

        public EditInstrumentHandleSystem(Contexts contexts) : base(contexts.input) {
            _context = contexts.input;
            _gameContext = contexts.game;
            _metaContext = contexts.meta;
            _cellEntitiesGroup = contexts.game.GetGroup(GameMatcher.CellPosition);
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) {
            return context.CreateCollector(InputMatcher.AllOf(InputMatcher.ScreenPoint, InputMatcher.MouseEvent));
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasScreenPoint;
        }

        public void Initialize()
        {
            //_gameContext.SetEditActiveInstrument(EditActiveInstrument.Obstacle);
        }

        protected override void Execute(List<InputEntity> entities)
        {
            if (_metaContext.isGameStateInGame)
                return;

            InputEntity entity = entities.First();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(entity.screenPoint.value), out hit, Mathf.Infinity)) {

                GameObject hoverTargetGo = null;

                if (hit.collider != null) {
                    hoverTargetGo = hit.collider.gameObject;
                } else if (hit.rigidbody != null) {
                    hoverTargetGo = hit.rigidbody.gameObject;
                }

                if (hoverTargetGo == null) {
                    return;
                }

                EntityLink clickedEntityLink = hoverTargetGo.GetComponentInParent<EntityLink>();

                if (clickedEntityLink == null) {
                    return;
                }

                foreach (GameEntity cellEntity in _cellEntitiesGroup) {
                    cellEntity.isHovered = false;
                }

                GameEntity hoveredEntity = (GameEntity)clickedEntityLink.entity;

                if (hoveredEntity.hasCellPosition) {
                    hoveredEntity.isHovered = true;
                }

            }
        }
    }
}
