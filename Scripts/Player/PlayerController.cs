<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
  
using UnityEngine;
using UnityEngine.EventSystems;
  


public class PlayerController : MonoBehaviour
{
    [Header("플레이어 스테이터스 조절")]
    public float maxSpeed;
    public float walkSpeed = 3;

    public float jumpForce = 20f;
    public int jumpCount = 0;
    public float dropDuration = 0.5f;
    public float downForce = 100f;
    private bool isDropping = false;

    public float dashCoolDown = 0.9f;
 

    [SerializeField]
    float originalGravity = 8;


    private bool isGrounded = false;
    private bool playerOnBoard = false;
    public float lastMoveDirc = 1f;


    private bool isWalking = true;
 
    public bool isDashing = false;
    public bool playerPaused = false;
    private bool isDashAble = true;

 
 
 

    private Collider2D currentBoard = null;
    CapsuleCollider2D playerCollider;



    public GameObject backLight;
    public GameObject shadow;
    private Rigidbody2D rigid;
    private Animator animator;
    SpriteRenderer spriteRenderer;

    PlayerCombat playerCombat;
    TrailRenderer dashTrail;
    AnimatorConverter animCon;
    public UIManager uiManager;
    public GameObject jumpEffect;

    GameManager gameManager;
    string currentSceneName;

    public AudioClip dashClip;

    public PhysicsMaterial2D groundedMat;
    public PhysicsMaterial2D slipperyMat;

    public LayerMask platformLayer;
 



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = GameManager.Instance;
        playerCombat = GetComponent<PlayerCombat>();
        dashTrail = GetComponent<TrailRenderer>();
        animCon = GetComponent<AnimatorConverter>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;

        playerCollider = GetComponent<CapsuleCollider2D>();

    }

    private void FixedUpdate()
    {
        float moveDirection = 0;
        if (playerPaused)
        {
            rigid.linearVelocity = new Vector2(0, rigid.linearVelocity.y);

            playerCollider.sharedMaterial = groundedMat;
            return;
        }

        if (playerCombat.isDied || playerCombat.isAttacking)
        {
            return;
        }



        if (playerCombat.isKnockedBack || isDashing || animCon.converting || playerCombat.isSkillUsing)
        {
            if (playerCombat.isKnockedBack || isDashing || playerCombat.isSkillUsing)
                return;
            else if (animCon.converting)
            {
                rigid.linearVelocity = Vector2.zero;
                return;
            }
        }




        bool isA_Pressed = Input.GetKey(KeyCode.A);
        bool isD_Pressed = Input.GetKey(KeyCode.D);

 
        if (isA_Pressed && isD_Pressed) 
        {
            isWalking = false;
            moveDirection = 0;
            animator.SetBool("isWalking", false);
        }
        else if (isA_Pressed) 
        {
            isWalking = true;
            moveDirection = -1;
            animator.SetBool("isWalking", true);
        }
        else if (isD_Pressed) 
        {
            isWalking = true;
            moveDirection = 1;
            animator.SetBool("isWalking", true);
        }
        else 
        {
            isWalking = false;
            moveDirection = 0;
            animator.SetBool("isWalking", false);
        }
        

 
            float speed = walkSpeed;

 
 
        rigid.linearVelocity = new Vector2(moveDirection * speed, rigid.linearVelocity.y);


 
        if (rigid.linearVelocity.x > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < -maxSpeed)
            rigid.linearVelocity = new Vector2(-maxSpeed, rigid.linearVelocity.y);


    }





    private void Update()
    {

        if (playerCombat.isDied || animCon.converting || playerPaused)
        {

            return;
        }
        if (isDashing || playerCombat.isAttacking)
        {
            playerCollider.sharedMaterial = groundedMat;
            animator.SetBool("isWalking", false);
            return;
        }
        if (isDashing)
        {
            backLight.gameObject.SetActive(false);
            shadow.gameObject.SetActive(false);

        }
        else if (currentSceneName == "Stage07")
        {
 

 
        }
        else
        {
            backLight.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S) && !playerCombat.isSkillUsing) 
        {

            if (jumpCount == 0)
            {
                animator.ResetTrigger("Attack");
                animator.SetTrigger("Jump");

                jumpCount++;
                rigid.linearVelocity = Vector2.zero;
                rigid.AddForce(new Vector2(0, jumpForce));
                shadow.gameObject.SetActive(false);
                GameObject jumpeffectobj = Instantiate(jumpEffect, gameObject.transform.position + new Vector3(0, 0.5f, 0), quaternion.identity);
                Destroy(jumpeffectobj, 1f);
            }


        }
        else if (Input.GetKeyUp(KeyCode.Space) && rigid.linearVelocity.y > 0) 
        {
            rigid.linearVelocity = rigid.linearVelocity * 0.5f;

        }
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S))
        {
            Debug.Log("1");
            StartCoroutine(DownJump());
        }


        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.normalized.x * 0.5f, rigid.linearVelocity.y);

        }

        if (Input.GetButton("Horizontal"))
        {
            if (playerCombat.isSkillUsing) return;

            if (!isDashing)
            {
                float moveDirection = Input.GetAxisRaw("Horizontal");
                if (moveDirection != 0)
                {
                    transform.localScale = new Vector3(1f * moveDirection, transform.localScale.y, transform.localScale.z);

                }
                if (moveDirection != 0)
                {
                    lastMoveDirc = moveDirection;
                }
            }

            

        }

        RaycastHit2D groundHit = CheckGrounded();
    
 
        bool isHitGround = groundHit.collider != null;
        
 
        bool isGoingUp = rigid.linearVelocityY > 0.01f;
        float verticalBumpAmount = 0.05f;
 
        if (isHitGround)
        {
 
 
            bool hitOneWayPlatform = platformLayer == (platformLayer | (1 << groundHit.collider.gameObject.layer));

            if (isGoingUp && hitOneWayPlatform)
            {
 
 
 
                isGrounded = false;
                rigid.position += Vector2.up * verticalBumpAmount;
            }
            else
            {
 
                isGrounded = true;
            }
        }
        else
        {
 
            isGrounded = false;
        }


        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {

 
            animator.SetBool("isWalking", false);

            isWalking = false;
 
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        DashControl(15);
        isGrounded = CheckGrounded();

        if (isGrounded)
        {
            playerCollider.sharedMaterial = groundedMat;
 
        }
        else
        {
            playerCollider.sharedMaterial = slipperyMat;
        }

        
    }
    Scene currentScene;

    IEnumerator DownJump()
    {
        LayerMask boardLayerMask = 1 << LayerMask.NameToLayer("Board");

        Vector2 rayOrigin = playerCollider.bounds.center;
        float rayDistance = playerCollider.bounds.extents.y + 0.1f; 

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, boardLayerMask);

 
        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red, 1.0f);



        if (hit.collider != null)
        {
            Debug.LogWarning("tlqkftoRldi");
 
            Physics2D.IgnoreCollision(playerCollider, hit.collider, true);

 
            yield return new WaitForSeconds(0.3f);

 
            if (hit.collider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, hit.collider, false);
            }
        }

    }
    public void PlayerDie()
    {


        backLight.gameObject.SetActive(false);
        shadow.gameObject.SetActive(false);
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        GameManager.Instance.currentPotion = GameManager.Instance.currentPotionMax;
        GameManager.Instance.playerInstanceCurrentHP = GameManager.Instance.playerInstanceMaxHP;
        GameManager.Instance.playerInstanceCurrentST = GameManager.Instance.playerInstanceMaxST;

        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        uiManager.FadeOutStart();
        yield return new WaitForSeconds(2);
        if (sceneName == "Stage01")
        {
            SceneManager.LoadScene("Stage01");
        }
        else if (sceneName == "DungeonBlue_00" || sceneName == "DungeonBlue_01")
        {
            SceneManager.LoadScene("Stage03");
        }
        else if (sceneName == "Stage04")
        {
            SceneManager.LoadScene("Stage04");
        }
        else if (sceneName == "Stage04.5")
        {

            gameManager.savedState = PlayerState.Blue;
            SceneManager.LoadScene("Stage04.5");
        }
        else if (sceneName == "DungeonPurple" || sceneName == "DungeonYellow")
        {
            SceneManager.LoadScene("Stage06");
        }
        else if (sceneName == "Stage09")
        {
            SceneManager.LoadScene("Stage09");
        }


    }
    void DashControl(float dashST)
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashAble && playerCombat.currentST >= dashST)
        {

            isDashing = true;
            float moveDirection = Input.GetAxisRaw("Horizontal");

            animator.SetTrigger("Dash");
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            gameObject.layer = 9;

            StartCoroutine(Dash(lastMoveDirc, dashST));
            StartCoroutine(DashCoolTimeCheck());
        }
    }

    IEnumerator Dash(float dasDirc, float dashST)
    {

        isDashing = true;
        SoundManager.instance.SFXPlay("Dash", dashClip);
        float dashSpeed = 40f; 
        float dashTime = 0.23f; 
        float elapsedTime = 0f; 

        rigid.linearVelocity = Vector2.zero;
        rigid.gravityScale = 0;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        playerCombat.currentST -= dashST; 
        while (elapsedTime < dashTime)
        {
            float t = elapsedTime / dashTime;
            float currentSpeed = Mathf.Lerp(dashSpeed, 0f, t);
            rigid.linearVelocity = new Vector2(dasDirc * currentSpeed, 0);

            elapsedTime += Time.deltaTime;

            yield return null;

        }

        rigid.gravityScale = originalGravity;

        isDashing = false;

        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(0.37f);
        gameObject.layer = 7;

    }

    IEnumerator DashCoolTimeCheck()
    {
        isDashAble = false;
        yield return new WaitForSeconds(dashCoolDown);
        isDashAble = true;


    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;


            jumpCount = 0;
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Jump");
            shadow.gameObject.SetActive(true);
        }


    }
    private void OnCollisionExit2D(Collision2D collision) 
    {

        isGrounded = false;
        animator.SetBool("isGrounded", false);
        shadow.gameObject.SetActive(false);
        if (collision.collider.CompareTag("Board"))
        {
            playerOnBoard = false;
 
            currentBoard = null;
            shadow.gameObject.SetActive(false);
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Board"))
        {
 
            playerOnBoard = true;
            currentBoard = other.collider;
        }
    }

    public void PlayerIdlePlay()
    {
        animator.Play("Idle");
        animator.ResetTrigger("Attack");
        animator.SetBool("isWalking", false);
        animator.ResetTrigger("Dash");
        animator.SetInteger("CurrentMode", (int)animCon.currentState);
    }
    public void PlayerPause()
    {
        playerPaused = true;
        PlayerIdlePlay();
    }
    public void PlayerResume()
    {
        playerPaused = false;
        animator.SetInteger("CurrentMode", (int)animCon.currentState);
    }

    [SerializeField]
    private LayerMask groundLayers;

    [Header("Ground Check")]
    public float groundWidthMultiplier = 0.5f; 
    public float groundCheckBuffer = 0.05f;
    private RaycastHit2D CheckGrounded()
    {
 

 
        float boxWidth = playerCollider.size.x * groundWidthMultiplier;
        Vector2 boxSize = new Vector2(boxWidth, 0.1f);

 
        float playerHalfHeight = playerCollider.size.y / 2f;
        float boxHalfHeight = boxSize.y / 2f;

        Vector2 boxOrigin = (Vector2)transform.position + playerCollider.offset;
        boxOrigin.y -= (playerHalfHeight - boxHalfHeight); 

 
        float castDistance = groundCheckBuffer;

 
        RaycastHit2D hit = Physics2D.BoxCast(
            boxOrigin, 
            boxSize, 
            0f, 
            Vector2.down, 
            castDistance, 
            groundLayers 
        );

 
        return hit;
    }
    
    private void OnDrawGizmosSelected()
    {
 
        if (playerCollider == null) return;

 
        Vector2 boxOrigin = (Vector2)transform.position + playerCollider.offset;
        float boxWidth = playerCollider.size.x * groundWidthMultiplier;
        Vector2 boxSize = new Vector2(boxWidth, 0.1f);
        float castDistance = (playerCollider.size.y / 2f) - (boxSize.y / 2f) + groundCheckBuffer;

 
        Vector2 endBoxOrigin = boxOrigin + Vector2.down * castDistance;

 
 
        if (Application.isPlaying && CheckGrounded())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

 
        Gizmos.DrawWireCube(boxOrigin, boxSize);

 
        Gizmos.DrawWireCube(endBoxOrigin, boxSize);

 
        Gizmos.DrawLine(boxOrigin + new Vector2(-boxWidth / 2, -boxSize.y / 2), endBoxOrigin + new Vector2(-boxWidth / 2, -boxSize.y / 2));
        Gizmos.DrawLine(boxOrigin + new Vector2(boxWidth / 2, -boxSize.y / 2), endBoxOrigin + new Vector2(boxWidth / 2, -boxSize.y / 2));
        Gizmos.DrawLine(boxOrigin + new Vector2(-boxWidth / 2, boxSize.y / 2), endBoxOrigin + new Vector2(-boxWidth / 2, boxSize.y / 2));
        Gizmos.DrawLine(boxOrigin + new Vector2(boxWidth / 2, boxSize.y / 2), endBoxOrigin + new Vector2(boxWidth / 2, boxSize.y / 2));
    }

}
=======
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
  
using UnityEngine;
using UnityEngine.EventSystems;
  


public class PlayerController : MonoBehaviour
{
    [Header("플레이어 스테이터스 조절")]
    public float maxSpeed;
    public float walkSpeed = 3;

    public float jumpForce = 20f;
    public int jumpCount = 0;
    public float dropDuration = 0.5f;
    public float downForce = 100f;
    private bool isDropping = false;

    public float dashCoolDown = 0.9f;
 

    [SerializeField]
    float originalGravity = 8;


    private bool isGrounded = false;
    private bool playerOnBoard = false;
    public float lastMoveDirc = 1f;


    private bool isWalking = true;
 
    public bool isDashing = false;
    public bool playerPaused = false;
    private bool isDashAble = true;

 
 
 

    private Collider2D currentBoard = null;
    CapsuleCollider2D playerCollider;



    public GameObject backLight;
    public GameObject shadow;
    private Rigidbody2D rigid;
    private Animator animator;
    SpriteRenderer spriteRenderer;

    PlayerCombat playerCombat;
    TrailRenderer dashTrail;
    AnimatorConverter animCon;
    public UIManager uiManager;
    public GameObject jumpEffect;

    GameManager gameManager;
    string currentSceneName;

    public AudioClip dashClip;

    public PhysicsMaterial2D groundedMat;
    public PhysicsMaterial2D slipperyMat;

    public LayerMask platformLayer;
 



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = GameManager.Instance;
        playerCombat = GetComponent<PlayerCombat>();
        dashTrail = GetComponent<TrailRenderer>();
        animCon = GetComponent<AnimatorConverter>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;

        playerCollider = GetComponent<CapsuleCollider2D>();

    }

    private void FixedUpdate()
    {
        float moveDirection = 0;
        if (playerPaused)
        {
            rigid.linearVelocity = new Vector2(0, rigid.linearVelocity.y);

            playerCollider.sharedMaterial = groundedMat;
            return;
        }

        if (playerCombat.isDied || playerCombat.isAttacking)
        {
            return;
        }



        if (playerCombat.isKnockedBack || isDashing || animCon.converting || playerCombat.isSkillUsing)
        {
            if (playerCombat.isKnockedBack || isDashing || playerCombat.isSkillUsing)
                return;
            else if (animCon.converting)
            {
                rigid.linearVelocity = Vector2.zero;
                return;
            }
        }




        bool isA_Pressed = Input.GetKey(KeyCode.A);
        bool isD_Pressed = Input.GetKey(KeyCode.D);

 
        if (isA_Pressed && isD_Pressed) 
        {
            isWalking = false;
            moveDirection = 0;
            animator.SetBool("isWalking", false);
        }
        else if (isA_Pressed) 
        {
            isWalking = true;
            moveDirection = -1;
            animator.SetBool("isWalking", true);
        }
        else if (isD_Pressed) 
        {
            isWalking = true;
            moveDirection = 1;
            animator.SetBool("isWalking", true);
        }
        else 
        {
            isWalking = false;
            moveDirection = 0;
            animator.SetBool("isWalking", false);
        }
        

 
            float speed = walkSpeed;

 
 
        rigid.linearVelocity = new Vector2(moveDirection * speed, rigid.linearVelocity.y);


 
        if (rigid.linearVelocity.x > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < -maxSpeed)
            rigid.linearVelocity = new Vector2(-maxSpeed, rigid.linearVelocity.y);


    }





    private void Update()
    {

        if (playerCombat.isDied || animCon.converting || playerPaused)
        {

            return;
        }
        if (isDashing || playerCombat.isAttacking)
        {
            playerCollider.sharedMaterial = groundedMat;
            animator.SetBool("isWalking", false);
            return;
        }
        if (isDashing)
        {
            backLight.gameObject.SetActive(false);
            shadow.gameObject.SetActive(false);

        }
        else if (currentSceneName == "Stage07")
        {
 

 
        }
        else
        {
            backLight.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S) && !playerCombat.isSkillUsing) 
        {

            if (jumpCount == 0)
            {
                animator.ResetTrigger("Attack");
                animator.SetTrigger("Jump");

                jumpCount++;
                rigid.linearVelocity = Vector2.zero;
                rigid.AddForce(new Vector2(0, jumpForce));
                shadow.gameObject.SetActive(false);
                GameObject jumpeffectobj = Instantiate(jumpEffect, gameObject.transform.position + new Vector3(0, 0.5f, 0), quaternion.identity);
                Destroy(jumpeffectobj, 1f);
            }


        }
        else if (Input.GetKeyUp(KeyCode.Space) && rigid.linearVelocity.y > 0) 
        {
            rigid.linearVelocity = rigid.linearVelocity * 0.5f;

        }
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S))
        {
            Debug.Log("1");
            StartCoroutine(DownJump());
        }


        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.normalized.x * 0.5f, rigid.linearVelocity.y);

        }

        if (Input.GetButton("Horizontal"))
        {
            if (playerCombat.isSkillUsing) return;

            if (!isDashing)
            {
                float moveDirection = Input.GetAxisRaw("Horizontal");
                if (moveDirection != 0)
                {
                    transform.localScale = new Vector3(1f * moveDirection, transform.localScale.y, transform.localScale.z);

                }
                if (moveDirection != 0)
                {
                    lastMoveDirc = moveDirection;
                }
            }

            

        }

        RaycastHit2D groundHit = CheckGrounded();
    
 
        bool isHitGround = groundHit.collider != null;
        
 
        bool isGoingUp = rigid.linearVelocityY > 0.01f;
        float verticalBumpAmount = 0.05f;
 
        if (isHitGround)
        {
 
 
            bool hitOneWayPlatform = platformLayer == (platformLayer | (1 << groundHit.collider.gameObject.layer));

            if (isGoingUp && hitOneWayPlatform)
            {
 
 
 
                isGrounded = false;
                rigid.position += Vector2.up * verticalBumpAmount;
            }
            else
            {
 
                isGrounded = true;
            }
        }
        else
        {
 
            isGrounded = false;
        }


        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {

 
            animator.SetBool("isWalking", false);

            isWalking = false;
 
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        DashControl(15);
        isGrounded = CheckGrounded();

        if (isGrounded)
        {
            playerCollider.sharedMaterial = groundedMat;
 
        }
        else
        {
            playerCollider.sharedMaterial = slipperyMat;
        }

        
    }
    Scene currentScene;

    IEnumerator DownJump()
    {
        LayerMask boardLayerMask = 1 << LayerMask.NameToLayer("Board");

        Vector2 rayOrigin = playerCollider.bounds.center;
        float rayDistance = playerCollider.bounds.extents.y + 0.1f; 

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, boardLayerMask);

 
        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red, 1.0f);



        if (hit.collider != null)
        {
            Debug.LogWarning("tlqkftoRldi");
 
            Physics2D.IgnoreCollision(playerCollider, hit.collider, true);

 
            yield return new WaitForSeconds(0.3f);

 
            if (hit.collider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, hit.collider, false);
            }
        }

    }
    public void PlayerDie()
    {


        backLight.gameObject.SetActive(false);
        shadow.gameObject.SetActive(false);
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        GameManager.Instance.currentPotion = GameManager.Instance.currentPotionMax;
        GameManager.Instance.playerInstanceCurrentHP = GameManager.Instance.playerInstanceMaxHP;
        GameManager.Instance.playerInstanceCurrentST = GameManager.Instance.playerInstanceMaxST;

        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        uiManager.FadeOutStart();
        yield return new WaitForSeconds(2);
        if (sceneName == "Stage01")
        {
            SceneManager.LoadScene("Stage01");
        }
        else if (sceneName == "DungeonBlue_00" || sceneName == "DungeonBlue_01")
        {
            SceneManager.LoadScene("Stage03");
        }
        else if (sceneName == "Stage04")
        {
            SceneManager.LoadScene("Stage04");
        }
        else if (sceneName == "Stage04.5")
        {

            gameManager.savedState = PlayerState.Blue;
            SceneManager.LoadScene("Stage04.5");
        }
        else if (sceneName == "DungeonPurple" || sceneName == "DungeonYellow")
        {
            SceneManager.LoadScene("Stage06");
        }
        else if (sceneName == "Stage09")
        {
            SceneManager.LoadScene("Stage09");
        }


    }
    void DashControl(float dashST)
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashAble && playerCombat.currentST >= dashST)
        {

            isDashing = true;
            float moveDirection = Input.GetAxisRaw("Horizontal");

            animator.SetTrigger("Dash");
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            gameObject.layer = 9;

            StartCoroutine(Dash(lastMoveDirc, dashST));
            StartCoroutine(DashCoolTimeCheck());
        }
    }

    IEnumerator Dash(float dasDirc, float dashST)
    {

        isDashing = true;
        SoundManager.instance.SFXPlay("Dash", dashClip);
        float dashSpeed = 40f; 
        float dashTime = 0.23f; 
        float elapsedTime = 0f; 

        rigid.linearVelocity = Vector2.zero;
        rigid.gravityScale = 0;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        playerCombat.currentST -= dashST; 
        while (elapsedTime < dashTime)
        {
            float t = elapsedTime / dashTime;
            float currentSpeed = Mathf.Lerp(dashSpeed, 0f, t);
            rigid.linearVelocity = new Vector2(dasDirc * currentSpeed, 0);

            elapsedTime += Time.deltaTime;

            yield return null;

        }

        rigid.gravityScale = originalGravity;

        isDashing = false;

        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(0.37f);
        gameObject.layer = 7;

    }

    IEnumerator DashCoolTimeCheck()
    {
        isDashAble = false;
        yield return new WaitForSeconds(dashCoolDown);
        isDashAble = true;


    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;


            jumpCount = 0;
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Jump");
            shadow.gameObject.SetActive(true);
        }


    }
    private void OnCollisionExit2D(Collision2D collision) 
    {

        isGrounded = false;
        animator.SetBool("isGrounded", false);
        shadow.gameObject.SetActive(false);
        if (collision.collider.CompareTag("Board"))
        {
            playerOnBoard = false;
 
            currentBoard = null;
            shadow.gameObject.SetActive(false);
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Board"))
        {
 
            playerOnBoard = true;
            currentBoard = other.collider;
        }
    }

    public void PlayerIdlePlay()
    {
        animator.Play("Idle");
        animator.ResetTrigger("Attack");
        animator.SetBool("isWalking", false);
        animator.ResetTrigger("Dash");
        animator.SetInteger("CurrentMode", (int)animCon.currentState);
    }
    public void PlayerPause()
    {
        playerPaused = true;
        PlayerIdlePlay();
    }
    public void PlayerResume()
    {
        playerPaused = false;
        animator.SetInteger("CurrentMode", (int)animCon.currentState);
    }

    [SerializeField]
    private LayerMask groundLayers;

    [Header("Ground Check")]
    public float groundWidthMultiplier = 0.5f; 
    public float groundCheckBuffer = 0.05f;
    private RaycastHit2D CheckGrounded()
    {
 

 
        float boxWidth = playerCollider.size.x * groundWidthMultiplier;
        Vector2 boxSize = new Vector2(boxWidth, 0.1f);

 
        float playerHalfHeight = playerCollider.size.y / 2f;
        float boxHalfHeight = boxSize.y / 2f;

        Vector2 boxOrigin = (Vector2)transform.position + playerCollider.offset;
        boxOrigin.y -= (playerHalfHeight - boxHalfHeight); 

 
        float castDistance = groundCheckBuffer;

 
        RaycastHit2D hit = Physics2D.BoxCast(
            boxOrigin, 
            boxSize, 
            0f, 
            Vector2.down, 
            castDistance, 
            groundLayers 
        );

 
        return hit;
    }
    
    private void OnDrawGizmosSelected()
    {
 
        if (playerCollider == null) return;

 
        Vector2 boxOrigin = (Vector2)transform.position + playerCollider.offset;
        float boxWidth = playerCollider.size.x * groundWidthMultiplier;
        Vector2 boxSize = new Vector2(boxWidth, 0.1f);
        float castDistance = (playerCollider.size.y / 2f) - (boxSize.y / 2f) + groundCheckBuffer;

 
        Vector2 endBoxOrigin = boxOrigin + Vector2.down * castDistance;

 
 
        if (Application.isPlaying && CheckGrounded())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

 
        Gizmos.DrawWireCube(boxOrigin, boxSize);

 
        Gizmos.DrawWireCube(endBoxOrigin, boxSize);

 
        Gizmos.DrawLine(boxOrigin + new Vector2(-boxWidth / 2, -boxSize.y / 2), endBoxOrigin + new Vector2(-boxWidth / 2, -boxSize.y / 2));
        Gizmos.DrawLine(boxOrigin + new Vector2(boxWidth / 2, -boxSize.y / 2), endBoxOrigin + new Vector2(boxWidth / 2, -boxSize.y / 2));
        Gizmos.DrawLine(boxOrigin + new Vector2(-boxWidth / 2, boxSize.y / 2), endBoxOrigin + new Vector2(-boxWidth / 2, boxSize.y / 2));
        Gizmos.DrawLine(boxOrigin + new Vector2(boxWidth / 2, boxSize.y / 2), endBoxOrigin + new Vector2(boxWidth / 2, boxSize.y / 2));
    }

}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
