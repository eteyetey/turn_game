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

    [Header("애니메이션 시간")]
    [SerializeField] private float attackHitDelay = 0.4f;
    [SerializeField] private float attackEndDelay = 0.4f;
    [SerializeField] private float hitAnimationTime = 0.5f;
    [SerializeField] private float deathAnimationTime = 1f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color originalColor;

    private void Awake()
    {
        hp = maxHp;
        isDead = false;

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

    // 공격 애니메이션부터 피해 처리까지 전부 담당
    public IEnumerator Attack(Unit target)
    {
        if (isDead || target == null || target.isDead)
        {
            yield break;
        }

        // 이전 트리거가 남아 있는 경우를 방지
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Death");
        animator.SetTrigger("Attack");

        // 공격 모션에서 실제 타격 시점까지 기다림
        yield return new WaitForSeconds(attackHitDelay);

        // 타격 시점에 피해 적용
        target.TakeDamage(atk);

        // 공격 모션의 남은 부분 대기
        yield return new WaitForSeconds(attackEndDelay);

        // 맞은 유닛의 피격 또는 사망 모션 대기
        if (target.isDead)
        {
            yield return new WaitForSeconds(target.deathAnimationTime);
        }
        else
        {
            yield return new WaitForSeconds(target.hitAnimationTime);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        float finalDamage = Mathf.Max(0f, damage - def);

        hp -= finalDamage;
        hp = Mathf.Max(0f, hp);

        if (hp <= 0f)
        {
            Die();
            return;
        }

        PlayHitAnimation();
    }

    private void PlayHitAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Death");
            animator.SetTrigger("Hit");
        }

        StartCoroutine(HitEffect());
    }

    private void Die()
    {
        isDead = true;
        hp = 0f;

        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Death");
        }
    }

    private IEnumerator HitEffect()
    {
        if (spriteRenderer == null)
        {
            yield break;
        }

        spriteRenderer.color = originalColor * 0.5f;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originalColor;
    }
}