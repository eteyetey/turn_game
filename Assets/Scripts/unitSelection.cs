using System.Collections;
using UnityEngine;

public class unitSelection : MonoBehaviour
{
    [Header("마우스 오버 효과")]
    [SerializeField]
    private Color hoverColor =
        new Color(0.55f, 0.55f, 0.55f, 1f);

    [SerializeField] private float blinkSpeed = 6f;
    [SerializeField] private float minimumStrength = 0.25f;

    private Unit unit;
    private battleManager bm;
    private SpriteRenderer spriteRenderer;

    private Color originalColor;
    private Coroutine hoverCoroutine;
    private bool isMouseOver;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        bm = FindFirstObjectByType<battleManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnMouseDown()
    {
        if (bm == null || unit == null)
        {
            return;
        }

        bm.Selectunit(unit);
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
        TryStartHoverEffect();
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        StopHoverEffect();
    }

    private void Update()
    {
        if (!isMouseOver)
        {
            return;
        }

        // 마우스를 올린 뒤 공격 선택 상태가 되었을 수도 있으므로 계속 검사
        if (CanShowHoverEffect())
        {
            if (hoverCoroutine == null)
            {
                TryStartHoverEffect();
            }
        }
        else
        {
            StopHoverEffect();
        }
    }

    private bool CanShowHoverEffect()
    {
        if (bm == null || unit == null || spriteRenderer == null)
        {
            return false;
        }

        return bm.CanSelectTarget(unit);
    }

    private void TryStartHoverEffect()
    {
        if (!CanShowHoverEffect())
        {
            return;
        }

        if (hoverCoroutine != null)
        {
            return;
        }

        hoverCoroutine = StartCoroutine(HoverBlinkEffect());
    }

    private IEnumerator HoverBlinkEffect()
    {
        while (isMouseOver && CanShowHoverEffect())
        {
            // 0~1 사이를 부드럽게 반복
            float wave = (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;

            // 완전히 원래 색으로 돌아가지 않도록 최소 효과 유지
            float strength = Mathf.Lerp(
                minimumStrength,
                1f,
                wave
            );

            spriteRenderer.color = Color.Lerp(
                originalColor,
                hoverColor,
                strength
            );

            yield return null;
        }

        RestoreOriginalColor();
        hoverCoroutine = null;
    }

    private void StopHoverEffect()
    {
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }

        RestoreOriginalColor();
    }

    private void RestoreOriginalColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private void OnDisable()
    {
        StopHoverEffect();
    }
}