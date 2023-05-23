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

    public Slider ChargeLSlider;
    public Slider ChargeRSlider;

    public ParticleSystem dust;
    public ParticleSystem death;
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
        if ((rb.velocity.x < 0.1 || rb.velocity.x > -0.1) && (rb.velocity.y < 0.1 || rb.velocity.y > -0.1) && !touchingSlope && !isFalling)
        {
            animator.SetBool("stationary", true);

            canJump = true;
            //rb.sharedMaterial = normalMaterial;
        } else
        {
            //Debug.Log(rb.velocity.x + " & " + rb.velocity.y);
            //animator.SetBool("stationary", false);
        }




        if (rb.velocity.x > 0 && !touchingGround)
        {
            spriteRender.flipX = false;
        }
        if (rb.velocity.x < 0 && !touchingGround)
        {
            spriteRender.flipX = true;
        }

        if (!isFalling && touchingGround)
        {
            startOfFall = rb.transform.position;
        }


        if (isFalling && touchingGround && Mathf.Abs(startOfFall.y - rb.transform.position.y) > 5 && !touchingSlope)
        {
            startOfFall = rb.transform.position;
            PlayerDeath();
            animator.SetBool("landed", true);
            animator.SetBool("splat", true);
            isFalling = false;
            Invoke("PlayerCanJump", 1f);
        } else if (isFalling && touchingGround && !touchingSlope)
        {
            animator.SetBool("landed", true);
            isFalling = false;
            Invoke("PlayerCanJump", 0.4f);
        }

        if (!touchingGround)
        {
            animator.SetBool("Jumped", false);
            rb.sharedMaterial = bounceMaterial;
            animator.SetFloat("yVelocity", rb.velocity.y);
            isFalling = true;
        }
        else
        {
            rb.sharedMaterial = normalMaterial;
            animator.SetFloat("yVelocity", rb.velocity.y);
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
    }

    void PlayerCanJump()
    {
        canJump = true;
    }

    void ResetJump()
    {
        //isFalling = true;
        jumpValue = 0.0f;
    }

    public void CompleteLevel()
    {
        CompletionTime = GameTime;
        SceneManager.LoadScene("2");
    }

    void PlayerDeath()
    {
        death.Play(); //(commented out to test performance)
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

        if (Input.touchCount > 0 && touchingGround && canJump)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase >= TouchPhase.Began)
            {
                animator.SetBool("jumping", true);
                //ChargeRSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(-280.8f, touch.position.y);
                //ChargeLSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(280.8f, touch.position.y);
                if (touch.position.x < Screen.width / 2)
                {
                    spriteRender.flipX = true;
                    jumpValue += Time.deltaTime * 20f;
                    directionValue = -2.6f;
                    ChargeLSlider.value = jumpValue;
                    ChargeRSlider.value = 0;

                }

                if (touch.position.x > Screen.width / 2)
                {
                    spriteRender.flipX = false;
                    jumpValue += Time.deltaTime * 20f;
                    directionValue = 2.6f;
                    
                    ChargeRSlider.value = jumpValue;
                    ChargeLSlider.value = 0;
                }

                if (jumpValue >= jumpHeight)
                {
                    jumpValue = jumpHeight;
                }

                animator.SetFloat("jumpCharge", jumpValue);
                //currentTime = currentTime + Time.deltaTime;
                //time = TimeSpan.FromSeconds(currentTime);
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
                animator.SetBool("landed", false);
                PlayerJumped();
                currentTime = 0;
                if (touchingGround)
                {
                    canJump = false;
                }
            }
        }
    }

    void PlayerJumped()
    {
        canJump = false;
        TotalJumps = TotalJumps + 1;
        rb.velocity = new Vector2(directionValue, jumpValue);
        ChargeRSlider.value = 0;
        ChargeLSlider.value = 0;
        Invoke("ResetJump", 0.1f);
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
        //dust.Play();
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
