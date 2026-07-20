using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType
    {
        Ally,
        Enemy
    }

    [Header("유닛 정보")]
    public float hp;
    public float maxHp = 100f;
    public float atk = 10f;
    public float def = 0f;
    public bool isDead;
    public UnitType type;

    [Header("애니메이션 컨트롤러")]
    [SerializeField] private RuntimeAnimatorController allyController;
    [SerializeField] private RuntimeAnimatorController enemyController;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color originalColor;

    private void Awake()
    {
        hp = maxHp;
        isDead = false;

        // SpriteRenderer와 Animator가 자식에 있으므로 InChildren 사용
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        ApplyAnimatorController();
    }

    private void ApplyAnimatorController()
    {
        if (animator == null)
        {
            Debug.LogError($"{name}의 자식에 Animator가 없음");
            return;
        }

        switch (type)
        {
            case UnitType.Ally:
                animator.runtimeAnimatorController = allyController;
                break;

            case UnitType.Enemy:
                animator.runtimeAnimatorController = enemyController;
                break;
        }
    }

    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(0f, damage - def);

        hp -= finalDamage;

        if (hp <= 0f)
        {
            hp = 0f;
            isDead = true;

            if (animator != null)
            {
                animator.SetTrigger("Death");
            }

            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        if (spriteRenderer == null)
        {
            yield break;
        }

        spriteRenderer.color = originalColor * 0.5f;
        transform.Translate(0f, 0.1f, 0f);

        yield return new WaitForSeconds(0.1f);

        transform.Translate(0f, -0.1f, 0f);
        spriteRenderer.color = originalColor;
    }
}