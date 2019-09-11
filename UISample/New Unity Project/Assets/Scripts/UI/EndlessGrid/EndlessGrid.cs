using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector4 = System.Numerics.Vector4;

public class EndlessGrid : UIBehaviour
{
    public enum Direction
    {
        Vertical,
        Horizontal,
    }
    public class ElementEvent : UnityEvent<GameObject,int>
    {
    }

    public class ElementData
    {
        public int Index;
        public RectTransform GameObject;
    }
    public Direction ScrollDirection;
    public ElementEvent OnUpdateElement = new ElementEvent();
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private int _allItemCount = 0;
    [SerializeField] private int _viewItemCount = 0;
    public RectOffset Padding;
    public Vector2 CellSize;
    public Vector2 Spacing;
    public int Column = 1;
    public GameObject ItemPrefab;

    public int AllItemCount
    {
        get => _allItemCount;
        set
        {
            _allItemCount = value;
            Recalc();
        }
    }

    public int ViewItemCount
    {
        get => _viewItemCount;
        set
        {
            _viewItemCount = value;
            Recalc();
        }
    }
    
    
    
    private List<ElementData> _items = new List<ElementData>();
    public bool InitOnStart = true;
    /// <summary>
    /// スクロール方向ごとのアンカー設定
    /// この設定になってなければ無理やり変える
    /// </summary>
    private static Dictionary<Direction, Vector4> _anchorSettings = new Dictionary<Direction, Vector4>()
    {
        {Direction.Vertical, new Vector4(0, 1, 1, 1)},
        {Direction.Horizontal, new Vector4(0, 0, 0, 1)}
    };

    protected override void Awake()
    {
        //OnUpdateElement.AddListener((g, i) => { g.GetComponentInChildren<TMPro.TMP_Text>().text = i.ToString(); });
        //_invisiblePadding = new RectOffset(-100, 100, -100, 100);
        base.Awake();
     
    }

    protected override void Start()
    {
        if (InitOnStart)
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        _items.RemoveAll(i =>
        {
            var go = i.GameObject.gameObject;
            Destroy(go);
            return true;
        });
        CheckAnchor();
        switch (ScrollDirection)
        {
            case Direction.Vertical:
                SetItemRectVertical();
                InstantiateItemVertical();
                _scrollRect.onValueChanged.AddListener(p=>OnMoveVertical());
                break;
            case Direction.Horizontal:
                SetItemRectHorizontal();
                InstantiateItemHorizontal();
                _scrollRect.onValueChanged.AddListener(p => OnMoveHorizontal());
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
       
    }
    /// <summary>
    /// 最大数変更時など再計算
    /// </summary>
    void Recalc()
    {
        switch (ScrollDirection)
        {
            case Direction.Vertical:
                SetItemRectVertical();
                OnMoveVertical(true);
                break;
            case Direction.Horizontal:
                SetItemRectHorizontal();
                OnMoveHorizontal(true);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        _scrollRect.onValueChanged.Invoke(_scrollRect.normalizedPosition);

    }
    #region Public

    public void ForceUpdate()
    {

    }

    public void Focus(int index)
    {

    }


    #endregion

    void CheckAnchor()
    {
        if (!_anchorSettings.ContainsKey(ScrollDirection))
        {
            return;
        }
       
        if (_scrollRect.content.anchorMin != new Vector2(_anchorSettings[ScrollDirection].X, _anchorSettings[ScrollDirection].Y) ||
            _scrollRect.content.anchorMax != new Vector2(_anchorSettings[ScrollDirection].Z, _anchorSettings[ScrollDirection].W))
        {

            Debug.LogWarning("Invalid Anchor Setting");
            _scrollRect.content.anchorMin =
                new Vector2(_anchorSettings[ScrollDirection].X, _anchorSettings[ScrollDirection].Y);
            _scrollRect.content.anchorMax =
                new Vector2(_anchorSettings[ScrollDirection].Z, _anchorSettings[ScrollDirection].W);

        }
    }

    bool CheckIndex(int index)
    {
        return index >= 0 && index < AllItemCount;
    }
    #region Vertical


    int ItemIndexToListIndexVertical(int index)
    {
        var singleHeight = CellSize.y + Spacing.y;
        var viewLength = ViewItemCount * singleHeight;
        var currentPos = _scrollRect.content.anchoredPosition.y + viewLength * 3 / 4 - singleHeight * index + Padding.top;
        var loop = (int)Mathf.Floor(currentPos / viewLength);
        var maxLoop = (int)Mathf.Ceil((float)AllItemCount / ViewItemCount) - 1;
        maxLoop = maxLoop < 0 ? 0 : maxLoop;
        loop = loop > maxLoop ? maxLoop : loop;
        loop = loop < 0 ? 0 : loop;
        return loop * ViewItemCount + index;

    }
    private void SetItemRectVertical()
    {
        var height = (CellSize.y +
                      Spacing.y) *
                     AllItemCount +
                     Padding.top +
                     Padding.bottom;
        _scrollRect.content.sizeDelta =
            new Vector2(_scrollRect.content.sizeDelta.x, height);
    }


    private float CalcPositionVertical(int listIndex)
    {
        return -(-(_scrollRect.content.sizeDelta.y / 2)
                 + (CellSize.y +
                    Spacing.y) *
                 listIndex +
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
            added = Instantiate(ItemPrefab, _scrollRect.content.transform);
            added.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(added.GetComponent<RectTransform>().anchoredPosition.x, CalcPositionVertical(count));
            _items.Add(new ElementData() { GameObject = added.GetComponent<RectTransform>(), Index = count });
            OnUpdateElement.Invoke(added,count);
            count++;
        } while (count < AllItemCount && count < ViewItemCount);
    }
    void OnMoveVertical(bool force = false)
    {

        for (var i = 0; i < _items.Count; i++)
        {
            var index = ItemIndexToListIndexVertical(i);
            var isRange = CheckIndex(index);
            if (force || isRange && _items[i].Index != index)
            {
                OnUpdateElement?.Invoke(_items[i].GameObject.gameObject, index);
                _items[i].Index = index;
                _items[i].GameObject.anchoredPosition = new Vector2(_items[i].GameObject.anchoredPosition.x, CalcPositionVertical(index));
            }
        }

    }

    #endregion
    #region Horizontal
    int ItemIndexToListIndexHorizontal(int index)
    {
        var singleSize = CellSize.x + Spacing.x;
        var viewLength = ViewItemCount * singleSize;
        var currentPos = -_scrollRect.content.anchoredPosition.x + viewLength * 3 / 4 - singleSize * index + Padding.left;
        var loop = (int)Mathf.Floor(currentPos / viewLength);
        var maxLoop = (int)Mathf.Ceil((float)AllItemCount / ViewItemCount) - 1;
        maxLoop = maxLoop < 0 ? 0 : maxLoop;
        loop = loop > maxLoop ? maxLoop : loop;
        loop = loop < 0 ? 0 : loop;
        return loop * ViewItemCount + index;

    }
    private void SetItemRectHorizontal()
    {
        var width = (CellSize.x +
                      Spacing.x) *
                     AllItemCount +
                     Padding.left +
                     Padding.right;
        _scrollRect.content.sizeDelta =
            new Vector2(width, _scrollRect.content.sizeDelta.y);
    }


    private float CalcPositionHorizontal(int listIndex)
    {
        return -(_scrollRect.content.sizeDelta.x / 2)
                 + (CellSize.x +
                    Spacing.x) *
                 listIndex +
                 Padding.left;
    }

    private void InstantiateItemHorizontal()
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
            added = Instantiate(ItemPrefab, _scrollRect.content.transform);
            added.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(CalcPositionHorizontal(count),added.GetComponent<RectTransform>().anchoredPosition.y);
            _items.Add(new ElementData() { GameObject = added.GetComponent<RectTransform>(), Index = count });
            OnUpdateElement.Invoke(added, count);
            count++;
        } while (count < AllItemCount && count < ViewItemCount);
    }
    void OnMoveHorizontal(bool force = false)
    {



        for (var i = 0; i < _items.Count; i++)
        {
            var index = ItemIndexToListIndexHorizontal(i);
            var isRange = CheckIndex(index);

            if (force || isRange && _items[i].Index != index)
            {
                OnUpdateElement?.Invoke(_items[i].GameObject.gameObject, index);
                _items[i].Index = index;
                _items[i].GameObject.anchoredPosition = new Vector2(CalcPositionHorizontal(index),_items[i].GameObject.anchoredPosition.y);
            }
        }

    }

    #endregion


}