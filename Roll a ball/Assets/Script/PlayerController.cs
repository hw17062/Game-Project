using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public float jumpSpeed;
    public GameObject gameBoard;
    public float cellSize;

    private Rigidbody rb;
    private int count;
    private float gridX;
    private float gridY;


    void Start()
	{
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        //gameBoard.
        this.gridX = 0.0f;
        this.gridY = 0.0f;
    }

    void FixedUpdate()
	{
        float moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveVertical = CrossPlatformInputManager.GetAxis("Vertical");

        this.gridX += moveHorizontal;
        this.gridY += moveVertical;
        Debug.Log("x:" + this.gridX + " y:" + this.gridY);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += jumpSpeed * Vector3.up;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 9)
        {
            winText.text = "You win!";
        }
    }

   
}
