using Pancake;
using Pancake.Apex;
using Pancake.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : GameComponent
{
    [Header("Dev config")]
    [SerializeField] private float raycastDistance = .6f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField, Group("Event")] private ScriptableEventNoParam rollEvent;
    [SerializeField, Group("Event")] private ScriptableEventNoParam diceRollEvent;
    [SerializeField] private Rigidbody rigidbody;
    
    [SerializeField] [ReadOnly] public bool isRoll;
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

        if (isSleeping && isGrounded && isRoll)
        {
            float xDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
            float yDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
            float zDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

            diceRollEvent.Raise();
            Debug.Log("oke");
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * raycastDistance);
    }

    private void OnRollDice()
    {
        isRoll = true;
        rigidbody.AddTorque(Random.Range(0, 500), Random.Range(0,500), Random.Range(0, 500));
    }

    public void ResetRoll()
    {
        isRoll = false;
    }
}
