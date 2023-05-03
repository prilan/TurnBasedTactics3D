using System;
using UnityEngine;
using Utility;
using System.Reflection;
using Entitas;
using Entitas.Unity;

public class ObjectInstantiatorBehaviour : MonoSingleton<ObjectInstantiatorBehaviour>
{
    [SerializeField] private Transform charactersTransform;
    [SerializeField] private Transform obstaclesTransform;

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject obstaclePrefab;

    private Contexts _contexts;
    private GameContext _gameContext;
    private IGroup<GameEntity> _characterEntitiesGroup;
    private IGroup<GameEntity> _obstacleEntitiesGroup;

    private void OnEnable()
    {
        _contexts = Contexts.sharedInstance;
        _gameContext = _contexts.game;
        _characterEntitiesGroup = _contexts.game.GetGroup(GameMatcher.Character);
        _obstacleEntitiesGroup = _contexts.game.GetGroup(GameMatcher.Obstacle);
    }

    public void ReplaceCharacter(Int2 cellPosition, EditActiveInstrument editInstrumentCharacter)
    {
        GameEntity[] characterEntities = _characterEntitiesGroup.GetEntities();

        ActiveCharacterPlayer activeCharacterPlayer = CommonUtility.ConvertToCharacterPlayerFromEditInstrument(editInstrumentCharacter);

        foreach (GameEntity characterEntity in characterEntities) {
            CharacterComponent characterComponent = (CharacterComponent)characterEntity.GetComponent(GameComponentsLookup.Character);
            if (characterComponent.characterPlayer.Equals(activeCharacterPlayer)) {
                GameObject characterGo = characterEntity.view.gameObject;
                Vector3 position = CommonUtility.CalculateTransformPositionByCellposition(cellPosition);
                characterGo.transform.position = position;
                characterComponent.cellPosition = cellPosition;
                break;
            }
        }
    }

    public void CreateOrRemoveObstacle(Int2 cellPosition)
    {
        GameEntity[] obstacleEntities = _obstacleEntitiesGroup.GetEntities();

        GameEntity obstacleExistingEntity = null;
        foreach (GameEntity obstacleEntity in obstacleEntities) {
            ObstacleComponent obstacleComponent = (ObstacleComponent)obstacleEntity.GetComponent(GameComponentsLookup.Obstacle);
            if (obstacleComponent.cellPosition.Equals(cellPosition)) {
                obstacleExistingEntity = obstacleEntity;
                break;
            }
        }

        if (obstacleExistingEntity != null) {
            // Можно сделать через ObjectPool
            obstacleExistingEntity.view.gameObject.SetActive(false);
            obstacleExistingEntity.Destroy();
        } else {
            CreateObstacle(cellPosition);
        }
    }

    private void CreateObstacle(Int2 cellPosition)
    {
        GameObject obstacleGo = Instantiate(obstaclePrefab, obstaclesTransform);
        obstacleGo.transform.position = CommonUtility.CalculateTransformPositionByCellposition(cellPosition) + new Vector3(0, CommonConsts.ObstacleSize / 2, 0);

        ViewableEntityInitializer initializableEntity = obstacleGo.GetComponent<ViewableEntityInitializer>();
        ObstacleComponentMonoBehaviour obstacleComponentMonoBehaviour = obstacleGo.GetComponent<ObstacleComponentMonoBehaviour>();

        Entity entity = (Entity)_gameContext.GetType().InvokeMember("CreateEntity", BindingFlags.InvokeMethod, null, _gameContext, null);

        string componentLookupClassName = _gameContext.contextInfo.name + "ComponentsLookup";
        Type[] componentTypes = (Type[])Type.GetType(componentLookupClassName).GetField("componentTypes", BindingFlags.Public | BindingFlags.Static).GetValue(null);

        IComponent viewComponent = new ViewComponent { gameObject = initializableEntity.gameObject };
        int viewComponentIndex = Array.IndexOf(componentTypes, viewComponent.GetType());
        entity.AddComponent(viewComponentIndex, viewComponent);

        ObstacleComponent component = (ObstacleComponent)obstacleComponentMonoBehaviour.Component;
        int componentIndex = Array.IndexOf(componentTypes, component.GetType());

        entity.AddComponent(componentIndex, component);

        component.cellPosition = cellPosition;

        Destroy(obstacleComponentMonoBehaviour);

        initializableEntity.gameObject.Link(entity);
        Destroy(initializableEntity);
    }
}
