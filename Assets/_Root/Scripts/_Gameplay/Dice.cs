using Pancake;
using Pancake.Apex;
using Pancake.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : GameComponent
{
    [Header("Dev config")]
    [SerializeField] private float minTorqueForce = -500f;
    [SerializeField] private float maxTorqueForce = 500f;
    [SerializeField] private float raycastDistance = .6f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField, Group("Event")] private ScriptableEventNoParam rollEvent;
    [SerializeField, Group("Event")] private ScriptableEventNoParam diceRollEvent;
    [SerializeField] private Rigidbody rigidbody;
    
    [SerializeField] [ReadOnly] public FaceUpType faceUpType = FaceUpType.Shrimp;
    [SerializeField] [ReadOnly] public bool isDoneRoll;
    [SerializeField] [ReadOnly] private bool isRoll;
    [SerializeField] [ReadOnly] private bool isSleeping = true;
    [SerializeField] [ReadOnly] private bool isGrounded = true;
    
    void Start()
    {
        rollEvent.OnRaised += OnRollDice;
    }
    
    void OnDestroy()
    {
        rollEvent.OnRaised -= OnRollDice;
    }

    protected override void Tick()
    {
        base.Tick();
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayerMask);
        isSleeping = rigidbody.velocity.magnitude == 0;

        // if (!isSleeping && !isGrounded)
        // {
        //     isRoll = true;
        // }
        
        if (isSleeping && isGrounded && isRoll)
        {
            float xDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
            float yDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
            float zDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

            if (xDot == -1)
            {
                faceUpType = FaceUpType.Deer;
            }
            else if (xDot == 1)
            {
                faceUpType = FaceUpType.Shrimp;
            }
            
            if (yDot == -1)
            {
                faceUpType = FaceUpType.Crab;
            }
            else if (yDot == 1)
            {
                faceUpType = FaceUpType.Fish;
            }
            
            if (zDot == -1)
            {
                faceUpType = FaceUpType.Chicken;
            }
            else if (zDot == 1)
            {
                faceUpType = FaceUpType.Gourd;
            }
            
            isRoll = false;
            isDoneRoll = true;
            
            diceRollEvent.Raise();
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * raycastDistance);
    }

    private void OnRollDice()
    {
        
        rigidbody.AddTorque(Random.Range(minTorqueForce, maxTorqueForce), Random.Range(minTorqueForce, maxTorqueForce), Random.Range(minTorqueForce, maxTorqueForce));
    }

    public void ResetRoll()
    {
        isDoneRoll = false;
    }
}

public enum FaceUpType
{
    Gourd,
    Crab,
    Fish,
    Shrimp,
    Chicken,
    Deer,
}