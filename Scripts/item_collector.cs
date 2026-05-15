using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;   // ← NEW

public class item_collector : MonoBehaviour
{
    /* -----------------------  Drawing state  ----------------------- */
    private LineRenderer activeLine;
    private Vector3      lastPoint;

    private Color  currentColor = Color.white;
    private string colourName   = "White";

    /* -----------------------  Inspector refs  ----------------------- */
    [SerializeField] private Text  cherriesText;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float minPointGap = 0.1f;
    [SerializeField] private float lineWidth   = 0.05f;

    /* -----------------------  Progress tracking  ------------------- */
    // Track whether each colour-fruit has been collected
    private bool gotCherry, gotBlueberry, gotKiwi;

    /* -----------------------  Pick-ups  ---------------------------- */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool collectedSomething = false;      // helper flag

        if (collision.CompareTag("Cherry") && !gotCherry)
        {
            currentColor  = Color.red;
            colourName    = "Red";
            gotCherry     = true;
            collectedSomething = true;
        }
        else if (collision.CompareTag("Blueberry") && !gotBlueberry)
        {
            currentColor  = Color.blue;
            colourName    = "Blue";
            gotBlueberry  = true;
            collectedSomething = true;
        }
        else if (collision.CompareTag("Kiwi") && !gotKiwi)
        {
            currentColor  = Color.green;
            colourName    = "Green";
            gotKiwi       = true;
            collectedSomething = true;
        }

        if (!collectedSomething) return;      // either not a fruit or duplicate

        Destroy(collision.gameObject);

        if (cherriesText != null)
            cherriesText.text = "Current Colour: " + colourName;

        /* ------------  All three collected?  ---------------------- */
        if (gotCherry && gotBlueberry && gotKiwi)
        {
            // Optional: disable further triggers so this fires once
            GetComponent<Collider2D>().enabled = false;
            SceneManager.LoadScene("End");
        }
    }

    /* -----------------------  Update loop  ------------------------- */
    void Update() => HandleLineDrawing();

    /* =====================  Line-drawing logic  ==================== */
    void HandleLineDrawing()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartLine();

        if (Input.GetKey(KeyCode.Space) && activeLine != null)
            AppendPointIfFarEnough();

        if (Input.GetKeyUp(KeyCode.Space))
            activeLine = null;
    }

    void StartLine()
    {
        GameObject go  = new GameObject("Line");
        activeLine     = go.AddComponent<LineRenderer>();

        if (lineMaterial == null)
            lineMaterial = new Material(Shader.Find("Sprites/Default"));
        activeLine.material = lineMaterial;

        activeLine.useWorldSpace   = true;
        activeLine.widthCurve      = AnimationCurve.Constant(0, 1, lineWidth);
        activeLine.numCapVertices  = 2;
        activeLine.sortingLayerName = "Default";
        activeLine.sortingOrder     = 10;

        activeLine.startColor = currentColor;
        activeLine.endColor   = currentColor;

        Vector3 p = transform.position;
        activeLine.positionCount = 2;
        activeLine.SetPosition(0, p);
        activeLine.SetPosition(1, p);
        lastPoint = p;
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

