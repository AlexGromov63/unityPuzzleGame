using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Element : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int X, Y;   
    public ElementType type;

    private GameController _gc;

    public void initElement(int x, int y, ElementType t, GameController gc) 
    {
        X = x;
        Y = y;
        type = t;
        _gc = gc;

        GetComponent<Image>().color = _gc.GetColorByType(type);
    }

    Camera cm = null;
    private void Awake()
    {
        cm = FindObjectOfType<Camera>();
    }

    bool isDrag = false;
    bool isVertical = false;
    bool isHorizontal = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (type == ElementType.BLOCK) 
            return;

        isDrag = true;
        isVertical = false;
        isHorizontal = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            var pos = cm.ScreenToWorldPoint(eventData.position);
            var lpos = transform.InverseTransformPoint(pos);
            if(isVertical == false && isHorizontal == false)
            {    
                if (lpos.magnitude > 0.1f)
                {
                    if (Mathf.Abs(lpos.x) > Mathf.Abs(lpos.y))
                    {
                        isHorizontal = true;
                    }
                    else
                    {
                        isVertical = true;
                    }
                }
            }
            if (isVertical)
            {
                if (transform.localPosition.y > 5 && _gc.CanMoveToXY(X, Y - 1) == false)
                    return;
                if (transform.localPosition.y < -5 && _gc.CanMoveToXY(X, Y + 1) == false)
                    return;
                transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);

                if (transform.localPosition.y > 100)
                {
                    Y--;
                    _gc.SetParentKletka(transform, X, Y);
                }
                if (transform.localPosition.y < -100)
                {
                    Y++;
                    _gc.SetParentKletka(transform, X, Y);
                }
            }
            else if (isHorizontal)
            {
                if (transform.localPosition.x > 5 && _gc.CanMoveToXY(X + 1, Y) == false)
                    return;
                if (transform.localPosition.x < -5 && _gc.CanMoveToXY(X - 1, Y) == false)
                    return;
                transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
                if (transform.localPosition.x > 100)
                {
                    X++;
                    _gc.SetParentKletka(transform, X, Y);
                }
                if (transform.localPosition.x < -100)
                {
                    X--;
                    _gc.SetParentKletka(transform, X, Y);
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        transform.localPosition = Vector3.zero;

        _gc.CheckWin();
    }
}
public enum ElementType
{
    BLOCK,
    YELLOW,
    ORANGE,
    RED
}