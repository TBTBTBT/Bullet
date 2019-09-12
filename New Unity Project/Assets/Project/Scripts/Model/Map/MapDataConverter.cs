using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDataConverter : MonoBehaviour
{
    [SerializeField] private Tilemap _road;
    [SerializeField] private Tilemap _node;
    [SerializeField] private List<StationModel> _stationModels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
#if UNITY_EDITOR
    public Tilemap Road => _road;
    public Tilemap Node => _node;
    public void SetStationModel(List<StationModel> models)
    {
        _stationModels = models;
    }


    
#endif

}
#if UNITY_EDITOR
namespace UnityEditor.UI
{
    [CustomEditor(typeof(MapDataConverter), true)]
    public class MapDataConverterEditor : Editor
    {
        //private SerializedProperty _road;
        //private SerializedProperty _maxElementX;
        //private SerializedProperty _maxElementY;
        //private SerializedProperty _spacing;
        //private SerializedProperty _padding;
        //private SerializedProperty _paddingEnd;
        //private SerializedProperty _childAlignment;
        //private SerializedProperty _maxItemCount;

        //private SerializedProperty _loop;

        void OnEnable()
        {
            //_itemPrototype = serializedObject.FindProperty("_itemPrototype");
            //_maxElementX = serializedObject.FindProperty("_maxElementX");
            //_maxElementY = serializedObject.FindProperty("_maxElementY");
            //_spacing = serializedObject.FindProperty("_spacing");
            //_padding = serializedObject.FindProperty("_padding");
            //_paddingEnd = serializedObject.FindProperty("_paddingEnd");
            //_childAlignment = serializedObject.FindProperty("_childAlignment");
            //_maxItemCount = serializedObject.FindProperty("_maxItemCount");
            //_childAlignment = serializedObject.FindProperty("_childAlignment");

            //_maxItemCount = serializedObject.FindProperty("_maxItemCount");
            //_loop = serializedObject.FindProperty("_loop");

        }

        private float progress = 0;
        void Generate()
        {
            var cvt = target as MapDataConverter;
            var list = new List<StationModel>();
            var bounds = new BoundsInt(-100, -100,0,200, 200,0);
            progress = 0;
            Debug.Log($"{ bounds.xMin},{ bounds.xMax}");
            for (var x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (var y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var road = cvt.Road.GetTile<RoadTileBase>(new Vector3Int(x, y, 0));
                    if (road != null)
                    {
                        var node = cvt.Node.GetTile<MapTileBase>(new Vector3Int(x, y, 0));
                        if (node != null)
                        {
                            var station = new StationModel(list.Count);
                            station.Pos = new Vector2Int(x, y);
                            station.Type = node.MapType;
                            list.Add(station);
                        }
                    }

                    
                    
                }

                progress = (float)x / (bounds.xMax - bounds.xMin);
                EditorUtility.DisplayProgressBar("Generate", $"マップ走査({x,4},{0,4})", progress);
            }
            Debug.Log($"data:{list.Count}");
            var count = 0;
            progress = 0;
            foreach (var stationModel in list)
            {
                progress = (float)count / list.Count;
                var list2 = SearchStationLink(cvt, stationModel.Pos, stationModel.Pos,new BitArray(4,true));
                stationModel.Relation = list2.ConvertAll(_ => list.First(s => s.Pos == _).Id).ToArray();
                EditorUtility.DisplayProgressBar("Generate", $"ノード間経路構築({count}/{list.Count})", progress);
                count++;
            }
            cvt.SetStationModel(list);
            EditorUtility.ClearProgressBar();
        }

        List<Vector2Int> SearchStationLink(MapDataConverter cvt ,Vector2Int pos,Vector2Int me,BitArray dir)
        {
            var isRoad = cvt.Road.GetTile<RoadTileBase>(new Vector3Int(pos.x, pos.y, 0)) != null;
            var node = cvt.Node.GetTile<MapTileBase>(new Vector3Int(pos.x, pos.y, 0));
            var isNode = node != null;
            if (isNode && pos != me)
            {
                return new List<Vector2Int> (){pos};
            }

            bool CheckDir( int num)
            {
                return dir.Length > num && dir.Get(num);
            }
            if (isRoad)
            {
                var list = new List<Vector2Int>();
                if (CheckDir(0)) list.AddRange(SearchStationLink(cvt, pos + new Vector2Int(-1,0),me,new BitArray(new []{true,false,false,false})));
                if (CheckDir(1)) list.AddRange(SearchStationLink(cvt, pos + new Vector2Int(1, 0), me, new BitArray(new[] { false, true, false, false })));
                if (CheckDir(2)) list.AddRange(SearchStationLink(cvt, pos + new Vector2Int(0, -1), me, new BitArray(new[] { false, false, true, false })));
                if (CheckDir(3)) list.AddRange(SearchStationLink(cvt, pos + new Vector2Int(0, 1), me, new BitArray(new[] { false, false, false, true })));
                return list;
            }
            return new List<Vector2Int>();

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            if (GUILayout.Button("Generate Map Data"))
            {
                Generate();
            }
            //EditorGUILayout.PropertyField(_itemPrototype);
            //EditorGUILayout.PropertyField(_maxElementX);
            //EditorGUILayout.PropertyField(_maxElementY);
            //EditorGUILayout.PropertyField(_spacing);
            //EditorGUILayout.PropertyField(_padding);
            //EditorGUILayout.PropertyField(_paddingEnd);
            //EditorGUILayout.PropertyField(_childAlignment);
            //EditorGUILayout.PropertyField(_maxItemCount);

            //EditorGUILayout.PropertyField(_loop);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
