using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Sources.Systems.ExternalInput
{
    public sealed class DragSelectionSystem : ReactiveSystem<InputEntity>, IInitializeSystem {

        readonly Contexts _contexts;
        readonly InputContext _context;
        private IGroup<GameEntity> _selectedEntitiesGroup;
        private IGroup<InputEntity> _keyEventGroup;

        public DragSelectionSystem(Contexts contexts) : base(contexts.input)
        {
            _contexts = contexts;
            _context = contexts.input;
            _selectedEntitiesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected));
            _keyEventGroup = contexts.input.GetGroup(InputMatcher.AllOf(InputMatcher.KeyEvent, InputMatcher.KeyHeld));
        }
    
    
        public void Initialize()
        {
            _context.SetDragSelectionData(Vector2.zero, Vector2.zero, Vector2.zero);
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) {
            return context.CreateCollector(
                InputMatcher.AllOf(
                    InputMatcher.ScreenPoint, 
                    InputMatcher.MouseEvent 
                ).AnyOf(InputMatcher.LeftMouseButtonDown, InputMatcher.LeftMouseButtonHeld, InputMatcher.LeftMouseButtonUp)
            );
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasScreenPoint;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            InputEntity dragSelectionDataEntity = _context.dragSelectionDataEntity;
        
            foreach (InputEntity entity in entities)
            {
                if (entity.isLeftMouseButtonDown)
                {
                    dragSelectionDataEntity.dragSelectionData.mouseDownScreenPoint = entity.screenPoint.value;
                }
                else if (entity.isLeftMouseButtonHeld) {
                    dragSelectionDataEntity.dragSelectionData.mouseHeldScreenPoint = entity.screenPoint.value;
                }
                else if (entity.isLeftMouseButtonUp) {
                    dragSelectionDataEntity.dragSelectionData.mouseUpScreenPoint = entity.screenPoint.value;
                }
            }


            dragSelectionDataEntity.ReplaceComponent(InputComponentsLookup.DragSelectionData, dragSelectionDataEntity.dragSelectionData);
            
            DragSelectionDataComponent dragSelectionDataComponent = dragSelectionDataEntity.dragSelectionData;
            if (
                dragSelectionDataComponent.mouseUpScreenPoint == Vector2.zero
            ) {
                return;
            }
        }
    }
}
