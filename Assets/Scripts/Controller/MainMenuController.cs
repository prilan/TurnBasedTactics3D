using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Edit Mode")]
    public Transform editModePanel;
    public Button goToRunModeButton;
    public Toggle characterAToggle;
    public Toggle characterBToggle;
    public Toggle obstacleToggle;

    [Header("Run Mode")]
    public Transform runModePanel;
    public Button goToEditModeButton;
    public Button passTurnButton;

    private MetaContext _metaContext;
    private GameContext _gameContext;

    private bool isUiActive = true;

    private void Awake()
    {
        goToRunModeButton.onClick.AddListener(OnGoToRunModeButtonnClicked);
        characterAToggle.onValueChanged.AddListener(OnCharacterAToggleValueChanged);
        characterBToggle.onValueChanged.AddListener(OnCharacterBToggleValueChanged);
        obstacleToggle.onValueChanged.AddListener(OnObstacleToggleValueChanged);

        goToEditModeButton.onClick.AddListener(OnGoToEditModeButtonClicked);
        passTurnButton.onClick.AddListener(OnPassTurnButtonClicked);
    }

    private void OnEnable()
    {
        _metaContext = Contexts.sharedInstance.meta;
        _gameContext = Contexts.sharedInstance.game;
    }

    private void Update()
    {
        SetUiActive(!_metaContext.isGameStateAnimating);
    }

    private void OnGoToRunModeButtonnClicked()
    {
        _metaContext.isGameStateInEdit = false;
        _metaContext.isGameStateInGame = true;
    }

    private void OnCharacterAToggleValueChanged(bool isOn)
    {
        if (isOn) {
            _gameContext.SetEditActiveInstrument(EditActiveInstrument.CharacterA);
        }
    }

    private void OnCharacterBToggleValueChanged(bool isOn)
    {
        if (isOn) {
            _gameContext.SetEditActiveInstrument(EditActiveInstrument.CharacterB);
        }
    }

    private void OnObstacleToggleValueChanged(bool isOn)
    {
        if (isOn) {
            _gameContext.SetEditActiveInstrument(EditActiveInstrument.Obstacle);
        }
    }

    private void OnGoToEditModeButtonClicked()
    {
        _metaContext.isGameStateInEdit = true;
        _metaContext.isGameStateInGame = false;
    }

    public void SetEditModeActive(bool isActive)
    {
        editModePanel.gameObject.SetActive(isActive);
    }

    public void SetRunModeActive(bool isActive)
    {
        runModePanel.gameObject.SetActive(isActive);
    }

    private void OnPassTurnButtonClicked()
    {
        GameEntity[] characterEntities = _gameContext.GetGroup(GameMatcher.Character).GetEntities();

        foreach (GameEntity characterEntity in characterEntities) {
            characterEntity.isActiveCharacter = !characterEntity.isActiveCharacter;
            characterEntity.isSelected = !characterEntity.isSelected;
        }
    }

    private void SetUiActive(bool isActive)
    {
        if (isActive && isUiActive)
            return;
        if (!isActive && !isUiActive)
            return;

        passTurnButton.interactable = isActive;
        goToEditModeButton.interactable = isActive;

        isUiActive = isActive;
    }
}
