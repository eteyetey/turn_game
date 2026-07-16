using System.Collections;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class battleManager : MonoBehaviour
{
    //턴의 종류
    public enum BattleStatus{
        PlayerTurn,
        Selecting,
        EnemyTurn,
        Win,
        Lose
    }
    public Unit player;
    public Unit enemy;
    public TMP_Text turn;
    public BattleStatus status;

    public void Start()
    {
        status = BattleStatus.PlayerTurn;
    }
    public void Update()
    {
        turn.SetText(status.ToString());
    }

    //공격버튼 onClick() 에 연결할꺼임
    public void SelectAttack()
    {
        //플레이어의 턴이 아니라면 선택은 무효
        if(status!=BattleStatus.PlayerTurn)
        {
            return;
        }
        //선택중으로 상태 변경
        status = BattleStatus.Selecting;
    }

    //유닛셀렉션에서 유닛을 클릭하면 호출함
    public void Selectunit(Unit target)
    {
        //선택 턴이 아니라면 무효
        if (status != BattleStatus.Selecting)
        {
            return;
        }
        //선택한것이 적이 아니라면 무효
        if(target.type!=Unit.unitType.Enemy)
        {
            return ;
        }
        //선택한것이 죽어있으면 무효
        if(target.isDead)
        {
            return;
        }
        PlayerAttack(target);
    }
    private void PlayerAttack(Unit target)
    {
        //플레이어의 공격==적의 체력 감소
        target.TakeDamage(player.atk);

        if (target.isDead)
        {
            status=BattleStatus.Win;
            return;
        }
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        //적의 턴 시작
        status = BattleStatus.EnemyTurn;
        //1초 기다리기
        yield return new WaitForSeconds(1f);

        player.TakeDamage(enemy.atk);

        if(player.isDead)
        {
            status = BattleStatus.Lose;
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        //적의 공격이 끝나면 다시 플레이어턴
        status = BattleStatus.PlayerTurn;
    }
}
