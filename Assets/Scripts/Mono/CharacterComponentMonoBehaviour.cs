using Entitas;

public class CharacterComponentMonoBehaviour : BaseComponentMonoBehaviour
{
    public Int2 cellPosition;
    private CharacterComponent component;

    public override IComponent Component
    {
        get
        {
            if (component == null) {
                return component = new CharacterComponent {
                    cellPosition = cellPosition
                };
            }

            return component;
        }
    }
}
