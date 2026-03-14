using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
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

    void FixedUpdate()
    {
        //checks if the gorundcheck circle is overlapping with any ground layer
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
    //character collision with ojects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player collided with the collectible
        if(collision.transform.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject); //destroy the collectible
            collectibles++; //counter for the collectibles
            textCollectibles.text = collectibles.ToString(); //convert from int to string to visualize
        }
        //if the player collided with the bomb
        if(collision.transform.CompareTag("Bombs"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reinitialice the game
        }
    }
}
