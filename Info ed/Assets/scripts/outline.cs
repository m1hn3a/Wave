using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BuildZoneOutline : MonoBehaviour
{
    public Color outlineColor = Color.green;
    public float lineWidth = 0.05f;

    private LineRenderer lr;
    private PolygonCollider2D poly;
    private BoxCollider2D box;
    private CircleCollider2D circle;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.loop = true;
        lr.positionCount = 0;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = outlineColor;
        lr.endColor = outlineColor;

        poly = GetComponent<PolygonCollider2D>();
        box = GetComponent<BoxCollider2D>();
        circle = GetComponent<CircleCollider2D>();

        DrawOutline();
    }

    void DrawOutline()
    {
        if (poly != null)
        {
            Vector2[] points = poly.points;
            lr.positionCount = points.Length;

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 worldPos = transform.TransformPoint(points[i]);
                lr.SetPosition(i, worldPos);
            }
        }
        else if (box != null)
        {
            Vector3 size = box.size * 0.5f;

            Vector3[] corners = new Vector3[]
            {
                new Vector3(-size.x, -size.y),
                new Vector3(-size.x,  size.y),
                new Vector3( size.x,  size.y),
                new Vector3( size.x, -size.y)
            };

            lr.positionCount = 4;

            for (int i = 0; i < 4; i++)
                lr.SetPosition(i, transform.TransformPoint(corners[i]));
        }
        else if (circle != null)
        {
            int segments = 40;
            lr.positionCount = segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * circle.radius;
                lr.SetPosition(i, transform.TransformPoint(pos));
            }
        }
    }
}
