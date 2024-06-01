using UnityEngine;
using bronya;


public class PlayerMovement : MonoBehaviour
{
    private Vector2 input;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    public float movespeed;
    public float jumpForce;
    public float doubleJumpForce = 15f;
    public float maxJumpTime = 0.3f;
    public float jumpStartTime;
    private bool isJumping = false;
    public float dashTime;
    private float dashTimeLeft;
    public bool isDashing;
    private float lastDash = -10f;      //确保第一次Time.time一定大于lastDash+ dashCoolDown;
    public float dashCoolDown;
    public float dashSpeed;
    private bool canDoubleJump;
    private bool isOnGround;

    public GameObject shadowprefab;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        jumpForce = 15;

    }
    private void Update()
    {
        //ObjectPool.Instance.GetObject(shadowprefab);
        if (transform.position.y > 2.42)
        {
            isOnGround = false;
        }
        else if (transform.position.y<=2.42)
        {
            isOnGround = true;
            _animator.SetBool(AnimatorHash.KingWing, false);
        }
        input.x = Input.GetAxisRaw("Horizontal");
        _rigidbody.velocity = new Vector2(input.x * movespeed, _rigidbody.velocity.y);
        if (_rigidbody.velocity.x != 0)
        {
            _animator.SetBool(AnimatorHash.IsMoving, true);
            if (_rigidbody.velocity.x > 0)                //right
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (_rigidbody.velocity.x < 0)          //left
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
        else
        {
            _animator.SetBool(AnimatorHash.IsMoving, false);
        }
        //在按下Spcae的瞬间，准备开启PerformJump函数，并且无论按下了多长时间，Perform函数只会在0.2s之后被调用，那么这是heldTime就是0.2s
        if (Input.GetKeyDown(KeyCode.Z) && !isJumping)
        {

            if(isOnGround)
            {
                isJumping = true;
                jumpStartTime = Time.time;
                canDoubleJump = true;
                Invoke("PerformJump", 0.2f);
                _animator.SetBool(AnimatorHash.KingWing, false);
            }
            else if(canDoubleJump)
            {
                canDoubleJump = false;
                DoubleJump();

            }
        }
        //如果你在任何时间抬起了Spcae，这个Invoke被取消
        if (Input.GetKeyUp(KeyCode.Z) && isJumping)
        {
            CancelInvoke("PerformJump");
            PerformJump();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(Time.time>(lastDash +dashCoolDown))
            {
                ReadyToDash();
            }
        }


        #region GravityScale && animation
        if (isOnGround)
        {
            _animator.SetBool(AnimatorHash.IsJumping, false);
            _animator.SetBool(AnimatorHash.IsFalling, false);
            // 可能还有其他关于站立或行走的动画状态
        }
        else
        {
            if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.gravityScale = 4.0f;
                _animator.SetBool(AnimatorHash.IsJumping, true);
                _animator.SetBool(AnimatorHash.IsFalling, false);
            }
            else if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.gravityScale = 7.0f;
                _animator.SetBool(AnimatorHash.IsFalling, true);
                _animator.SetBool(AnimatorHash.KingWing, false);
                _animator.SetBool(AnimatorHash.IsJumping, false);
            }
        }
        #endregion
    }
    //按下了0.2s或者更长的时间，heldTime = Time.time - jumpStartTime == 0.2f
    private void PerformJump()
    {
        float heldTime = Time.time - jumpStartTime;
        float jumpMultiplier = Mathf.Clamp01(heldTime / 0.2f)*0.5f+0.5f;         // 最小值为0.5，最大值为1
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce * jumpMultiplier);
        isJumping = false;
    }

    private void DoubleJump()
    {
        _animator.SetBool(AnimatorHash.KingWing, true);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, doubleJumpForce);
    }
    #region "Dashing"
    private void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }

    private void FixedUpdate()
    {
        Dash();
    }

    private void Dash()
    {
        //冲刺时把设置为0；
        if(isDashing)
        {
            if (dashTimeLeft > 0) 
            {
                //_rigidbody.gravityScale = 0;
                _animator.SetBool(AnimatorHash.IsDashing, true);
                float direction = transform.rotation.y == 0 ? -1 : 1;
                _rigidbody.velocity = new Vector2(dashSpeed * direction, _rigidbody.velocity.y);
                //1
                ObjectPool.Instance.GetObject(shadowprefab);
                dashTimeLeft -= Time.deltaTime;
            }
            if(dashTimeLeft<=0)
            {
                isDashing = false;
                _animator.SetBool(AnimatorHash.IsDashing,false);
            }
        }
    }

    #endregion
}
