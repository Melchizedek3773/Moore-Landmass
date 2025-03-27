using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))] 
public class MooreTerrainGenerator : MonoBehaviour
{
    [SerializeField] float cellSize;
    [SerializeField] int gridSize;
    
    [SerializeField] int iteration;
    [SerializeField] int mooreRadius;
    [SerializeField] int cellsAliveN;
    [SerializeField] int hMax;
    
    private Vector3[] _vertices;
    private int[] _triangles;
        
    Mesh _mesh;
    MeshCollider _meshCollider;
    void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _mesh.name = "Terrain";
        _meshCollider = GetComponent<MeshCollider>();
            
        ContiguousProceduralGrid();
        CreateMesh();
            
        _meshCollider.sharedMesh = _mesh;
            
        Vector2[] uvs = new Vector2[_vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(_vertices[i].x, _vertices[i].z);
        }
        _mesh.uv = uvs;
    }

    private void ContiguousProceduralGrid()
    {
        _vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        _triangles = new int[gridSize * gridSize * 6];
        
        float vertexOffset = cellSize * 0.5f;
        int v = 0;
        int t = 0;

        var mooreNoiseMap = MooreNoise.MooreNoiseGenerator(iteration, gridSize, mooreRadius, cellsAliveN, hMax);
        // Вершины
        for (int x = 0; x <= gridSize; x++)
        {
            for (int y = 0; y <= gridSize; y++)
            {
                _vertices[v] = new Vector3(x*cellSize - vertexOffset, mooreNoiseMap[v], y*cellSize - vertexOffset);
                v++;
            }
        }
            
        // Треугольники
        v = 0;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                // Четный квадрат
                if ((x + y) % 2 == 0)
                {
                    _triangles[t] = v;
                    _triangles[t + 1] = v + 1;
                    _triangles[t + 2] = v + gridSize + 1;
                    _triangles[t + 3] = v + gridSize + 1;
                    _triangles[t + 4] = v + 1;
                    _triangles[t + 5] = v + gridSize + 2;
                }
                // Нечётный квадрат
                else
                {
                    _triangles[t] = v + gridSize + 1;
                    _triangles[t + 1] = v;
                    _triangles[t + 2] = v + gridSize + 2;
                    _triangles[t + 3] = v + gridSize + 2;
                    _triangles[t + 4] = v;
                    _triangles[t + 5] = v + 1;
                }

                v++;
                t += 6;
            }
            v++;
        }
    }
    private void CreateMesh()
    {
        _mesh.Clear();
        
        _mesh.SetVertices(_vertices);
        _mesh.SetTriangles(_triangles, 0);
        _mesh.RecalculateNormals();
            
        _meshCollider.sharedMesh = _mesh;
    }
    
}