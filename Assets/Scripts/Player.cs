using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 0f;
    [SerializeField] private float maxSpeed; // Max speed of the player is set in the inspector
    [SerializeField] private float rotationGrades; // Rotation speed of the player is set in the inspector

    private Animator animator, fireAnimator;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        animator = GetComponent<Animator>(); // Get the animator
        fireAnimator = transform.GetChild(0).GetComponent<Animator>(); // Get the fire animator
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Move();
        }
    }

    private void Move() // Player movement and rotation
    {
        float rotationMovement = Input.GetAxis("Horizontal");
        float movement = Input.GetAxis("Vertical");
        if (rotationMovement < 0) // If the player is rotating to the left
        {
            transform.Rotate(0.0f, 0.0f, rotationGrades * Time.deltaTime, Space.Self); // Rotate the player
            animator.SetTrigger("left");
        }
        else if (rotationMovement > 0) // If the player is rotating to the right
        {
            transform.Rotate(0.0f, 0.0f, -rotationGrades * Time.deltaTime, Space.Self); // Rotate the player
            animator.SetTrigger("right");
        } else
        {
            animator.SetTrigger("idle"); // If the player is not rotating, set the idle animation
        }

        if (movement > 0 && speed < maxSpeed) // If the player is moving forward and the speed is less than the max speed
        { 
            speed += 0.05f;
            fireAnimator.SetBool("going", true);
        } 
        else if(movement < 0 && speed > 0) // If the player is moving backwards and the speed is greater than 0
        {
            speed -= 0.025f;
            fireAnimator.SetBool("going", false); 
        }
        else if (movement == 0 && speed > 0) // If the player is not moving and the speed is greater than 0
        {
            speed -= 0.005f;
            fireAnimator.SetBool("going", false);
        }
        else if (movement <= 0 && speed <= 0) // If the player is moving backwards or is not moving and the speed is less than or equal to 0
        {
            speed = 0f;
            fireAnimator.SetBool("going", false);
        }
        else if (movement > 0 && speed > maxSpeed) // If the player is moving forward and the speed is greater than the max speed
        {
            speed = maxSpeed;
            fireAnimator.SetBool("going", true);
        }
        transform.Translate(Vector3.up * speed * Time.deltaTime); // Move the player forward or backwards depending on the speed
        // For debug
        if(Input.GetKey(KeyCode.Space)) // If the player presses the space key, the player will be teleported to the center of the screen
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "limit") // If the player hits the limit, it will be teleported to the other side
        {
            if (collision.transform.position.x != 0) // If the limit is vertical
            {
                if(collision.transform.position.x > 0) // If the limit is on the right
                {
                    transform.position = new Vector3(transform.position.x * -1 + 0.1f, transform.position.y, 0); // Teleport to the left, adding 0.1f to avoid collision
                }
                else
                {
                    transform.position = new Vector3(transform.position.x * -1 - 0.1f, transform.position.y, 0); // Teleport to the right, substracting 0.1f to avoid collision
                }
            }
            else
            {
                if (collision.transform.position.y > 0) // If the limit is on the top
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y * -1 + 0.1f, 0); // Teleport to the bottom, adding 0.1f to avoid collision
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y * -1 - 0.1f, 0); // Teleport to the top, substracting 0.1f to avoid collision
                }
            }
        }
    }
}
