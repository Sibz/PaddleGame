using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBallScript : MonoBehaviour
{
    public Velocity BallVelocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BallVelocity.ResetBall();
    }
}
