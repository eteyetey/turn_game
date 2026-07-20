using System.Collections;
using TMPro;
using UnityEngine;

public class battleManager : MonoBehaviour
{
    // 턴의 종류
    public enum BattleStatus
    {
        PlayerTurn,
        Selecting,
        PlayerAttacking,
        EnemyTurn,
        Win,
        Lose
    }

    public Unit player;
    public Unit enemy;
    public TMP_Text turn;
    public BattleStatus status;

    private void Start()
    {
        status = BattleStatus.PlayerTurn;
    }

    private void Update()
    {
        turn.SetText(status.ToString());
    }

    // 공격 버튼 OnClick()에 연결
    public void SelectAttack()
    {
        // 플레이어 턴이 아니라면 선택 무효
        if (status != BattleStatus.PlayerTurn)
        {
            return;
        }

        status = BattleStatus.Selecting;
    }

    // unitSelection에서 유닛을 클릭하면 호출
    public void Selectunit(Unit target)
    {
        if (status != BattleStatus.Selecting)
        {
            return;
        }

        if (target.type != Unit.UnitType.Enemy)
        {
            return;
        }

        if (target.isDead)
        {
            return;
        }

        StartCoroutine(PlayerAttack(target));
    }

    private IEnumerator PlayerAttack(Unit target)
    {
        // 중복 클릭 방지
        status = BattleStatus.PlayerAttacking;

        // 공격 애니메이션 → 피해 → Hit/Death 애니메이션
        yield return StartCoroutine(player.Attack(target));

        if (target.isDead)
        {
            status = BattleStatus.Win;
            yield break;
        }

        yield return StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        status = BattleStatus.EnemyTurn;

        // 적 턴 시작 전 약간 대기
        yield return new WaitForSeconds(0.5f);

        // 적 공격 애니메이션 → 피해 → 플레이어 Hit/Death
        yield return StartCoroutine(enemy.Attack(player));

        if (player.isDead)
        {
            status = BattleStatus.Lose;
            yield break;
        }

        status = BattleStatus.PlayerTurn;
    }
}