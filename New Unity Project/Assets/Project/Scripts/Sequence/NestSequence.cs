using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Toast;
public interface INestSequence
{
    IEnumerator Update();
    string CurrentState { get; }
}
public abstract class NestSequence<T> : INestSequence,IEnumerator where T : struct 
{
    protected Statemachine<T> _statemachine = new Statemachine<T>();
    public string CurrentState { get => _statemachine.Current.ToString(); }
    public object Current => null;
    private bool _isInitSequence = false;
    protected abstract T EndState();
    protected abstract T StartState();
    protected abstract void InitStatemachine();

    public IEnumerator Update()
    {
        while (MoveNext())
        {
            yield return null;
        }
    }

    public bool MoveNext()
    {
        if (!_isInitSequence)
        {
            SequenceTracer.Instance.Add(this);
            _isInitSequence = true;
            InitStatemachine();
            _statemachine.Next(StartState());
        }
        _statemachine.Update();
        var result = !EqualityComparer<T>.Default.Equals(_statemachine.Current, EndState());
        if (!result)
        {
            SequenceTracer.Instance.Remove(this);
        }
        return result;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}
/// <summary>
/// 常に今実行中のシーケンスを追いかける
/// </summary>
public class SequenceTracer : Singleton<SequenceTracer>
{
    List<INestSequence> _sequence = new List<INestSequence>();
    public List<INestSequence> Sequences => _sequence;
    public void Add(INestSequence seq)
    {
        _sequence.Add(seq);
    }
    public void Remove(INestSequence seq)
    {
        _sequence.Remove(seq);
    }
}
#if UNITY_EDITOR
/*
public class SequenceTracerWindow : EditorWindow
{
    [MenuItem("Toast/SequenceTracer")]
    static void Open()
    {
        var window = CreateInstance<SequenceTracerWindow>();
        window.Show();
    }
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        foreach(var s in SequenceTracer.Instance.Sequences)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{s.GetType().ToString()} : {s.CurrentState}");
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
}
*/
#endif