using Entitas;

public class CellPositionComponentMonoBehaviour : BaseComponentMonoBehaviour
{
    public Int2 value;
    private CellPositionComponent component;

    public override IComponent Component
    {
        get
        {
            if (component == null) {
                return component = new CellPositionComponent {
                    value = value
                };
            }

            return component;
        }
    }
}
