using UnityEngine;

public abstract class AbstractController : MonoBehaviour
{
    public abstract bool WasPressedThisFrame(PlayerInput playerInput);
    public abstract bool IsPressed(PlayerInput playerInput);
    public abstract bool HasAnyKeyPressed();
}