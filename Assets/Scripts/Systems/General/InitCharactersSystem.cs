using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Utility;
using Object = UnityEngine.Object;

public class InitCharactersSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public InitCharactersSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        List<ViewableEntityInitializer> initializableEntities = Object.FindObjectsOfType<ViewableEntityInitializer>().ToList();

        int characterIndex = 0;
        for (int index = 0; index < initializableEntities.Count; index++) {
            ViewableEntityInitializer initializableEntity = initializableEntities[index];
            List<CharacterComponentMonoBehaviour> characterMonoComponents = initializableEntity.GetComponents<CharacterComponentMonoBehaviour>().ToList();

            if (!characterMonoComponents.Any())
                continue;

            CharacterComponentMonoBehaviour characterMonoComponent = characterMonoComponents[0];

            CharacterComponent characterComponent = (CharacterComponent)characterMonoComponent.Component;

            GameObject characterGo = initializableEntity.gameObject;
            Transform characterCollider = characterGo.transform.Find("CharacterCylinder");

            if (CommonUtility.RaycastWorldPositionToCell(characterCollider.position, out GameEntity cellEntity)) {

                CellPositionComponent cellPositionComponent = (CellPositionComponent)cellEntity.GetComponent(GameComponentsLookup.CellPosition);
                Int2 cellPosition = cellPositionComponent.value;

                characterComponent.cellPosition = cellPosition;

                if (characterIndex == 0) {
                    characterComponent.characterPlayer = ActiveCharacterPlayer.CharacterA;
                } else if (characterIndex == 1) {
                    characterComponent.characterPlayer = ActiveCharacterPlayer.CharacterB;
                }

                characterIndex++;
            }
        }
    }
}
