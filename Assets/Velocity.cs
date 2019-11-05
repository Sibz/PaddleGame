using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public Vector2 InitialVelocity = new Vector2(2, -2);

    public float SpeedIncrement = 0.5f;

    public float SpeedIncrementInterval = 3;

    private Rigidbody2D m_RigidBody;
    private Vector3 m_InitialPosition;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_RigidBody.velocity = InitialVelocity;
        m_InitialPosition = transform.position;
        StartCoroutine(IncrementSpeed());
    }

    public void ResetBall()
    {
        transform.position = m_InitialPosition;
        m_RigidBody.velocity = InitialVelocity;
    }

    private IEnumerator IncrementSpeed()
    {
        yield return new WaitForSeconds(SpeedIncrementInterval);
        m_RigidBody.velocity += m_RigidBody.velocity * SpeedIncrement;
        StartCoroutine(IncrementSpeed());
    }
}
