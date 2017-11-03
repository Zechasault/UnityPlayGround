using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    const float VerticalLineGradient = 1e5f; //100000
    float gradient;
    float y_intercept;
    Vector2 pointOfLine1, pointOfLine2;

    float gradientPerpendicular;
    bool approachSide;

    public Line(Vector2 pointOfLine, Vector2 pointPerpendicular)
    {
        float dx = pointOfLine.x - pointPerpendicular.x;
        float dy = pointOfLine.y - pointPerpendicular.y;
        if (dx == 0f)
        {
            gradientPerpendicular = VerticalLineGradient;
        }
        else
        {
            gradientPerpendicular = dx / dy;
        }

        //gradient = pente
        //gradient of a line multiplied by the gradient of a perpendicular line = -1
        if (gradientPerpendicular == 0)
        {
            gradient = VerticalLineGradient;
        }
        else
        {
            gradient = -1 / gradientPerpendicular;
        }
        // y=mx+c -> c=y-mx
        y_intercept = pointOfLine.y - gradient * pointOfLine.x;
        pointOfLine1 = pointOfLine;
        pointOfLine2 = pointOfLine + new Vector2(1, gradient); //we don't really care, can be anywhere on the line
        approachSide = false; //a struct need every one of its fields filled with a value before calling methods
        approachSide = GetSide(pointPerpendicular);
    }

    bool GetSide(Vector2 p)
    {
        return (p.x - pointOfLine1.x) * (pointOfLine2.y - pointOfLine1.y) > (p.y - pointOfLine1.y) * (pointOfLine2.x - pointOfLine1.x);
    }

    public bool HasCrossedLine(Vector2 point)
    {
        return GetSide(point) != approachSide;
    }

    public void DrawWgizmos(float length)
    {
        Vector2 lineDir = new Vector2(1, gradient).normalized;
        Vector2 lineCentre = new Vector2(pointOfLine1.x, pointOfLine1.y);
        Gizmos.DrawLine(lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f);
    }
}
