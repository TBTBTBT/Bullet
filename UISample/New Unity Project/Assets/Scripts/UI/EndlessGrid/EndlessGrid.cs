using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public interface IAsyncUI
{

}

public class EndlessGrid : UIBehaviour
{
    public enum Direction
    {
        Vertical,
        Horizontal,
s    }
    public class ElementEvent : UnityEvent<GameObject,int>
    {
    }

    public Direction ScrollDirection;
    public ElementEvent OnUpdateElement = new ElementEvent();
    [SerializeField] private RectTransform _contents;
    [SerializeField] private ScrollRect _scrollRect;
    public RectOffset Padding;
    public Vector2 CellSize;
    public Vector2 Spacing;
    
    public int AllItemCount = 0;
    public int ViewItemCount = 0;

    public GameObject ItemPrefab;
    //private RectOffset _invisiblePadding;// = new RectOffset(-100,100,-100,100);
    private List<(int index,RectTransform obj)> _items = new List<(int index,RectTransform obj)>();


    protected override void Awake()
    {
        //_invisiblePadding = new RectOffset(-100, 100, -100, 100);
        base.Awake();
        if (_contents.anchorMin != new Vector2(0, 1) ||
            _contents.anchorMax != new Vector2(1,1))
        {
            Debug.LogError("Invalid Anchor Setting");
        }
        _scrollRect.onValueChanged.AddListener(OnChangeValue);
        SetItemRectVertical();
        InstantiateItemVertical();
    }

    int StartIndexVertical(float pos)
    {
        var singleHeight = CellSize.y + Spacing.y;
        var topPos = pos - singleHeight * ViewItemCount / 2;
        var index = (int) Mathf.Floor(topPos / singleHeight);// * col;
        return index;


    }
    int ItemIndexToListIndexVertical(int index)
    {
        var start = StartIndexVertical(0);
        var start = StartIndexVertical(_contents.anchoredPosition.y);
        
    }
    private void SetItemRectVertical()
    {
        var height = (CellSize.y +
                      Spacing.y) *
                     AllItemCount +
                     Padding.top +
                     Padding.bottom;
        _contents.sizeDelta =
            new Vector2(_contents.sizeDelta.x, height);
    }

    
    private float CalcVerticalPosition(int count)
    {
        var top = -(_contents.sizeDelta.y / 2)
        return -(- (_contents.sizeDelta.y / 2)
               +(CellSize.y +
                Spacing.y) *
               count +
               Padding.top);
    }

    private void InstantiateItemVertical()
    {
        _items.Clear();
        GameObject added = null;
        var count = 0;
        if (AllItemCount <= 0)
        {
            return; 

        }
        do
        {
            added = Instantiate(ItemPrefab, _contents.transform);
            added.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(added.GetComponent<RectTransform>().anchoredPosition.x,CalcVerticalPosition(count));
            //OnUpdateElement.Invoke(count);
            _items.Add(added.GetComponent<RectTransform>());
          
            count++;
        } while (count < AllItemCount && count < ViewItemCount);
    }

    private void UpdateItemVertical()
    {

    }
    void OnChangeValue(Vector2 pos)
    {
        //Debug.Log(pos);
        //Debug.Log(_scrollRect.content.anchoredPosition);
        OnMoveHorizontal();
        OnMoveVertical();
    }

    private bool CheckVisibleVertical(Vector2 anchoredPosition)
    {
        var min = _contents.anchoredPosition.y - (_contents.sizeDelta.y / 2);
        var max = _contents.anchoredPosition.y + (_contents.sizeDelta.y / 2);
        min -= CellSize.y;
        max += CellSize.y;
        return anchoredPosition.y > min && anchoredPosition.y < max;
    }
    private float GetNewPositionVertical(float anchoredPosition)
    {
        var min = _contents.anchoredPosition.y - (_contents.sizeDelta.y / 2);
        var max = _contents.anchoredPosition.y + (_contents.sizeDelta.y / 2);
        min -= CellSize.y;
        max += CellSize.y;
        switch (anchoredPosition)
        {
            case float n when n < min:
                return max;
            case float n when n > max:
                return min;
        }
        return anchoredPosition;
    }


    

    
    void OnMoveVertical()
    {
        if (!_scrollRect.vertical)
        {
            return;
        }

     
        Debug.Log(StartIndexVertical(1));


        for (var i = 0; i < _items.Count; i++)
        {
            _items[i].gameObject.SetActive(
            CheckVisibleVertical(_items[i].anchoredPosition));
            //_items[i].anchoredPosition = new Vector2(_items[i].anchoredPosition.x, GetNewPositionVertical(_items[i].anchoredPosition.y));
        }
    }

    void OnMoveHorizontal()
    {
        if (!_scrollRect.horizontal)
        {
            return;
        }
    }
}