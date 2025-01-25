
using UnityEngine;

public class CalculateAnnulus
{
    private Vector3 Center = Vector3.zero;
    private float InnerRadius = 1f;
    private float OuterRadius = 3f;
    
    public CalculateAnnulus(Vector3 center, float innerRadius, float outerRadius)
    {
        this.Center = center;
        this.InnerRadius = innerRadius;
        this.OuterRadius = outerRadius;
    }

    public Vector2 GetRandomPointInAnnulus2D()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Mathf.Sqrt(Random.Range(InnerRadius * InnerRadius, OuterRadius * OuterRadius));

        float x = Center.x + radius * Mathf.Cos(angle);
        float y = Center.y + radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}