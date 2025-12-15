using UnityEngine;

public class DoorLockState : MonoBehaviour
{
    public bool isLocked = true;

    public void Unlock()
    {
        isLocked = false;
        Debug.Log("🔓 Door unlocked!");
    }
}
