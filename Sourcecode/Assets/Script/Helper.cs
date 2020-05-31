using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class Helper
{
    public static int GetHighestNummber(int[] _input)
    {
        return _input.Max();
    }

    public static float GetHighestNummber(float[] _input)
    {
        return _input.Max();
    }

    public static int GetLowestNummber(int[] _input)
    {
        return _input.Min();
    }

    public static float GetLowestNummber(float[] _input)
    {
        return _input.Min();
    }

    #region -GetMyBoxOrder-
    public static Vector3 GetBotLeft(Vector3[] _input)
    {
        Vector3 output = new Vector3(float.MaxValue, 0, float.MaxValue);
        for (int i = 0; i < _input.Length; i++)
        {
            if (output.x >= _input[i].x && output.z >= _input[i].z)
            {
                output = _input[i];
            }
        }
        return output;
    }

    public static Vector3 GetBotRight(Vector3[] _input)
    {
        Vector3 output = new Vector3(float.MinValue, 0, float.MaxValue);
        for (int i = 0; i < _input.Length; i++)
        {
            if (output.x <= _input[i].x && output.z >= _input[i].z)
            {
                output = _input[i];
            }
        }
        return output;
    }

    public static Vector3 GetTopLeft(Vector3[] _input)
    {
        Vector3 output = new Vector3(float.MaxValue, 0, float.MinValue);
        for (int i = 0; i < _input.Length; i++)
        {
            if (output.x >= _input[i].x && output.z <= _input[i].z)
            {
                output = _input[i];
            }
        }
        return output;
    }

    public static Vector3 GetTopRight(Vector3[] _input)
    {
        Vector3 output = new Vector3(float.MinValue, 0, float.MinValue);
        for (int i = 0; i < _input.Length; i++)
        {
            if (output.x <= _input[i].x && output.z <= _input[i].z)
            {
                output = _input[i];
            }
        }
        return output;
    }
    #endregion

    /// <summary>
    /// Check if Point is inside a Range of Points
    /// </summary>
    /// <param name="_pos">The Point to Check</param>
    /// <param name="_distanceMin">Mininmal Distance</param>
    /// <param name="_usedPoints">Point so compare</param>
    /// <returns>Return true if the Point is outside the Range</returns>
    public static bool InsideRange(Vector3 _pos, float _distanceMin, List<Vector3> _usedPoints)
    {
        foreach (Vector3 pos in _usedPoints)
        {
            if (Vector3.Distance(_pos, pos) < _distanceMin)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Check if Point is inside a Range of Points
    /// </summary>
    /// <param name="_pos">The Point to Check</param>
    /// <param name="_distanceMin">Mininmal Distance</param>
    /// <param name="_distanceMax">Maximal Distance</param>
    /// <param name="_usedPoints">Point so compare</param>
    /// <returns>Return true if the Point is outside the Range</returns>
    public static bool InsideRange(Vector3 _pos, float _distanceMin, float _distanceMax, List<Vector3> _usedPoints)
    {
        foreach (Vector3 pos in _usedPoints)
        {
            float Distance = Vector3.Distance(_pos, pos);
            if (Distance < _distanceMin || Distance > _distanceMax)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Check if a Point is inside an Area
    /// </summary>
    /// <param name="_tmpPos">The Point to check</param>
    /// <param name="_size">The Size if the Area</param>
    /// <param name="_center">The Center of the Area</param>
    /// <param name="_showBunds">True to draw Gizmos</param>
    /// <returns>True if the Point is inside</returns>
    public static bool InsideArea(Vector3 _tmpPos, Vector3 _size, Vector3 _center, bool _showBunds = false)
    {
        Bounds b = new Bounds(_center, _size);
        if (_showBunds)
        {
            ShowBounds(b);
        }
        return b.Contains(_tmpPos);
    }

    public static bool InsideArea(Vector3 _tmpPos, Bounds _b, bool _showBunds = false) //Check if the point is in the city
    {
        if (_showBunds)
        {
            ShowBounds(_b);
        }
        return _b.Contains(_tmpPos);
    }

    public static void ShowBounds(Bounds _b)
    {
        Color color = Color.green;

        Vector3 v3FrontTopLeft;
        Vector3 v3FrontTopRight;
        Vector3 v3FrontBottomLeft;
        Vector3 v3FrontBottomRight;
        Vector3 v3BackTopLeft;
        Vector3 v3BackTopRight;
        Vector3 v3BackBottomLeft;
        Vector3 v3BackBottomRight;

        Vector3 v3Center = _b.center;
        Vector3 v3Extents = _b.extents;

        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, color, 20f);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, color, 20f);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, color, 20f);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, color, 20f);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, color, 20f);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, color, 20f);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, color, 20f);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, color, 20f);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, color, 20f);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, color, 20f);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, color, 20f);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, color, 20f);
    }

    /// <summary>
    /// Get the Edge between two Points
    /// </summary>
    /// <param name="_pos1">Start Point</param>
    /// <param name="_pos2">End Point</param>
    /// <returns></returns>
    public static Vector3 GetEdge(Vector3 _pos1, Vector3 _pos2)
    {
        return _pos2 - _pos1;
    }

    /// <summary>
    /// Find the Closest Point from a Point
    /// </summary>
    /// <param name="_point">Start Point</param>
    /// <param name="_input">Array of Points to find</param>
    /// <returns>The Closest Point</returns>
    public static Vector3 FindClosestPoint(Vector3 _point, Vector3[] _input)
    {
        float[] distance = new float[4];
        for (int i = 0; i < _input.Length; i++)
        {
            distance[i] = Vector3.SqrMagnitude(_input[i] - _point);
        }
        return _input[Array.IndexOf(distance, distance.Min())];
    }

    /// <summary>
    /// Get a Point on a Circel
    /// </summary>
    /// <param name="angleDegrees"></param>
    /// <param name="radius"></param>
    /// <returns>The Point on the Circel</returns>
    public static Vector3 GetUnitOnCircle(float angleDegrees, float radius)
    {
        // initialize calculation variables
        float _x = 0;
        float _z = 0;
        float angleInRadians = 0;
        Vector3 output;

        // convert degrees to radians
        angleInRadians = angleDegrees * Mathf.PI / 180.0f;

        // get the 2D dimensional coordinates
        _x = radius * Mathf.Cos(angleInRadians);
        _z = radius * Mathf.Sin(angleInRadians);

        // derive the 2D vector
        output = new Vector3(_x, 0, _z);

        // return the vector info
        return output;
    }

    /// <summary>
    /// Check if at the pos there is already a building
    /// </summary>
    /// <param name="_pos">The Point to Check</param>
    /// <param name="_CollToCheck">The Collider to Check</param>
    /// <returns>True if the Point is not inside the Collider</returns>
    public static bool CheckForValidPos(Vector3 _pos, params Collider[] _CollToCheck)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(_pos.x, 5000, _pos.z), -Vector3.up, out hit))
        {
            foreach (Collider coll in _CollToCheck)
            {
                if (hit.collider.GetType() == coll.GetType())
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
