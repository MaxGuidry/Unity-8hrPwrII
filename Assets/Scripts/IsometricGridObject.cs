using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu(menuName = "ScriptableObjects/IsometricGridObject")]
public class IsometricGridObject : ScriptableObject
{
    [SerializeField] public List<GameObject> CellGameObjects;

    [SerializeField] public List<IsometricCell> Cells;


    [Range(2, 10)] public int Height;

    [Range(2, 10)] public int Width;

    public void GenerateGrid()
    {
        Cells = new List<IsometricCell>();
        for (var j = 0; j < Height; j++)
        for (var i = 0; i < Width; i++)
            Cells.Add(new IsometricCell(i, new Vector2(i, j), this));
    }

    public void PlaceCells()
    {
        Cells.ForEach(c =>
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            CellGameObjects.Add(go);
            go.transform.position = c.WorldPos;
        });
    }

    public void RemoveCells()
    {
        CellGameObjects.ForEach(c => DestroyImmediate(c));
        CellGameObjects = new List<GameObject>();
        Cells = new List<IsometricCell>();
    }
}
#if UNITY_EDITOR


[CustomEditor(typeof(IsometricGridObject))]
public class CustomIsometricGridObjectEditor : Editor
{
    private IsometricGridObject _target => target as IsometricGridObject;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Grid"))  
            _target.GenerateGrid();

        if (GUILayout.Button("Place Cells"))
            _target.PlaceCells();

        if (GUILayout.Button("Remove Cells"))
            _target.RemoveCells();
    }

    public static void Label(string value)
    {
        EditorGUILayout.LabelField(value);
    }
}
#endif

[Serializable]
public class IsometricCell
{
    private const float tilesizeX = 2;
    private const float tilesizeY = 1;
    private readonly float dk = tilesizeY;
    private readonly float dx = tilesizeX / 2;
    private readonly float dy = tilesizeY / 2;
    public object Grid;
    public int Id;
    public Vector2 Position;

    public IsometricCell(int id, Vector2 pos, object grid)
    {
        Id = id;
        Position = pos;
        Grid = grid;
    }

    public float i => Position.x;
    public float j => Position.y;
    public float k => 0;

    public float Xpos => i * dx - j * dx;
    public float Ypos => i * dy + j * dy + k * dk;

    public Vector2 WorldPos => new Vector2(Xpos, Ypos);
}