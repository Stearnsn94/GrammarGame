using UnityEngine;

public class Enemy_detection_cone : MonoBehaviour
{
    bool is_detected;

    //have to make a cone collider from scratch
    PolygonCollider2D cone_collider;

    private void Awake()
    {
        cone_collider = gameObject.AddComponent<PolygonCollider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private PolygonCollider2D view()
    {
        Vector2[] points = new Vector2[3];
        points[0] = Vector2.zero; // tip of the cone
        points[1] = Quaternion.Euler(0, 0, 30) * Vector2.right * 5; // one side
        points[2] = Quaternion.Euler(0, 0, -30) * Vector2.right * 5; // other side
        cone_collider.SetPath(0, points);
        cone_collider.isTrigger = true;

        return cone_collider;
    }

    private bool on_trigger_enter(Collider2D cone_view)
    {

    
        return false;
    }
}
