using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class LineRenderer : MonoBehaviour
{
    [SerializeField]
    Material _lineMaterial = null;

    [SerializeField]
    UnityEngine.U2D.PixelPerfectCamera _camera = null;

    struct Line
    {
        public Vector2 From{ get; set; }
        public Vector2 To{ get; set; }
        public Color Color{ get; set; }
    }
    List<Line> Lines = new List<Line>();

    float _width;
    float _height;

    void Awake()
    {
        Camera camera = GetComponent<Camera>();
        _width = camera.pixelWidth;
        _height = camera.pixelHeight;
    }

    void OnPostRender()
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, _width, 0, _height);
        
        foreach(Line line in Lines)
        {
            GL.Begin(GL.LINES);
            _lineMaterial.color = line.Color;
            _lineMaterial.SetPass(0);
            GL.Color(line.Color);
            GL.Vertex(ToPixelSpace(line.From));
            GL.Vertex(ToPixelSpace(line.To));
            GL.End();
        }

        GL.PopMatrix();

        Lines.Clear();
    }

    Vector2 ToPixelSpace(Vector2 p)
    {
        float worldToPixel = _camera.assetsPPU * 2.0f;

        return new Vector2(ToPixelSpace(p.x - _camera.transform.position.x, _width, worldToPixel), ToPixelSpace(p.y, _height, worldToPixel));
    }

    float ToPixelSpace(float v, float dim, float worldToPixel)
    {
        return ((((v * worldToPixel) / dim) + 1.0f) / 2.0f) * dim;
    }

    public void RenderLine(Vector2 from, Vector2 to, Color color)
    {
        Lines.Add(new Line()
        {
            From = from,
            To = to,
            Color = color
        });
    }
}
