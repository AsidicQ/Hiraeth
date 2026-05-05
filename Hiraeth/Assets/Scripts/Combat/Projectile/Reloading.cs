using TMPro;
using UnityEngine;
using System.Collections;

public class Reloading : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI reloadingText;

    [Header("Input")]
    public KeyCode reloadKey = KeyCode.R;

    [Header("References")]
    public HandSway weaponSway;
    private GunBase gunBase;

    public Animator animator;

    void Start()
    {
        weaponSway = GetComponentInParent<HandSway>();
        animator = GetComponentInParent<Animator>();

        gunBase = GetComponentInParent<GunBase>();

        UpdateAmmo();
    }

    public void Initialize(TextMeshProUGUI ammoCounter, TextMeshProUGUI reloadingText)
    {
        this.ammoCounter = ammoCounter;
        this.reloadingText = reloadingText;
        reloadingText.enabled = false;
    }

    void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (gunBase == null) return;

        if (Input.GetKeyDown(reloadKey) && gunBase.currentAmmo < gunBase.maxAmmo && gunBase.reserveAmmo > 0
            && !gunBase.isReloading && !GunBase.isAiming)
            StartCoroutine(gunBase.Reload());

        if (gunBase.isReloading)
        {
            animator.SetBool("isAimingAnim", false);
        }
    }

    public void UpdateAmmo()
    {
        if (ammoCounter != null)
        {
            ammoCounter.text = gunBase.currentAmmo + " / " + gunBase.reserveAmmo;
        }
    }

    public IEnumerator PlayAfterAnimation(string firstAnimName, string NextAnimName)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        while (!info.IsName(firstAnimName))
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        while (info.normalizedTime < 1.0f)
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        animator.Play(NextAnimName);
    }
}