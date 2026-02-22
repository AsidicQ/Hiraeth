using UnityEngine;

public class FallAnim : MonoBehaviour
{
    [Header("Anim")]
    public ParticleSystem fallParticles;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float fallVelocityThreshold = -10f;
    private bool wasaInAir = false;
    private float maxFallVelocity;

    [Header("References")]
    public Movement movement;
    public Floating floating;

    void Start()
    {
        movement = GetComponent<Movement>();
        floating = GetComponent<Floating>();
    }

    private void Update()
    {
        if (movement.state == Movement.MovementState.Air && !floating.isFloating)
        {
            wasaInAir = true;

            if (movement.rb.linearVelocity.y < maxFallVelocity)
            {
                maxFallVelocity = movement.rb.linearVelocity.y;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool isGround = ((1 << collision.gameObject.layer) & groundLayer) != 0;

        if (isGround && wasaInAir)
        {
            CheckForLanding();
        }
    }

    void CheckForLanding()
    {
        if (maxFallVelocity <= fallVelocityThreshold)
        {
            float impactStrength = Mathf.InverseLerp(-2f, -20f, maxFallVelocity);
            float sizeMultiplier = Mathf.Lerp(0.5f, 2.5f, impactStrength);

            var main = fallParticles.main;
            main.startSizeMultiplier = sizeMultiplier;

            fallParticles.Play();
        }

        maxFallVelocity = 0f;
        wasaInAir = false;
    }
}