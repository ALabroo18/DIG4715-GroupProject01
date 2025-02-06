using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.Burst.CompilerServices;


public class PlayerBehavior : MonoBehaviour
{

    public AudioSource audioSource;
    // Create singleton reference.
    public static PlayerBehavior instance;

    // Variables related to player movement.
    private Rigidbody2D rb2d;
    [HideInInspector] public float playerSpeed = 7.0f;
    private float horizontalMovement; // Used for both movement and sprite change.
    private float verticalMovement; // Used for both movement and sprite change.

    // Variables related to player sprite change.
    private float inputAmount = 0.02f;
    //  Animator playerAnimator;

    // Variables used for player class and color.
    private Color classColor;
    [HideInInspector] public string className;
    private bool hasFired = false;

    public int potionCount = 0;

    public bool hasKey = true;
    private float colorChangeTime = 0.25f; // Time between changes of color when player takes damage.

    // Variables used for weapon behavior.
    private Transform playerTransform;
    private Vector2 lastFacingDirection = Vector2.right;
    private float playerAttackCooldown = 1.1f;
    private float lastAttackTime;
    public GameObject playerWeapon;
    private float playerAttackSpeed = 10.5f;
    public GameObject playerBomb;
    private float bombCoolDown = 5.0f;
    // public GameObject sword;

    //Variables for Animations
    Animator animator;
    private Vector2 moveInput;

    // Create a list of the various classes that the player can be.
    //[HideInInspector] public List<string> playerClassName = new List<string>()
    //{
    //    "archer",
    //    "wizard",
    //    "blueberry"
    //};

    //// Variables for lives and score.
    [HideInInspector] public int maxLives = 2000;
    private int _lives = 0;
    //private int bombs = 3;

    //// When hit by an enemy or enemy attack, this is the amount of lives the player loses.
    private int livesLostOnHit = -10;

    // Lives variable that is accessed in other classes, so _lives is not accessed by other classes.
    public int Lives
    {
       get
       {
           return _lives;
       }
       set
       {
           _lives = value;
       }
    }

    private int _score = 0;

    // Score variables that is accessed in other classes, so _score is not accessed by other classes.
    public int Score
    {
       get
       {
           return _score;
       }
       set
       {
           _score = value;
       }
    }

    [Header("UI Elements")]
    //// UI variables.
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    //[SerializeField] private TextMeshProUGUI bombsText;
    //[SerializeField] private GameObject gameOverDisplay;

    //// Delegate and event when game is over.
    public delegate void GameOverEvent();
    public event GameOverEvent gameIsOver;

    //// When the player gets hit, they have a small invincibility time, and this bool tracks when they are or are not invincible.
    private bool invincible = false;
    //private float invincibleTime = 1.2f; // Time player is invincible

    void Start()
    {
        // Set singleton reference.
        instance = this;

        // Set references to player components.
        rb2d = GetComponent<Rigidbody2D>(); // Set rigidbody reference.
        animator = GetComponent<Animator>(); // Set animator reference.
        playerTransform = transform; // Set transform reference.
        // Assign movement axis references.
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        // Function that gives the player a random class, but will probably change in the future for player to choose their class.
        //PlayerClass();

        // Check to see if the classColor has been set. If not, set it a default white color.
        if (classColor == new Color (0, 0, 0, 0))
        {
            classColor = Color.white;
        }

        // Set player lives to the max at the start. Do this before setting UI so it is up to date.
        Lives = maxLives;

        // Run at start to make sure UI is displayed when player begins the game.
        SetUI();
        StartCoroutine(RemoveHealth());

        
        
    }

    public void PlaySound(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
//     public int score = 0;

// void Start()
// {
//     StartCoroutine( TimerRoutine() );
// }

IEnumerator RemoveHealth()
    {
        
        //yield on a new YieldInstruction that waits for 5 seconds.
        while(maxLives > 0){
            ChangeLives(-1);
            yield return new WaitForSeconds(1);
        }

        //After we have waited 5 seconds print the time again.
        
    }

    void Update()
    {
        // Run PlayerInput function that checks for any input made by the user.
        PlayerInput();

        // Check to make sure the player's save data does not spawn them outside the map.
        //BoundaryChecks();



    }

    void FixedUpdate()
    {
        // Run function that controls the player's movement in fixed update for accurate physics interactions.
        PlayerMovement();
    }

    // Function to check for player input.
    void PlayerInput()
    {
        // Assign movement axis references.
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.K)) //Space for lance, j for AOE bomb
        {
            SpawnPlayerWeapon();
        }
        else if(Input.GetKey(KeyCode.J)){
            destroyAll();
        }
    }

    void PlayerMovement()
    {
        // Change rigidbody velocity (allow player to move) and do it in fixed update for accurate physics interactions.
        rb2d.velocity = new Vector2(horizontalMovement * playerSpeed, verticalMovement * playerSpeed);

        // Run function that changes the player's sprite based on movement.
        UpdateSpriteAnimation();

        // Update the last facing direction whenever movement occurs.
        if (Mathf.Abs(horizontalMovement) > 0.05f || Mathf.Abs(verticalMovement) > 0.05f)
        {
            lastFacingDirection = new Vector2(horizontalMovement, verticalMovement).normalized;
        }
    }

    // Temporary coroutine that changes the player's color when they collide with an enemy.
    //IEnumerator ColorChange()
    //{
    //    // Change player color to red, wait a set amount of time, then change color back to white.
    //    this.GetComponent<Renderer>().material.color = Color.red;
    //    yield return new WaitForSeconds(colorChangeTime);
    //    this.GetComponent<Renderer>().material.color = classColor;
    //}

    //// Function that handles the damage the player takes. 
    public void TakeDamage()
    {
       // If the player is not invincible, take away a life, flash red to indicate damage taken, and set them to invincible for a brief time.
       if (!invincible)
       {
           ChangeLives(livesLostOnHit);
        //    StartCoroutine(ColorChange()); // Run change color coroutine
        //    StartCoroutine(SetInvincible());
       }
    }

    //// Updates the sprite animation based on the direction the player is moving in.
    void UpdateSpriteAnimation()
    {

        animator.SetBool("isWalking", true);
       // Based on the direction of movement, set the sprite for that direction.
       if (horizontalMovement < 0)
       {
            animator.SetFloat("LastInputX", horizontalMovement);
           // Moving left, set movingLeft to true and others to false.
        //    SetAnimatorBools(true, false, false, false);
       }
       else if (horizontalMovement > 0)
        {
           // Moving right, set movingRight to true and others to false.
            //    SetAnimatorBools(false, true, false, false);
            
            animator.SetFloat("InputX", horizontalMovement);
        }
       else if (verticalMovement > 0)
       {
           // Moving up, set movingUp to true and others to false.
        //    SetAnimatorBools(false, false, true, false);
            animator.SetFloat("InputY", verticalMovement);
       }
       else if (verticalMovement < 0)
       {
           // Moving down, set movingDown to true and others to false.
        //    SetAnimatorBools(false, false, false, true);

            Debug.Log("hi");
            animator.SetFloat("LastInputY", verticalMovement);
       }
    }

    // Updates the Animator bools. Makes code look a lot better as the alternative was listing what is in this function in each if statement above.
    // void SetAnimatorBools(bool movingLeft, bool movingRight, bool movingUp, bool movingDown)
    // {
    //    playerAnimator.SetBool("moveLeft", movingLeft);
    //    playerAnimator.SetBool("moveRight", movingRight);
    //    playerAnimator.SetBool("moveUp", movingUp);
    //    playerAnimator.SetBool("moveDown", movingDown);
    // }
    

    //// Function that sets the player's class.
    //// Will probably change as I think player will have the ability to choose their class.
    //public void PlayerClass()
    //{
    //    // Try to load the class name saved in the player save data.
    //    try
    //    {
    //        PlayerData data = SaveSystem.LoadPlayer();
    //        className = data.playerClass;
    //    }
    //    // If it doesn't exist, an exception will occur, so give the player a random class.
    //    catch (Exception)
    //    {
    //        //Debug.Log("Exception: " + e);

    //        // Create a random number that uses the length of the list as the max value.
    //        float randomNum = Random.Range(0, playerClassName.Count);

    //        // Set the player's className to an int version of the random number variable. 
    //        // Use (int) just in case number haapens to be float.
    //        className = playerClassName[(int)randomNum];

    //        // Display the player's class in the console window.
    //        //Debug.Log($"Your class is: {className}");
    //    }
    //    // Whether the className was loaded from save data or randomly chosen, set the variables based on the className.
    //    finally
    //    {
    //        // Use a switch statement to change the player's color and speed based on their class.
    //        switch (className)
    //        {
    //            case "archer":
    //                // Player is archer, so use base color and make them quick.
    //                classColor = Color.white;
    //                playerSpeed = 7.0f;
    //                break;
    //            case "wizard":
    //                // Player is wizard, so make them cyan since it stands out and slow them down.
    //                classColor = Color.cyan;
    //                playerSpeed = 5.5f;
    //                break;
    //            case "blueberry":
    //                // Player is blueberry so make them blue and zoomin.
    //                Color blueberryColour = new Color(0.31f, 0.53f, 0.97f);
    //                classColor = Color.blue;
    //                playerSpeed = 8.5f;
    //                break;
    //            default:
    //                // If none of the classes above, use base color and base speed.
    //                classColor = Color.white;
    //                break;
    //        }

    //        // Once classColor is set, apply it.
    //        this.GetComponent<Renderer>().material.color = classColor;
    //    }
    //}

    void SpawnPlayerWeapon()
    {
       GameObject playerAttack;
       // Check to see if enough time has passed since the last weapon spawn to spawn another.
       if (Time.time - lastAttackTime < playerAttackCooldown)
       {
           return; // Not enough time has passed, so exit the function.
       }
    //    else if (Time.time - lastAttackTime < bombCoolDown && bombs == 0)
    //    {
    //        return; // Not enough time has passed, so exit the function.

    //    }
    //    else if (Time.time - lastAttackTime > bombCoolDown)
    //    {
    //        bombs += 3;
    //    }

       //Make an if statement that if the projectile has not been destroyed, then return 
       /*
        * if(hasFired == false){
        *   return;
        * }
        */
       if (Input.GetKey(KeyCode.K))
       {
            //if a second has passed, set it back to false
            if(Time.time < 1.0f)
            {
                hasFired = true;
            }
            else
            {
                hasFired = false;
            }
            

            if(hasFired == false)
            {
                // Spawn the attack at the player's position and give it a variable name.
                playerAttack = Instantiate(playerWeapon, playerTransform.position, Quaternion.identity);
                // Get the rigidbody of the player's attack.
                Rigidbody2D playerAttackRb = playerAttack.GetComponent<Rigidbody2D>();
                // As long as the playerAttack's rigidbody exits (does not equal null), run code below.
                if (playerAttackRb != null)
                {
                    // Set the direction the attack moves in the direction the player is facing.
                    playerAttackRb.velocity = lastFacingDirection * playerAttackSpeed;

                    // Calculate the angle based on the movement direction of the player.
                    // Once calculated, set the player's attack to that rotation.
                    float angle = Mathf.Atan2(lastFacingDirection.x, -lastFacingDirection.y) * Mathf.Rad2Deg;
                    playerAttack.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                else
                {
                    Debug.LogWarning("Rigidbody2D not found on player attack."); // Debug here in case issue occurs.
                }
            }
            else
            {
                Debug.Log("You did something wrong");
            }
           


    }
        //    else if (Input.GetKey(KeyCode.Z) && bombs >= 1)
        //    {
        //        // Spawn the attack at the player's position and give it a variable name.
        //        playerAttack = Instantiate(playerBomb, playerTransform.position, Quaternion.identity);
        //        // Get the rigidbody of the player's attack.
        //        Rigidbody2D playerAttackRb = playerAttack.GetComponent<Rigidbody2D>();
        //        // As long as the playerAttack's rigidbody exits (does not equal null), run code below.
        //        if (playerAttackRb != null)
        //        {
        //            // Set the direction the attack moves in the direction the player is facing.
        //            playerAttackRb.velocity = lastFacingDirection * playerAttackSpeed;

        //            // Calculate the angle based on the movement direction of the player.
        //            // Once calculated, set the player's attack to that rotation.
        //            float angle = Mathf.Atan2(lastFacingDirection.x, -lastFacingDirection.y) * Mathf.Rad2Deg;
        //            playerAttack.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //        }
        //        else
        //        {
        //            Debug.LogWarning("Rigidbody2D not found on player attack."); // Debug here in case issue occurs.
        //        }
        //        bombs -= 1;
        //        Debug.Log($"Bomb: {bombs}");
        //    else if (Input.GetKey(KeyCode.E))
        //    {
        //        // Spawn the attack at the player's position and give it a variable name.
        //        playerAttack = Instantiate(sword, playerTransform.position, Quaternion.identity);
        //        playerAttack.transform.SetParent(this.gameObject.transform);
        //    }

        lastAttackTime = Time.time; // Begin attack cooldown.       
    }

    //// Function is called when the weapon is destroyed. This then resets the cooldown timer, allowing the player to attack again.
    //public void WeaponDestroyed()
    //{
    //    lastAttackTime = Time.time - playerAttackCooldown;
    //}

    //// Function to make sure lives and score do not go outside their boundaries.
   void MinAndMaxChecks()
    {
       // If lives somehow go over the max, keep them at max.
       if (_lives > maxLives)
       {
           _lives = maxLives;
       }

       // If lives goes under 0, set it back to 0.
       if (_lives <= 0)
       {
           _lives = 0;
       }

       // If score somehow goes below 0, set it back to 0.
       if (_score < 0)
       {
           _score = 0;
       }

       //Slowly enhance your character every 10 hits
       // Ensure the _score > 0 check remains or else this if statment will cause false positives, resulting in the player gaining
       // lives even if their score is at 0. Checking that the score is above 0 ensures that does not happen.
       if(_score > 0 && _score % 100 == 0)
       {
           maxLives++;
           _lives++;
       }
    }
    //
    ///
    /// USE SCORE LATER
    //// Function to change score.
    public void ChangeScore(int scoreChange)
    {
       // Adjust score by the change amount.
       Score += scoreChange;

       // Run a check to make sure the max or minimum of lives and score are not hit. *Score does not have a max.
       MinAndMaxChecks();

       // Set the UI so it changes when score changes.
       SetUI();
    }
    //USE CHANGE LIVES LATER
    //// Function to change lives.
    public void ChangeLives(int livesChange)
    {
       // Try to change lives.
       try
       {
           // Adjust the lives by the change amount.
           Lives += livesChange;

           // Run a check to make sure the max or minimum of lives and score are not hit. *Score does not have a max.
           MinAndMaxChecks();

           // Set the UI so it changes when the lives change.
           SetUI();

           // After changing, if the lives are at or below 0, throw an exception made in the OutOfLivesException class.
           if (_lives <= 0)
           {
               throw new OutOfLivesException();
           }
       }
       // If an OutOfLiveException is caught, begin the zero lives remaining coroutine that ends the game and debug that the player is out of lives.
       catch (OutOfLivesException)
       {
           StartCoroutine(ZeroLivesRemaining());
           //Debug.Log("Can't continue because there are no more lives remaining!" + exception);
       }
    }
    //USE SETUI LATER
    //// Function to set UI.
    public void SetUI()
    {
       // Set the livesText to the text in "" + the current lives variable value.
       livesText.text = "Lives " + _lives;

       // Set the scoreText to the text in "" + the current score variable value.
       scoreText.text = _score + " Score";

    //    bombsText.text = "Bombs " + bombs;
    }

    public void destroyAll(){

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  
        if(potionCount >= 0){
            // For every enemy in the scene, destroy them
            foreach(GameObject Enemy in enemies){
                GameObject.Destroy(Enemy);
            }
            potionCount -= 1;
            
        }
        else{
            
           Debug.Log("No potion");
        }
            
    }

///USE ZEROLIVESREMAINING LATER
    //// Coroutine that starts when the player has no more lives.
    IEnumerator ZeroLivesRemaining()
    {
       // Stop time, turn on the game over display to play its animation
       // (animation update mode is set to unscaled time in the inspector to allow it to play while time is stopped),
       // wait until a little bit after the animation is done (use WaitForSecondsRealtime so it uses unscaled time,
       // allowing the wait time to work even if time scale is at 0), then run the event.
       Time.timeScale = 0f;
    //    gameOverDisplay.SetActive(true);
       yield return new WaitForSecondsRealtime(2.5f);
       gameIsOver();
    }

    //// Class that creates an exception that is used when there are no more lives remaining.
    public class OutOfLivesException : Exception
    {
       public OutOfLivesException() : base("Player ran out of lives!")
       {
       }
    }

    //// When player gets hit, set them to invincible for a brief time frame before allowing them to take damaage again.
    // private IEnumerator SetInvincible()
    // {
    //    invincible = true;
    //    yield return new WaitForSeconds(invincibleTime);
    //    invincible = false;
    // }

    //// Check the player's x and y position to ensure that they are not outside of the map's x or y (width or height).
    //void BoundaryChecks()
    //{
    //    if (transform.position.x > CreateMap.mapWidth || transform.position.x < -CreateMap.mapWidth
    //        || transform.position.y > CreateMap.mapHeight || transform.position.y < -CreateMap.mapHeight)
    //    {
    //        transform.position = new Vector3(0, 0, transform.position.z);
    //    }
    //}

}