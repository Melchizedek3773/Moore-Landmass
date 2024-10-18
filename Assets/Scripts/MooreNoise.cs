using UnityEngine;
using Random = UnityEngine.Random;

public class MooreNoise : MonoBehaviour
{
    void Awake()
    {
        MooreNoiseMap(24, 2, 13);
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
                
                Debug.Log(v[i,j]);
            }
        }
        return v;
    }
}