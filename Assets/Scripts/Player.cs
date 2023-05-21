using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //Game Objects
    public GameObject DeathDisplay;

    //Ints
    public int TotalJumps;
    public int TotalDeaths;
    public int Completions;

    //Floats
    public float GameTime;
    private float CompletionTime;
    private float jumpHeight = 8.5f;
    private float jumpValue;
    private float directionValue;
    public float groundCheckRadius;
    private float CameraHeight;
    private float CameraWidth;
    private float jumpTimeCount;
    public float jumpTime;
    private float currentTime;

    //Bools
    public bool touchingGround, touchingSlope;
    public bool isFalling;
    public bool justJumped;
    public bool canJump;


    public ParticleSystem dust;
    public TextMeshProUGUI DeathCount;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;
    public PhysicsMaterial2D bounceMaterial, normalMaterial;
    public Animator animator;
    private SpriteRenderer spriteRender;
    private Vector3 startOfFall;

    public Camera worldCam;
    public Transform groundCheck;
    


    //private float startOfFall;

    
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
        if (File.Exists(path) && !SceneManager.GetSceneByName("Menu").isLoaded)
        {
            LoadPlayer();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fan"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y+10f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Total game time taken while playing.
        GameTime = GameTime + Time.deltaTime;

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

            //Here should be the death animation code.
            //Addition to the death counter
            //UI deathcounter pop up
            PlayerDeath();
            
            Debug.Log("player splat");
            animator.SetBool("splat", true);
            isFalling = false;
            Invoke("PlayerCanJump", 0.1f);
        } else if (isFalling && touchingGround && !touchingSlope)
        {
            Debug.Log("player hit floor no splat");
            animator.SetTrigger("PlayerLanded");
            isFalling = false;
            Invoke("PlayerCanJump", 0.1f);
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

            canJump = false;
            TotalJumps = TotalJumps + 1;
            rb.velocity = new Vector2(directionValue, jumpValue);
            if (jumpValue > 5)
            {
                CreateDust();
            }
            Invoke("ResetJump", 0.1f);

        }

    }

    void PlayerCanJump()
    {
        canJump = true;
    }

    void ResetJump()
    {
        isFalling = true;
        jumpValue = 0.0f;
    }

    public void CompleteLevel()
    {
        Debug.Log("Completed level");
        CompletionTime = GameTime;
        SceneManager.LoadScene("2");
    }

    void PlayerDeath()
    {
        Debug.Log("Death called");
        LeanTween.alpha(DeathDisplay.GetComponent<RectTransform>(), 1f, 0f);
        LeanTween.value(DeathCount.gameObject, a => DeathCount.color = a, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0f);
        StartCoroutine(IncrementDeathCount());
        DeathDisplay.SetActive(true);
        LeanTween.alpha(DeathDisplay.GetComponent<RectTransform>(), 0f, .5f).setDelay(2);
        LeanTween.value(DeathCount.gameObject, a => DeathCount.color = a, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), .5f).setDelay(2f);

    }

    void TouchInput()
    {
        if (EventSystem.current.IsPointerOverGameObject() ||
            EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        if (Input.touchCount > 0 && touchingGround && canJump && !isFalling)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase >= TouchPhase.Began)
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





            if (touch.phase == TouchPhase.Ended)
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

    private bool lastPositionMatch()
    {
        if (startOfFall == rb.transform.position)
        {
            return true;
        } else
        {
            return false;
        }
        
    }

    public void CreateDust()
    {
        dust.Play();
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

    public IEnumerator IncrementDeathCount()
    {
        yield return new WaitForSeconds(0.5f);
        TotalDeaths = TotalDeaths + 1;
        DeathCount.text = TotalDeaths.ToString();
    }
}
