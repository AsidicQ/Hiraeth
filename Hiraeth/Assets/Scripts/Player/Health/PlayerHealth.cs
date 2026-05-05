using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<float, float> OnHealthChanged;

    [Header("Health")]
    private float currentHealth;
    [SerializeField] private float maxHealth;
    //private PlayerHealthBar healthBar;

    //public DeathScreen deathScreen;
    [SerializeField] private float fadeDurationToDeathScreen;
    [SerializeField] private float timeTarget;

    public static bool isDead;

    public AudioSource deathSound;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        //healthBar = GetComponent<PlayerHealthBar>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        //healthBar.UpdateBar(currentHealth, maxHealth);

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Death()
    {
        isDead = true;

        //deathScreen.StartCoroutine(deathScreen.ShowDeathScreen(fadeDurationToDeathScreen));
        //deathScreen.StartCoroutine(deathScreen.SlowDownTime(timeTarget));

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        CameraMovement.canLook = false;
        Movement.canMove = false;
        deathSound.Play();
    }
}
