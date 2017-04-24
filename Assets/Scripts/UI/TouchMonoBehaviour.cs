using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Extension functionality for touch-dependant UI elements.
/// <para>Automatically informs TouchInput to cancel operations when interacting with UI elements.</para>
/// <para>USAGE: Inherit from this class when writing scripts using UI elements.</para>
/// <para>WARNING: Only works when the object or child objects have Button components.</para>
/// </summary>
public abstract class TouchMonoBehaviour : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        // Cancel other touch so you don't acidentally perform a player action.
        TouchInput.CancelTouchOperations();
    }

    public void OnPointerDown(PointerEventData data)
    {
        // Cancel other touch so you don't acidentally perform a player action.
        TouchInput.CancelTouchOperations();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // Cancel other touch so you don't acidentally perform a player action.
        TouchInput.CancelTouchOperations();
    }

    public void OnPointerExit(PointerEventData data)
    {
        // Cancel other touch so you don't acidentally perform a player action.
        TouchInput.CancelTouchOperations();
    }

    public void OnPointerUp(PointerEventData data)
    {
        // Cancel other touch so you don't acidentally perform a player action.
        TouchInput.CancelTouchOperations();
    }

    public void OnDrag(PointerEventData data)
    {
        // Cancel other touch so you don't acidentally perform a player action.
        TouchInput.CancelTouchOperations();
    }
}
