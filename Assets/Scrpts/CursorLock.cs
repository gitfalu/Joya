using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLock : MonoBehaviour
{
    private void Update()
    {
        // 左クリックを押した瞬間
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        // 左クリックを離した瞬間
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
