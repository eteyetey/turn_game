using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("효과 설정")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lifeTime = 0.8f;

    private TMP_Text text;
    private Color originalColor;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        if (text != null)
        {
            originalColor = text.color;
        }
    }

    public void Setup(float damage)
    {
        if (text == null)
        {
            return;
        }

        text.text = Mathf.RoundToInt(damage).ToString();

        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifeTime)
        {
            elapsedTime += Time.deltaTime;

            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            float alpha = 1f - elapsedTime / lifeTime;

            text.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}