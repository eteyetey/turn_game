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

    [Header("공격 이동 설정")]
    [SerializeField] private float dashSpeed = 200f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float afterAttackDelay = 0.15f;

    [Header("데미지 텍스트")]
    [SerializeField] private DamageText damageTextPrefab;
    [SerializeField] private Vector3 damageTextOffset = new Vector3(0f, 2f, 0f);

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
    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab == null)
        {
            return;
        }

        Vector3 spawnPosition = transform.position + damageTextOffset;

        spawnPosition.x += Random.Range(-0.2f, 0.2f);

        DamageText damageText = Instantiate(
            damageTextPrefab,
            spawnPosition,
            Quaternion.identity
        );

        damageText.Setup(damage);
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

    public IEnumerator Attack(Unit target)
    {
        if (isDead || target == null || target.isDead)
        {
            yield break;
        }

        Vector3 startPosition = transform.position;

        Vector3 direction =
            (target.transform.position - transform.position).normalized;

        Vector3 attackPosition =
            target.transform.position - direction * attackDistance;

        attackPosition.y = startPosition.y;
        attackPosition.z = startPosition.z;

        // 상대 앞으로 이동
        yield return StartCoroutine(
            MoveToPosition(attackPosition)
        );

        if (animator != null)
        {
            animator.ResetTrigger("Hit");
            animator.ResetTrigger("Death");
            animator.SetTrigger("Attack");
        }

        // 공격이 실제로 맞는 시점
        yield return new WaitForSeconds(attackHitDelay);

        target.TakeDamage(atk);

        // 공격 모션 마무리
        yield return new WaitForSeconds(attackEndDelay);

        if (target.isDead)
        {
            yield return new WaitForSeconds(target.deathAnimationTime);
        }
        else
        {
            yield return new WaitForSeconds(target.hitAnimationTime);
        }

        yield return new WaitForSeconds(afterAttackDelay);

        // 제자리로 복귀
        yield return StartCoroutine(
            MoveToPosition(startPosition)
        );
    }

    private IEnumerator MoveToPosition(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                dashSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = destination;
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

        ShowDamageText(finalDamage);

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
}