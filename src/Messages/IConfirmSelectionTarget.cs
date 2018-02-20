using UnityEngine.EventSystems;

public interface IConfirmSelectionTarget : IEventSystemHandler
{
    void ConfirmSelection();
}
