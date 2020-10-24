using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public MeshFilter Mesh_Filter;

    public float FovDegrees = 90f;
    public int ViewRayCount = 3;
    public float ViewDistance = 50f;

    public LayerMask ViewLayerMask;

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    void Start()
    {
        var mesh = new Mesh();
        Mesh_Filter.mesh = mesh;

        vertices = new Vector3[ViewRayCount + 1];

        for(var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vector3.zero;
        }

        uv = new Vector2[vertices.Length];
        triangles = new int[(ViewRayCount - 1) * 3];
    }

    void Update()
    {
        _calculateMesh();
        _assignMesh();
    }

    private void _calculateMesh()
    {
        vertices[0] = Vector3.zero;

        var currentAngle = -FovDegrees / 2f;
        float angleIncrease = FovDegrees / (float)(ViewRayCount - 1f);
        for (var i = 0; i < ViewRayCount; i++)
        {
            var vertex = _vectorFromAngle(currentAngle) * ViewDistance;

            if (Physics.Raycast(transform.position, transform.TransformDirection(vertex.normalized), out var hit, ViewDistance, ViewLayerMask))
            {
                vertex = transform.worldToLocalMatrix * (hit.point - transform.position);
                if(vertex.magnitude > ViewDistance)
                {
                    vertex = vertex.normalized * ViewDistance;
                }
            }

            vertices[i + 1] = vertices[i + 1] * 0.9f + vertex * 0.1f;
            if (i != (ViewRayCount - 1))
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            currentAngle += angleIncrease;
        }
    }

    private void _assignMesh()
    {
        Mesh_Filter.mesh.vertices = vertices;
        Mesh_Filter.mesh.uv = uv;
        Mesh_Filter.mesh.triangles = triangles;
        Mesh_Filter.mesh.bounds = new Bounds(transform.position, Vector3.one * 1000f);
    }

    private Vector3 _vectorFromAngle(float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
    }
}
