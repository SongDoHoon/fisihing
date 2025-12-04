using UnityEngine;

public class AquariumSlotButton : MonoBehaviour
{
    public int slotIndex;  // ½½·Ô ¹øÈ£ (0,1,2)

    public void OnClickSlot()
    {
        AquariumManager.Instance.OnClickSlot(slotIndex);
        
    }
}
