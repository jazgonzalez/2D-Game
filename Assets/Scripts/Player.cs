using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb2D; //para modificar la fisica del jugador
    private float move;
    //variables for jumping
    public float jumpForce = 4; //upward force
    private bool isGrounded; //check if the player is currently touching the ground
    public Transform groundCheck; //reference to a point at the player's feet to check for ground
    public float groundRadius=0.1f; //radius of the circle to detect ground

    public LayerMask groundLayer; //determine what objects are "ground"
    private Animator animator; //animation variable
    //UI VARIABLES
    private int collectibles;
    public TMP_Text textCollectibles;

    //AUDIO VARIABLES
    public AudioSource audioSource;
    public AudioClip collectibleClip;
    public AudioClip  barrelClip; 

    //HEARTS VARIABLES
    public int lives =3; //number of lives
    public Image[] heartImages; //heart images

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialization of variables
        rb2D = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>(); 
    }
    

    // Update is called once per frame
    void Update()
    {
        move= Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(move*speed, rb2D.linearVelocity.y); //vector 2 D (horizontal,vertical)

        //flip the character based on movement on the x-direction
        if(move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move),1,1);
        
        //jump if we use the space button when we are on the ground
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce); //only modify the y direction to jump
        }
        // Send the movement value to the Animator. 
        // Mathf.Abs ensures the value is always positive so the "Run" animation triggers 
        // regardless of moving left or right.
        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y); //track jumping or falling
        animator.SetBool("IsGrounded",isGrounded);
    }

    //upate number of lives
    void updateHearts()
    {
        if(lives >=0 && lives< heartImages.Length)
        {
            heartImages[lives].enabled =false; //disabled the image of the heart
        }
    }
    void FixedUpdate()
    {
        //checks if the gorundcheck circle is overlapping with any ground layer
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
    // Helper function called by "Invoke" to reload the current scene
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //character collision with ojects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player collided with the collectible
        if(collision.transform.CompareTag("Collectible"))
        {
            //play a sound when it touches the strawberry
            audioSource.PlayOneShot(collectibleClip);
            Destroy(collision.gameObject); //destroy the collectible
            collectibles++; //counter for the collectibles
            textCollectibles.text = collectibles.ToString(); //convert from int to string to visualize
        }
        //if the player collided with the spike
        if(collision.transform.CompareTag("Spike") )
        {
            lives--;
            //update the UI
            if (lives>0 && lives < heartImages.Length)
            {
                heartImages[lives].enabled = false;
            }
            //if the player still has lives
            if (lives>0)
            {
                animator.SetTrigger("Hurt"); //activates the hurt animation
            
            }
            //if the player has no lives
            else
            {
                animator.SetTrigger("Die"); //activates the die animation
                // Delay restart by 1 second so the animation can be seen
                Invoke("RestartLevel", 1f);
            }
        }

        //if the player touches the bomb
        if(collision.transform.CompareTag("Bombs"))
        {
        
            collision.enabled = false;
            // activates the bomb
            Animator bombAnim = collision.GetComponent<Animator>();
            bombAnim.SetTrigger("explode"); 
            lives--;
            if (lives >= 0 && lives < heartImages.Length)
            {
                heartImages[lives].enabled = false;
            }

            if (lives > 0)
            {
                animator.SetTrigger("Hurt");
            }
            else
            {
                animator.SetTrigger("Die");
                Invoke("RestartLevel", 1f);
            }

            //destroy the bomb
            Destroy(collision.gameObject, 0.5f); 
        }

        //if the player touches the barrel
        if(collision.transform.CompareTag("Barrel"))
        {
            //play a sound when it jumps
            audioSource.PlayOneShot(barrelClip);
            // Calculate the direction of the knockback by getting the vector from the collision point to the player
            Vector2 knockbackDir = (rb2D.position - (Vector2)collision.transform.position).normalized;   
            rb2D.linearVelocity = Vector2.zero;
            // Apply an immediate physical impulse to push the player away from the barrel
            rb2D.AddForce(knockbackDir*3,ForceMode2D.Impulse);
            //get all box colliders attached to the barrel
            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();
            //Loop through each collider and disable it to prevent further collisions while the barrel is being destroyed
            foreach (BoxCollider2D col in colliders)
            {
                col.enabled =false;
            }
            // Enable the Animator component to play the barrel's destruction or interaction animation
            collision.GetComponent<Animator>().enabled=true;
            //destroy the barrel after 0.5s
            Destroy(collision.gameObject,0.6f);

        }

        //if the player touches the floor
        if(collision.transform.CompareTag("DeathZone") || collision.transform.CompareTag("Enemy"))
        {
            //triggers death animation
            animator.SetTrigger("Die");
           // Quick restart after falling
            Invoke("RestartLevel", 0.5f);
        }

    }
}
