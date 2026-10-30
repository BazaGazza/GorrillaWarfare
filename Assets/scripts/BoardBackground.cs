using UnityEngine;

public class BoardBackground : MonoBehaviour
{
    [Header("Target UI Element")]
    public SpriteRenderer backgroundImage; // Drag your Panel/Image component here

    [Header("Gradient & Speed Settings")]
    public Gradient colorGradient; // Pick your color keys in the Inspector
    public float cycleSpeed = 0.2f; // Adjust how fast colors transition (lower = smoother/slower)

    void Start()
    {
        // Auto-assign Image component if attached to the same GameObject
        if (backgroundImage == null)
            backgroundImage = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (backgroundImage != null && colorGradient != null)
        {
            // Smoothly bounce time back and forth between 0.0 and 1.0
            float t = Mathf.PingPong(Time.time * cycleSpeed, 1f);

            // Evaluate the gradient at position 't' and set the UI panel color
            backgroundImage.color = colorGradient.Evaluate(t);
        }
    }
}
