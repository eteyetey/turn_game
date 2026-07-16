using UnityEngine;

public class Unit : MonoBehaviour


{

    public enum unitType
    {
        Ally,
        Enemy
    }
    public float hp;
    public float maxhp = 100;
    public float atk = 10;
    public float def = 0;
    public bool isDead;
    public unitType type;
    private void Awake()
    {
        hp = maxhp;
        isDead = false;
    }
   public  void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0f){
            hp = 0;
            isDead=true;
            }

    }
}
