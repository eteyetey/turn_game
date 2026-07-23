using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public enum BackgroundType
    {
        Castle,
        Forest,
        Dungeon
    }

    [Header("배경을 표시할 SpriteRenderer")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [Header("배경 이미지")]
    [SerializeField] private Sprite castleBackground;
    [SerializeField] private Sprite forestBackground;
    [SerializeField] private Sprite dungeonBackground;

    [Header("현재 배경")]
    [SerializeField] private BackgroundType currentBackground;

    private BackgroundType previousBackground;

    private void Start()
    {
        previousBackground = currentBackground;
        ChangeBackground(currentBackground);
    }

    private void Update()
    {
        // 게임 실행 중 인스펙터에서 값이 바뀌었는지 확인
        if (previousBackground == currentBackground)
        {
            return;
        }

        previousBackground = currentBackground;
        ChangeBackground(currentBackground);
    }

    public void ChangeBackground(BackgroundType backgroundType)
    {
        currentBackground = backgroundType;

        switch (backgroundType)
        {
            case BackgroundType.Castle:
                backgroundRenderer.sprite = castleBackground;
                break;

            case BackgroundType.Forest:
                backgroundRenderer.sprite = forestBackground;
                break;

            case BackgroundType.Dungeon:
                backgroundRenderer.sprite = dungeonBackground;
                break;

            default:
                Debug.LogWarning($"등록되지 않은 배경 타입: {backgroundType}");
                break;
        }
    }
}