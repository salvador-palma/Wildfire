using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    public BoxCollider2D area;
    public bool Block(Vector2 dest)
    {
        // Get the world space center and size of the box
        Vector2 center = (Vector2)area.transform.position + area.offset;
        Vector2 size = area.size;
        Vector2 halfSize = size * 0.5f;

        // Create the bounds manually
        Rect bounds = new Rect(center - halfSize, size);

        return bounds.Contains(dest);
        return area.OverlapPoint(dest);
    }
}
