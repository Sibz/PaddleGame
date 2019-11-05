using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public GameObject Ball;
    public GameObject PaddleCollider;

    public float RayDistance = 1;

    public LayerMask PaddleLayer;

    [Tooltip("Max angle of paddle collider change when hitting the very end of the paddle.")]
    [Range(1, 90)]
    public float AngularChange = 45;

    private Rigidbody2D m_BallRigidBody2D;
    private SpriteRenderer m_PaddleSpriteRenderer;
    private void Start()
    {
        m_BallRigidBody2D = Ball.GetComponent<Rigidbody2D>();
        m_PaddleSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //Get the direction the ball is travelling
        //This is basically the velocity, however we normalize this
        Vector2 direction = m_BallRigidBody2D.velocity.normalized;

        //Ray cast towards the paddle
        RaycastHit2D hit = Physics2D.Raycast(Ball.transform.position, direction, RayDistance, PaddleLayer);

        // Draw a ray we can see in scene view
        Debug.DrawRay(Ball.transform.position, direction * RayDistance, Color.red);

        if (hit)
        {
            PaddleCollider.SetActive(true);

            // Reset our paddle collider so it is flat and at position of paddle
            PaddleCollider.transform.rotation = new Quaternion();
            PaddleCollider.transform.position = transform.position;

            // If we have hit we want to rotate the ball around that point
            Vector2 hitPoint = hit.point; // Worldspace

            // Transform above into local space for the paddle
            // so we can figure how far along paddle hit was
            Vector2 localHitPoint = hitPoint - (Vector2)transform.position;

            // Calculate the offseet on the paddle that we hit
            // this will be -1 to 1, with 0 being centre;
            float offset = /*point.x / length*/ localHitPoint.x / (m_PaddleSpriteRenderer.bounds.extents.x);

            // Get our incoming angle
            // -180' to 180'
            var incomingAngle = Vector2.SignedAngle(direction, Vector3.down);

            // Calculate our angularchange based on hit position
            float adjustedAngularChange = -AngularChange * offset;

            // We want to translate the incoming angle based on the offset
            // but if it's a really shallow angle, bounce it more upwards.
            // (This is to prevent overly sideways movment)
            // Setting the paddle to half incoming angle will reflect directly up
            // so we use this for our corrective figure
            float correctionForShallowHitsAngle = -incomingAngle / 2;

            // Now we can get a value between to two based on how
            // sideways the ball in coming in from
            // if its 90' then 100% use the correctionForShallowHitsAngle
            // if it's 0' then 100% use the adjustedAngularChange
            // we can use linear interpolation (lerp) for this.

            float computedAngularChange =
                Mathf.Lerp(adjustedAngularChange, correctionForShallowHitsAngle, Mathf.Abs(incomingAngle) / 90);

            // Rotate around hit point with angle adjusted by above percent
            PaddleCollider.transform.RotateAround(hitPoint, Vector3.forward, computedAngularChange);

        }
        else
        {
            PaddleCollider.SetActive(false);
        }
    }
}
