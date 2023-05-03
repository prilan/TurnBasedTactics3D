using UnityEngine;
using Entitas;

    public class MetaGameStateAnimatingComponentMonoBehaviour : BaseComponentMonoBehaviour {
        
        public override IComponent Component
        {
            get 
            { 
                return new GameStateAnimatingComponent {
                    
                };
            }
        }
    }
