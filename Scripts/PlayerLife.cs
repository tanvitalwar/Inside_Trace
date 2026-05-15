using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static; // Stop the player from moving
        anim.SetTrigger("death"); // Trigger the death animation

    }
    private void RestartLevel()
    {
        SceneManager.LoadScene("End");   // load the dedicated game-over scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        // Reload the current scene
    }
}
