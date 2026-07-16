using System.Collections;
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
    private SpriteRenderer sp;
    private Color color;
    private void Awake()
    {
        hp = maxhp;
        isDead = false;
        sp = GetComponent<SpriteRenderer>();
        color = sp.color;
    }
   public  void TakeDamage(float damage)
    {
        hp -= damage;
        StartCoroutine(HitEffect());
        if (hp <= 0f){
            hp = 0;
            isDead=true;
            }

    }

    private IEnumerator HitEffect()
    {
        sp.color = color * 0.5f;
        transform.Translate(new Vector3(0, 0.1f, 0));
        yield return new WaitForSeconds(0.1f);
        transform.Translate(new Vector3(0, -0.1f, 0));
        sp.color = color;
    }

}
