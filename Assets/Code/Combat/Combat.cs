using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs rework - I like the system but it doesn't really work with the direction I'm headed

public class Combat : MonoBehaviour
{/*
    [SerializeField] public float Range;
    [SerializeField] public float Cooldown;

    RPGStat Life = new RPGStat(10f);
    RPGStat Stamina = new RPGStat(10f);
    RPGStat WeaponDamage = new RPGStat(5f);
    RPGStat AttackSpeed = new RPGStat(1f);

    public AttackStage attackStage;

    public AttackType attackType;

    private void Awake() {
        
    }

    public void StartAttack(){
        Cooldown = 0f;
        attackStage = AttackStage.START;
    }

    public bool DoAttack(Combat target){
        //Debug.Log(Cooldown);
        Debug.Log(attackStage);
        switch(attackStage){
            case AttackStage.START:
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
                //var isdead = target.TakeDamage(WeaponDamage._current);
                if (isdead == true) {
                    Debug.Log("Target is dead");
                    SetStage(AttackStage.START); 
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
                    SetStage(AttackStage.START);
                } break;            
        }
        return false;
    }    

    public bool TakeDamage(float damage){
        //Life._current -= damage;
        //Debug.Log(this.gameObject + " is taking damage, life remaining is " + Life._current);
        /*if(Life._current <= 0f)
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

    public void ProgressAttack(){}

*/}