using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCursor : MonoBehaviour
{
    [SerializeField] Texture2D[] MouseTex;


    CursorMode _cursorMode = CursorMode.Auto;
    Vector2 _vector2= Vector2.zero;
    static InteractiveCursor Instance;
    private void Awake() {
        Instance = this;
        Cursor.SetCursor(Instance.MouseTex[0], Instance._vector2, Instance._cursorMode);
    }
    public static void ChangeCursor(int i){

        Cursor.SetCursor(Instance.MouseTex[i], Instance._vector2, Instance._cursorMode);
    }
}
