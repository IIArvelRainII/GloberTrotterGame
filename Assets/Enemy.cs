using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using Update = Unity.VisualScripting.Update;

public class Enemy : MonoBehaviour
{
    public bool imAManiquiee;
    public int life = 100;
    public bool iGetHit;
    public bool alreadtHit;
    private Animator _anim;
    
    //Enemy Propeties
    public float speed = 5f;
    public int damage = 5;
    private Rigidbody2D _rbenemy;
    public float timeToChangeSide = 2;
    public float resetTimeToChangeSide = 2;
    public bool spriteSeeright = true;
    public bool iSeeRight = true;
    public bool iAlredyChangeSide = false;
    public bool imUnderAttack = false;
    public float timerImNotLongerUnderAttack = 5;
    public float timerImNotLongerUnderAttackReset = 5;
    public int vectorX, vectorY;
    
    //PlayerControllerScript
    public PlayerController playerControllerScript;
    
    
    private void Awake()
    {
        _rbenemy = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        iSeeRight = true;
    }

    private void Update()
    {
        ImNotLongerUnderAttackFunction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemyMovement();
    }
    

    void EnemyMovement()
    {
        if (spriteSeeright)
        {
            if (imAManiquiee == false)
            {
                if (iSeeRight)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    _rbenemy.AddForce(Vector3.right * speed, ForceMode2D.Force);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    _rbenemy.AddForce(Vector3.left * speed, ForceMode2D.Force);
                }

                if (iAlredyChangeSide == false && imUnderAttack == false)
                {
                    StartCoroutine("ChangeSide");
                }

            }
        }
        else
        {
            if (imAManiquiee == false)
            {
                if (iSeeRight)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    _rbenemy.AddForce(Vector3.right * speed, ForceMode2D.Force);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    _rbenemy.AddForce(Vector3.left * speed, ForceMode2D.Force);
                }

                if (iAlredyChangeSide == false && imUnderAttack == false)
                {
                    StartCoroutine("ChangeSide");
                }

            }
        }
    }

    public void GettingDamage(int damage)
    {
        if (imAManiquiee == false)
        {
            life -= damage;
            if (iSeeRight && playerControllerScript.imFacingRight)
            {
                _rbenemy.AddForce(new Vector2(vectorX,vectorY) , ForceMode2D.Impulse);
                imUnderAttack = true;
                iSeeRight = false;
                timerImNotLongerUnderAttack = timerImNotLongerUnderAttackReset;

            }

            if (iSeeRight && playerControllerScript.imFacingRight == false)
            {
                _rbenemy.AddForce(new Vector2(-vectorX,vectorY) , ForceMode2D.Impulse);
                imUnderAttack = true;
                iSeeRight = true;
                timerImNotLongerUnderAttack = timerImNotLongerUnderAttackReset;
            }

            if (iSeeRight == false && playerControllerScript.imFacingRight)
            {
                _rbenemy.AddForce(new Vector2(vectorX,vectorY) , ForceMode2D.Impulse);
                imUnderAttack = true;
                iSeeRight = false;
                timerImNotLongerUnderAttack = timerImNotLongerUnderAttackReset;

            }

            if (iSeeRight == false && playerControllerScript.imFacingRight == false)
            {
                _rbenemy.AddForce(new Vector2(-vectorX,vectorY) , ForceMode2D.Impulse);
                imUnderAttack = true;
                iSeeRight = true;
                timerImNotLongerUnderAttack = timerImNotLongerUnderAttackReset;
            }
            
        }
            
            _anim.SetBool("ImHit" , true);
            StartCoroutine("imNotHitAnymore");
    }

    void ImNotLongerUnderAttackFunction()
    {
        
        timerImNotLongerUnderAttack -= Time.deltaTime;
        if (timerImNotLongerUnderAttack <= 0)
        {
            imUnderAttack = false;
        }
    }

    IEnumerator imNotHitAnymore()
    {
        yield return new WaitForSeconds(0.1f);
        _anim.SetBool("ImHit", false);
    }

    IEnumerator ChangeSide()
    {
        yield return new WaitForSeconds(timeToChangeSide);

        if (imUnderAttack == false)
            {
                iSeeRight = false;
                iAlredyChangeSide = true;
                StartCoroutine("SetOriginalview");
            }
            
        
      

    }

    IEnumerator SetOriginalview()
    {
            yield return new WaitForSeconds(timeToChangeSide);
            if (imUnderAttack == false)
            {
                iSeeRight = true;
                iAlredyChangeSide = false;  
            }
           
            
        
    }
}
