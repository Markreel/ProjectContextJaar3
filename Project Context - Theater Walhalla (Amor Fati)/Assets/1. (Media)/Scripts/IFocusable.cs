using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFocusable
{
    void Focus(Vector2 _mousePos);
    void Unfocus(Vector2 _mousePos);
}
