using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        //Bullet Logic here

        Destroy(gameObject);
    }
}
