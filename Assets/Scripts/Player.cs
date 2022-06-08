using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int Completions;
    public float GameTime;
    public int TotalJumps;
    private float CompletionTime;

    private float jumpHeight = 8.5f;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;
    public PhysicsMaterial2D bounceMaterial, normalMaterial;
    public bool canJump;
    private float jumpValue;
    private float directionValue;

    public Animator animator;
    private SpriteRenderer spriteRender;
    private Vector3 startOfFall;

    private float CameraHeight;
    private float CameraWidth;
    public Camera worldCam;

    public Transform groundCheck;
    public float groundCheckRadius;
    public bool touchingGround, touchingSlope;
    public bool isFalling;

    public bool justJumped;

    //private float startOfFall;

    private float jumpTimeCount;
    public float jumpTime;

    public Text startF;
    public Text FinalF;
    public Text timer;
    public Text JHeight;
    private float currentTime;
    public TimeSpan time;

    // Start is called before the first frame update
    void Start()
    {
        GameTime = 0;
        //startOfFall = rb.transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        currentTime = 0;

        string path = Application.persistentDataPath + "/player.file";
        Debug.Log(path);
        if (File.Exists(path))
        {
            LoadPlayer();
        }

    }

    private void FixedUpdate()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //Total game time taken while playing.
        GameTime = GameTime + Time.deltaTime;

        timer.text = rb.velocity.y.ToString();
        JHeight.text = rb.velocity.x.ToString();
        Vector3 pos = Camera.main.WorldToViewportPoint(rb.transform.position);
        touchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, 1<<6);
        touchingSlope = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, 1<<7);

        //If player jumps out of the screen then reset them back to where they were
        if (pos.x < -0.05 || 1.05 < pos.x)

        {
            rb.velocity = new Vector2(0f, 0f);
            rb.transform.position = startOfFall;
            canJump = true;
            animator.SetTrigger("ResetToIdle");
        }

        //If the players x and y velocity is 0 (completely still)
        if (rb.velocity.x == 0 && rb.velocity.y == 0 && !touchingSlope)
        {
            animator.SetBool("stationary", true);
            canJump = true;
        } else
        {
            animator.SetBool("stationary", false);
        }




        if (rb.velocity.x > 0 && !touchingGround)
        {
            spriteRender.flipX = false;
            animator.SetBool("WallTouch", false);
        }
        if (rb.velocity.x < 0 && !touchingGround)
        {
            spriteRender.flipX = true;
            animator.SetBool("WallTouch", false);
        }
        if (rb.velocity.x == 0 && !touchingGround)
        {
            animator.SetBool("WallTouch", true);
        }


        if (!isFalling && touchingGround)
        {
            startOfFall = rb.transform.position;
        }


        if (isFalling && touchingGround && Mathf.Abs(startOfFall.y - rb.transform.position.y) > 5 && !touchingSlope)
        {
            startOfFall = rb.transform.position;

            Debug.Log("player splat");
            animator.SetBool("splat", true);
            isFalling = false;
            Invoke("PlayerCanJump", 0.1f);
        } else if (isFalling && touchingGround)
        {
            Debug.Log("player hit floor no splat");
            //animator.SetBool("landed", true);
            animator.SetTrigger("PlayerLanded");
            isFalling = false;
            Invoke("PlayerCanJump", 0.1f);
            //rb.velocity = Vector3.zero;
        }

        if (!touchingGround)
        {
            animator.SetBool("Jumped", false);
            rb.sharedMaterial = bounceMaterial;
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
        else
        {
            rb.sharedMaterial = normalMaterial;
        }

        if (touchingSlope)
        {
            rb.sharedMaterial = normalMaterial;
            animator.SetBool("PlayerSliding", true);
        } else if (!touchingSlope)
        {
            animator.SetBool("PlayerSliding", false);
        }

        TouchInput();

        if (justJumped)
        {
            justJumped = false;
            isFalling = true;
            canJump = false;
            TotalJumps = TotalJumps + 1;
            rb.velocity = new Vector2(directionValue, jumpValue);
            Invoke("ResetJump", 0.3f);

        }

    }

    void PlayerCanJump()
    {
        canJump = true;
    }

    void ResetJump()
    {
        jumpValue = 0.0f;
    }

    public void CompleteLevel()
    {
        Debug.Log("Completed level");
        CompletionTime = GameTime;
    }

    void TouchInput()
    {
        if (EventSystem.current.IsPointerOverGameObject() ||
            EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            if (touch.phase >= TouchPhase.Began && touchingGround && canJump && !isFalling)
            {
                animator.SetBool("splat", false);
               animator.SetBool("landed", false);
                animator.SetBool("jumping", true);
                Debug.Log("Pressed jump");
                if (touch.position.x < Screen.width / 2)
                {
                    spriteRender.flipX = true;
                    jumpValue += Time.deltaTime * 20f;
                    directionValue = -2.6f;
                }

                if (touch.position.x > Screen.width / 2)
                {
                    spriteRender.flipX = false;
                    jumpValue += Time.deltaTime * 20f;
                    directionValue = 2.6f;
                }

                if (jumpValue >= jumpHeight)
                {
                    jumpValue = jumpHeight;
                }

                animator.SetFloat("jumpCharge", jumpValue);
                currentTime = currentTime + Time.deltaTime;
                time = TimeSpan.FromSeconds(currentTime);
                //timer.text = time.Seconds.ToString() + ":" + time.Milliseconds.ToString();


            }





            if (touch.phase == TouchPhase.Ended && touchingGround && canJump && !isFalling)
            {
                if (jumpValue < 2f)
                {
                    jumpValue = 2f;
                }
                animator.SetBool("Jumped", true);
                animator.SetBool("jumping", false);
                justJumped = true;
                currentTime = 0;
            }
        }
    }



    public void SavePlayer()
    {

        SaveSystem.SavePlayer(this, startOfFall, spriteRender.flipX);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.SavedPosition[0];
        position.y = data.SavedPosition[1];
        position.z = data.SavedPosition[2];
        transform.position = position;
        spriteRender.flipX = data.SpriteFlip;

        Debug.Log("player loaded");
    }
}
