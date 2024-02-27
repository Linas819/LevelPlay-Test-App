using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer line;
    public  List<Transform> positions;
    GameController gameController;
    public void Start()
    {
        line.positionCount = 1;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    // Update is called once per frame
    public void LineSetUp(int currentProgression)
    {
        if (currentProgression > 1)
        {
            SetLinePoint(currentProgression - 2, currentProgression - 1);
        }
        if (currentProgression == positions.Count) //checking if the last button was pressed
        {
            StartCoroutine(EndLevel(currentProgression));
        }
    }

    public void SetLinePoint(int pointStart, int pointEnd)
    {
        line.positionCount++; //increases the number of line positions, for the Line Renderer
        line.SetPosition(pointStart, positions[pointStart].position);
        line.SetPosition(pointStart+1, positions[pointEnd].position);
        StartCoroutine(LineDraw(pointStart, pointEnd));
    }
    IEnumerator LineDraw(int pos1, int pos2)
    {
        float time = 2;
        Vector3 orig = line.GetPosition(pos1);
        Vector3 orig2 = line.GetPosition(pos2);
        Vector3 newpos;
        for (float t=0; t < time; t += Time.deltaTime)
        {
            if (line.positionCount > pos1 + 2) //checking if the line positions count has not increased due to a new positions made.
            {
                line.SetPosition(pos1+1, orig2); //ends the animation by setting to the completed point
                break;
            }
            newpos = Vector3.Lerp(orig, orig2, t / time); //new end position, gottent by setting the distance between two points.
            line.SetPosition(pos1+1, newpos); //sets the line rendere end to the temporary new positions
            yield return null;
        }
    }
    IEnumerator EndLevel(int currentProgression)
    {
        yield return new WaitForSeconds(2);
        SetLinePoint(currentProgression - 1, 0); //draws the last line
        yield return new WaitForSeconds(3);
        line.positionCount = 1; //removes the lines
        gameController.EndLevel(); //stats the end level
    }
    public void PositionsList(Transform position)
    {
        positions.Add(position.transform); //contains the positions of the points
    }
}