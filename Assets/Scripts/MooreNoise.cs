using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


// Надо бы перевести все массивы на булеаны (тру, фолс), как это сделал Степан в 2 уроке для решета Эратосфена
// P.S.: Тогда не придётся думать о конвертировании интов в флоаты или апроксимации флоатов между собой. If(t)->v+=1;

public class MooreNoise : MonoBehaviour
{
    [SerializeField] int iteration;
    [SerializeField] int mapSize;
    [SerializeField] int mooreRadius;
    [SerializeField] int cellsAliveN;
    [SerializeField] int hMax;
    
    void Awake()
    {
        MooreNoiseGenerator(iteration, mapSize, mooreRadius,cellsAliveN, hMax);
    }
    public static int[,] MooreNoiseMap(int n, int r, int N)
    {
        int[,] v = new int[n+1,n+1];
        
        int d;
        for (int i = 0; i < n; i++)
        {
            for (int j = 1; j < n; j++)
            {
                if (Random.Range(0, 100) >= 50)
                    d = 1;
                else d = 0;

                v[i,j] = d;
            }
        }
        
        int b = 0;
        for (int i = r; i <= n-r; i++)
        {
            for (int j = r; j <= n-r; j++)
            {
                for (int x = i-r; x <= i+r; x++)
                {
                    for (int y = j-r; y <= j+r; y++)
                    {
                        if (v[x,y] == 1)
                            b++;
                    }
                }
                if (b >= N)
                    v[i,j] = 1;
                else 
                    v[i,j] = 0;
                b = 0;
            }
        }
        return v;
    }

    public static float[] MooreNoiseGenerator(int iterations, int gridSize, int radius, int cellsLiveCount, int hMax)
    {
        float[] map = new float[(gridSize+2) * (gridSize+2)];
        int v = 0;
        for (int k = 1; k <= iterations; k++)
        {
            var buffer = MooreNoiseMap(gridSize, radius, cellsLiveCount);
            
            for (int l = 0; l <= gridSize; l++)
                for (int m = 0; m <= gridSize; m++)
                    map[v++] += buffer[l, m];
            
            v = 0;
        }

        float h = 0;
        for (int l = 0; l <= (gridSize + 1) * (gridSize + 1); l++)
        {
            if (h < map[l])
                h = map[l];
            Mathf.Floor(map[l]);
        }

        h = (h/10000) * hMax;
        for (int k = 0; k <= (gridSize + 1) * (gridSize + 1); k++)
            map[k] /= h;
        
        return map;
    }
}