using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightedObject : MonoBehaviour
{
    private MeshRenderer originalMesh;
    [SerializeField] private MeshRenderer highlightedMesh;
    [SerializeField] private float highlightDistance = 5f;
    private Transform player;

    void Start()
    {
        originalMesh = GetComponent<MeshRenderer>();

        if (highlightedMesh == null)
        {
            highlightedMesh = GetComponentInChildren<MeshRenderer>();
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;

        Highlight(false);
    }

    public void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        Highlight(distanceToPlayer <= highlightDistance);
    }

    public void Highlight(bool state)
    {
        if (highlightedMesh != null)
        {
            highlightedMesh.enabled = state;
            originalMesh.enabled = !state;
        }
    }
}