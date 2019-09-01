using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Toast;
public class MapUIManager : SingletonMonoBehaviour<MapUIManager>,IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    private Vector2 _posAnchorBuffer;
    private Vector2 _posBuffer;
    private Vector2 _acc;
    private MapView _mapView;

    public void Set(MapView view)
    {
        _mapView = view;
    }
    //private Vector2 _posBuffer;
    //private new void Awake()
    //{

        //GetComponent<Canvas>().sortingOrder = 1;
    //}
    public Vector2 TileToWorldPos(Vector2Int pos)
    {
        return _mapView.TileToWorldPos(pos);
    }
    public Vector2Int WorldToTilePos(Vector2 pos)
    {
        return _mapView.WorldToTilePos(pos);
    }
    public void OnDrag(PointerEventData eventData)
    {
        _posBuffer = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _posAnchorBuffer = eventData.position;
        _posBuffer = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _posBuffer = Vector2.zero;
        _posAnchorBuffer = Vector2.zero;
    }

    void Update()
    {
        _acc = Vector2.Lerp(_acc, _posBuffer - _posAnchorBuffer, 0.5f);
        CameraManager.Instance.GameCamera.transform.Translate(_acc / 500);

    }
}
