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
    RawImage _image = null;

    [SerializeField]
    Vector2 _scale = new Vector2(6, 4);

    struct Line
    {
        public Vector2 From{ get; set; }
        public Vector2 To{ get; set; }
        public Color Color{ get; set; }
    }
    List<Line> Lines = new List<Line>();

    void OnPostRender()
    {
        GL.PushMatrix();
        _lineMaterial.SetPass(0);
        
        GL.LoadPixelMatrix(0, 400, 0, 225);

        foreach(Line line in Lines)
        {
            GL.Begin(GL.LINES);
            GL.Color(line.Color);
            GL.Vertex(ToScreenSpace(line.From));
            GL.Vertex(ToScreenSpace(line.To));
            GL.End();
        }

        GL.PopMatrix();

        Lines.Clear();
    }

    Vector2 ToScreenSpace(Vector2 p)
    {

        return new Vector2((((p.x / _scale.x) + 1.0f) / 2.0f) * 400, (((p.y / _scale.y) + 1.0f) / 2.0f) * 225);
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
