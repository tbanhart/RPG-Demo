using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] public float Range;
    [SerializeField] float Cooldown;

    RPGStat Life = new RPGStat(20f);
    RPGStat Stamina = new RPGStat(10f);
    RPGStat WeaponDamage = new RPGStat(5f);
    RPGStat AttackSpeed = new RPGStat(1f);

    AttackStage attackStage;


    public AttackType attackType;

    private void Awake() {
        attackType = new AttackType(){
            Name = "Unarmed",
            Windup = 10f * Time.deltaTime * 2f,
            Cooldown = 12f * Time.deltaTime * 2f
        };
    }

    public bool DoAttack(Combat target){
        //Debug.Log(Cooldown);
        Debug.Log(attackStage);
        switch(attackStage){
            case AttackStage.DEFAULT:
                Debug.Log("Starting windup");
                Cooldown = attackType.Windup;
                SetStage(AttackStage.WINDUP);
                break;
            case AttackStage.WINDUP:
                Debug.Log("Winding up");
                Cooldown -=  1f * Time.deltaTime;
                if(Cooldown <= 0f){
                    SetStage(AttackStage.DAMAGE);
                } break;
            case AttackStage.DAMAGE:
                Debug.Log("Did some damage");
                Cooldown = attackType.Cooldown;
                SetStage(AttackStage.COOLDOWN);
                var isdead = target.TakeDamage(WeaponDamage.Current);
                if (isdead == true) {
                    Debug.Log("Target is dead");
                    SetStage(AttackStage.DEFAULT); 
                    return true;
                }
                else{ 
                    SetStage(AttackStage.COOLDOWN);
                }
                break;
            case AttackStage.COOLDOWN:
                Debug.Log("Winding down");
                Cooldown -= 1f * Time.deltaTime;
                if(Cooldown <= 0f){
                    SetStage(AttackStage.DEFAULT);
                } break;            
        }
        return false;
    }    

    public bool TakeDamage(float damage){
        Life.Current -= damage;
        Debug.Log(this.gameObject + " is taking damage, life remaining is " + Life.Current);
        if(Life.Current <= 0f)
            return true;
        else return false;
    }    

    public void Kill(string source){
        Debug.Log(this.name + " was killed by " + source);
        Destroy(this.gameObject);
    }

    void SetStage(AttackStage stage){
        Debug.Log("Setting stage to " + stage);
        attackStage = stage;
    }

    enum AttackStage{DEFAULT, WINDUP, DAMAGE, COOLDOWN}
}
