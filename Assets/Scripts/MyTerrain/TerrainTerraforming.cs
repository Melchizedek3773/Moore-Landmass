using System;
using UnityEngine;

namespace MyTerrain
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] 
    public class TerrainTerraforming : MonoBehaviour
    {
        [SerializeField] Camera cam; 
        [SerializeField] float terraformingEffeciency = 0.01f;
        [SerializeField] float terraformingRadius = 0.5f;
        
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        
        void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
        }
        void Update()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if ( Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitInfo) )
            {
                TerraformTerrain(hitInfo.point, terraformingEffeciency, terraformingRadius);
            }
            if ( Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out hitInfo) )
            {
                TerraformTerrainDown(hitInfo.point, -terraformingEffeciency, terraformingRadius);
            }
        }

        // Терраформинг
        private Mesh _mesh;
        private Vector3[] _vertices;
        
        private void TerraformTerrain(Vector3 pos, float height, float range)
        {
            _mesh = _meshFilter.sharedMesh;
            _vertices = _mesh.vertices;
            pos -= _meshFilter.transform.position;

            int a = 0;
            foreach (Vector3 vert in _vertices)
            {
                if (Vector2.Distance( new Vector2(vert.x, vert.z), new Vector2(pos.x, pos.z)) <= range )
                {
                    if (vert.y > 2)
                        return;
                    
                    // Center
                    if (a % 2 != 0)
                        _vertices[a] += new Vector3(0, height, 0);
                    
                    // Up, down
                    if (_vertices[a+1].y+1 < _vertices[a].y)
                        _vertices[a+1] += new Vector3(0, height, 0);
                    if (_vertices[a-1].y+1 < _vertices[a].y)
                        _vertices[a-1] += new Vector3(0, height, 0);
                    
                    // Right, left
                    if (_vertices[a+25].y+1 < _vertices[a].y)
                        _vertices[a+25] += new Vector3(0, height, 0);
                    if (_vertices[a-25].y+1 < _vertices[a].y)
                        _vertices[a-25] += new Vector3(0, height, 0);
                    
                    //Right - up, down
                    if (_vertices[a+26].y+1 < _vertices[a].y)
                        _vertices[a+26] += new Vector3(0, height, 0);
                    if (_vertices[a+24].y+1 < _vertices[a].y)
                        _vertices[a+24] += new Vector3(0, height, 0);
                    
                    //Left - up, down
                    if (_vertices[a-24].y+1 < _vertices[a].y)
                        _vertices[a-24] += new Vector3(0, height, 0);
                    if (_vertices[a-26].y+1 < _vertices[a].y)
                        _vertices[a-26] += new Vector3(0, height, 0);
                }
                a++;
            }
            _mesh.SetVertices(_vertices);
            _mesh.RecalculateNormals();
            _meshCollider.sharedMesh = _mesh;
        }
        private void TerraformTerrainDown(Vector3 pos, float height, float range)
        {
            _mesh = _meshFilter.sharedMesh;
            _vertices = _mesh.vertices;
            pos -= _meshFilter.transform.position;

            int a = 0;
            foreach (Vector3 vert in _vertices)
            {
                if (Vector2.Distance( new Vector2(vert.x, vert.z), new Vector2(pos.x, pos.z)) <= range )
                {
                    if (vert.y < -2)
                        return;
                    
                    // Center
                    if (a % 2 != 0)
                        _vertices[a] += new Vector3(0, height, 0);
                    
                    // Up, down
                    if (_vertices[a+1].y-1 > _vertices[a].y)
                        _vertices[a+1] += new Vector3(0, height, 0);
                    if (_vertices[a-1].y-1 > _vertices[a].y)
                        _vertices[a-1] += new Vector3(0, height, 0);
                    
                    // Right, left
                    if (_vertices[a+25].y-1 > _vertices[a].y)
                        _vertices[a+25] += new Vector3(0, height, 0);
                    if (_vertices[a-25].y-1 > _vertices[a].y)
                        _vertices[a-25] += new Vector3(0, height, 0);
                    
                    //Right - up, down
                    if (_vertices[a+26].y-1 > _vertices[a].y)
                        _vertices[a+26] += new Vector3(0, height, 0);
                    if (_vertices[a+24].y-1 > _vertices[a].y)
                        _vertices[a+24] += new Vector3(0, height, 0);
                    
                    //Left - up, down
                    if (_vertices[a-24].y-1 > _vertices[a].y)
                        _vertices[a-24] += new Vector3(0, height, 0);
                    if (_vertices[a-26].y-1 > _vertices[a].y)
                        _vertices[a-26] += new Vector3(0, height, 0);
                }
                a++;
            }
            _mesh.SetVertices(_vertices);
            _mesh.RecalculateNormals();
            _meshCollider.sharedMesh = _mesh;
        }
    }
}
