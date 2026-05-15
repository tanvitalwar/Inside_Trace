using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    // -------- Movement & animation --------
    private Animator ani;
    private Rigidbody2D rBody;

    [SerializeField] private float moveSpeed = 0.5f;    // tweak in Inspector

    // -------- Line-drawing (Version A) --------
    [Header("Line drawing")]
    [SerializeField] private Material lineMaterial;     // drag in a pure-black unlit/Sprite-Default material
    [SerializeField] private float lineWidth  = 0.05f;  // world units
    [SerializeField] private float minPointGap = 0.1f;  // add a point every X units

    private LineRenderer activeLine;
    private Vector3      lastPoint;

    /* -------------------------------------------------------------------------- */

    void Start()
    {
        ani  = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        //HandleLineDrawing();
    }

    /* =============================  MOVEMENT  ============================= */

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");

        // update animator blend tree
        if (horizontal != 0) { ani.SetFloat("Horizontal", horizontal); ani.SetFloat("Vertical", 0); }
        if (vertical   != 0) { ani.SetFloat("Vertical",   vertical);   ani.SetFloat("Horizontal", 0); }

        Vector2 dir = new Vector2(horizontal, vertical).normalized;
        ani.SetFloat("Speed", dir.magnitude);

        rBody.linearVelocity = dir * moveSpeed;   // Rigidbody2D uses .velocity in 2-D physics
    }

    /* ==========================  LINE  DRAWING  ========================== */

    // void HandleLineDrawing()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //         StartLine();

    //     if (Input.GetKey(KeyCode.Space) && activeLine != null)
    //         AppendPointIfFarEnough();

    //     if (Input.GetKeyUp(KeyCode.Space))
    //         activeLine = null;   // stop adding points; line stays in scene
    // }

void StartLine()
{
    GameObject go = new GameObject("Line");
    activeLine    = go.AddComponent<LineRenderer>();

    // ★ Fallback material (if you forgot to drag one in)
    if (lineMaterial == null)
        lineMaterial = new Material(Shader.Find("Sprites/Default"));
    activeLine.material = lineMaterial;

    activeLine.useWorldSpace  = true;
    activeLine.widthCurve     = AnimationCurve.Constant(0, 1, lineWidth);
    activeLine.numCapVertices = 2;          // rounded ends

    // ★ Make sure it's on top of tiles & background
    activeLine.sortingLayerName = "Default";   // change if you have a custom layer
    activeLine.sortingOrder     = 10;          // bigger number → drawn later

    // --- add TWO identical points right away ---
    Vector3 p = transform.position;
    activeLine.positionCount = 2;             // ★ must be 2+
    activeLine.SetPosition(0, p);
    activeLine.SetPosition(1, p);
    lastPoint = p;                            // for distance test
}


    void AppendPointIfFarEnough()
    {
        if (Vector3.Distance(lastPoint, transform.position) >= minPointGap)
            AddPoint(transform.position);
    }

    void AddPoint(Vector3 p)
    {
        activeLine.positionCount += 1;
        activeLine.SetPosition(activeLine.positionCount - 1, p);
        lastPoint = p;
    }
}
