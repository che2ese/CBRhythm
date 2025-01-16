using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public CanvasScaler canvasScaler;

    // ���� ȭ�� ������ �ػ󵵸� �����մϴ�.
    public float baseAspectRatio = 16f / 9f; // �⺻ ȭ�� ���� (16:9)
    public Vector2 baseResolution = new Vector2(1920, 1080); // ���� �ػ�

    private void Start()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }

        AdjustReferenceResolution();
    }

    private void AdjustReferenceResolution()
    {
        // ���� ȭ���� ���μ��� ������ ����մϴ�.
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // ȭ�� ������ ���� referenceResolution�� �����մϴ�.
        if (currentAspectRatio > baseAspectRatio)
        {
            // ȭ���� �� ���� ��� (���� �������� ����)
            canvasScaler.referenceResolution = new Vector2(baseResolution.x, baseResolution.x / currentAspectRatio);
        }
        else
        {
            // ȭ���� �� ���� ��� (���� �������� ����)
            canvasScaler.referenceResolution = new Vector2(baseResolution.y * currentAspectRatio, baseResolution.y);
        }
    }

    private void Update()
    {
        // ȭ�� ũ�Ⱑ ����Ǿ����� �����ϰ� ������Ʈ�մϴ�.
        AdjustReferenceResolution();
    }
}
