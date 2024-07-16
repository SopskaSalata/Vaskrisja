using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private float horizontal;
    public float speed = 0f;
    public float maxSpeed = 350f;
    public float accelerationspeed;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    private bool isWallSliding;
    public float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    private float wallJupingDirection;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    public bool col = false;
    private Animator anim;
    public Transform atackPoint;
    public float atackRange = 0.5f;
    public LayerMask enemyLayers;
    public int atackDamage = 40;
    public float atackRate = 2f;
    float nextAtackTime = 0f;
    

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    private void Update()
    {
        anim = GetComponent<Animator>();

       if(horizontal != 0f && speed < maxSpeed)
        {
            speed += Time.deltaTime * accelerationspeed;

        }

       if(horizontal == 0f)
        {
            speed = 0f;
        }
       
            Atack();
        
        
            if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else 
        {
            anim.SetBool("IsRunning", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            anim.SetBool("IsDashing",true);
        }

        else
        {
            anim.SetBool("IsDashing", false);
        }
      

        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
        Atack();
        
    }

    private void FixedUpdate()
    {

        WallSlider();
        

        if (isDashing)
        {
            return;
        } 

       
            rb.velocity = new Vector3(horizontal * speed * Time.deltaTime, rb.velocity.y, 0f);
      
        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlider()
    {
        if (isWalled() && !IsGrounded() && rb.velocity.y <0)
        {
            isWallSliding = true;
            
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlidingSpeed);
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJupingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump")&& wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJupingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJupingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;


    }

    private void Atack()
    {

        if (Time.time >= nextAtackTime)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("Atack");

                nextAtackTime = Time.time + 1f / atackRate;

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(atackPoint.position, atackRange, enemyLayers);

                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyHP>().TakeDamage(atackDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(atackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(atackPoint.position, atackRange);
    }
}