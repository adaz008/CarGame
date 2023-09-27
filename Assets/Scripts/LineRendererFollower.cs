using UnityEngine;

public class LineRendererFollower : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private Transform player;

    private int lastVisibleIndex = 0;
    private float distanceTraveled = 0f;
    private float lineRendererLength = 0f;

    Vector3[] positions;

    private void Start()
    {
        positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        
        // Calculate the length of the Line Renderer path
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            lineRendererLength += Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));

            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) + transform.position);
        }

        player = GameObject.FindGameObjectWithTag("Car").transform;
    }

    private void Update()
    { 
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            //Vector3 linePoint = lineRenderer.GetPosition(i);
            //Vector3 carPoint = player.position;
            //float angle = Vector3.SignedAngle(linePoint, carPoint, player.forward);

            Vector3 linePoint = lineRenderer.GetPosition(i);
            Vector3 carPoint = player.position;

            // Create new Vector2 variables that only contain the X and Z values of the line point and the car point
            Vector2 linePoint2D = new Vector2(linePoint.x, linePoint.z);
            Vector2 carPoint2D = new Vector2(carPoint.x, carPoint.z);

            // Get the signed angle between the two Vector2 variables
            float angle = linePoint.y == -100f ? 0f :  Vector2.SignedAngle(linePoint2D - carPoint2D, player.forward);

            Debug.Log(i + "   " + angle);

                if (angle > 0f)
                {

                lineRenderer.SetPosition(i, positions[i]); // show point
                }
                else
                {
                lineRenderer.SetPosition(i, new Vector3(0f, -100f, 0f)); // hide point
                }
        }
    }
}

