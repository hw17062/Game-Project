using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveUp()
    {
        player.transform.Translate(1.0f, 0.0f, 0.0f);
    }

    void MoveDown()
    {
        player.transform.Translate(-1.0f, 0.0f, 0.0f);
    }

    void MoveLeft()
    {
        player.transform.Translate(0.0f, 0.0f, 1.0f);
    }

    void MoveRight()
    {
        player.transform.Translate(1.0f, 0.0f, -1.0f);
    }

    void MoveUpLeft()
    {
        player.transform.Translate(1.0f, 0.0f, 1.0f);
    }

    void MoveUpRight()
    {
        player.transform.Translate(1.0f, 0.0f, -1.0f);
    }

    void MoveDownLeft()
    {
        player.transform.Translate(-1.0f, 0.0f, 1.0f);
    }

    void MoveDownRight()
    {
        player.transform.Translate(-1.0f, 0.0f, -1.0f);
    }
}
