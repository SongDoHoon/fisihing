using UnityEngine;

public class CloseTargetObject : MonoBehaviour
{
    public GameObject target;

    public void Close()
    {
        Debug.Log("X 버튼 눌림!"); // 로그로 확인
        if (target != null)
        {
            target.SetActive(false);
            Debug.Log("타겟 비활성화 완료!");
        }
        else
        {
            Debug.LogWarning("Target이 연결되지 않았습니다!");
        }
    }
}
