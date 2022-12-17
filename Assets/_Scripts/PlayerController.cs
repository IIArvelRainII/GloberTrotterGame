using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    public GameObject playerGameobject; // player GameObject variable
    private Rigidbody2D _rigibody2D; // rigibody2D variable
    private Animator _animator; // private Animator variable

    //Movement Propeties
    private Vector2 movement; // vector2 variable
    public float speed = 5f; // Speed of the player
    public float maxSpeed = 8;
    public float delayOfIdle; // delay
    public bool imFacingRight = true;

    // Jumping Propeties
    public bool grounded = true;
    public float jumpForce = 2;
    public LayerMask whatIsGround;
    public Transform origenCheckPoint;
    public float radius = 2;
    public float timeOfStartFalling = 1f;

    // Attack Propeties
    private float nextAttackTime = 1f; // Delay of your nextAttack
    public int AttackStage = 0; // Tells in what Attack animation are you on 
    private bool Attacking = false; // bolean telling if you are attacking or not
    public float attackRate = 2f; // attack rate
    private float delayToNeutral = 0.8f;
    public float maxDelayOfNeutral = 0.8f;
    public Transform attackPoint; // start of attack Physics collider
    public float radiusAttack = 2f; // radius of attack Physics collider
    public LayerMask hittable; // State the layers that you can hit
    public int Damage = 20;

    
    



    private void Awake()
    {
        
        _rigibody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
      
        speed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        AttackAnimation();
    }

    private void FixedUpdate()
    {
        Movement();
        Jumping();
    }

    private void Movement()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), 0);
        if(movement.x > 0)
        {
            imFacingRight = true;
            playerGameobject.transform.localScale = new Vector3(1, 1, 1);
            _animator.SetBool("ImMoving", true);
            _rigibody2D.velocity = new Vector2(Mathf.Clamp(_rigibody2D.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(_rigibody2D.velocity.y, -maxSpeed, maxSpeed));
            _rigibody2D.AddForce(Vector2.right * speed, ForceMode2D.Force);


        }
        if(movement.x < 0)
        {
            imFacingRight = false;
            playerGameobject.transform.localScale = new Vector3(-1, 1, 1);
            _animator.SetBool("ImMoving", true);
            _rigibody2D.velocity = new Vector2(Mathf.Clamp(_rigibody2D.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(_rigibody2D.velocity.y, -maxSpeed, maxSpeed));
            _rigibody2D.AddForce(Vector2.left * speed, ForceMode2D.Force);
        }
        if(movement.x == 0)
        {
            _animator.SetBool("ImMoving", false);
            

        }
    }

    private void Jumping()
    {
        grounded = Physics2D.OverlapCircle(origenCheckPoint.position, radius, whatIsGround);
        if (grounded)
        {          
            _animator.SetBool("Grounded", true);
            _animator.SetBool("ImFalling", false);
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && grounded)
        {
            _animator.SetBool("Grounded", false);
            _rigibody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(StartFalling());
        }
        IEnumerator StartFalling()
        {
            yield return new WaitForSeconds(timeOfStartFalling);
            _animator.SetBool("ImFalling", true);
        }
    }

    private void AttackAnimation()
    {
        if (Time.time >= nextAttackTime) // if the time is grater or equal than the next attack time
        {
            if (Input.GetKey(KeyCode.Space))
            {
                nextAttackTime = Time.time + 1f / attackRate; // restart the nextAttackTime delay
                AttackStage++; // +1 on attack stage
                _animator.SetBool("Attacking", true);
                if (AttackStage == 1)
                {
                    _animator.SetInteger("AttackNumber", 1);
                    delayToNeutral = maxDelayOfNeutral;
                }
                if (AttackStage == 2)
                {
                    _animator.SetInteger("AttackNumber", 2);
                    delayToNeutral = maxDelayOfNeutral;
                }
                if(AttackStage == 3)
                {
                    _animator.SetInteger("AttackNumber", 3);
                    delayToNeutral = maxDelayOfNeutral;
                    AttackStage = 0;
                } 
            }
        }

        // Go neutral 
        if (delayToNeutral >= 0) // if delay of neutral is grater or equal to 0
        {
            delayToNeutral -= Time.deltaTime; // Start Substract 
        }
        if (delayToNeutral <= 0) // if delay of neutral is less or equal to 0 
        {
            Attacking = false; // im not attacking
            _animator.SetBool("Attacking", false); // Set bolean animator ImNotAttacking to true
        }
    }

    private void Attack1()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, radiusAttack, hittable);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().GettingDamage(Damage);
        }
    }
    private void Attack2()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, radiusAttack, hittable);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().GettingDamage(Damage);
        }
    }
    private void Attack3()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, radiusAttack, hittable);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().GettingDamage(Damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origenCheckPoint.position, radius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, radiusAttack);
    }
   
}
