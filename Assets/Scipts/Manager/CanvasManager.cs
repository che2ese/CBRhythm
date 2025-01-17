using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public CanvasScaler canvasScaler;

    // 기준 화면 비율과 해상도를 설정합니다.
    public float baseAspectRatio = 16f / 9f; // 기본 화면 비율 (16:9)
    public Vector2 baseResolution = new Vector2(1920, 1080); // 기준 해상도

    private void Start()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }

        // 게임 시작 시 한 번만 비율 조정
        AdjustReferenceResolution();
    }

    private void AdjustReferenceResolution()
    {
        // 현재 화면의 가로세로 비율을 계산합니다.
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // 화면 비율에 따라 referenceResolution을 조정합니다.
        if (currentAspectRatio > baseAspectRatio)
        {
            // 화면이 더 넓은 경우 (가로 기준으로 맞춤)
            canvasScaler.referenceResolution = new Vector2(baseResolution.x, baseResolution.x / currentAspectRatio);
        }
        else
        {
            // 화면이 더 높은 경우 (세로 기준으로 맞춤)
            canvasScaler.referenceResolution = new Vector2(baseResolution.y * currentAspectRatio, baseResolution.y);
        }
    }
}
