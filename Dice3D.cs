using System.Numerics;

namespace Kaskrout;

/// <summary>A small perspective-rendered, six-sided die for the MAUI GraphicsView.</summary>
internal sealed class Dice3DDrawable : IDrawable
{
    private readonly Face[] faces =
    [
        new(1, new(0, 0, 1), new(1, 0, 0), new(0, 1, 0), Color.FromArgb("#E52228")),
        new(6, new(0, 0, -1), new(-1, 0, 0), new(0, 1, 0), Color.FromArgb("#A90F19")),
        new(5, new(1, 0, 0), new(0, 0, -1), new(0, 1, 0), Color.FromArgb("#BD111C")),
        new(2, new(-1, 0, 0), new(0, 0, 1), new(0, 1, 0), Color.FromArgb("#CF171F")),
        new(3, new(0, 1, 0), new(1, 0, 0), new(0, 0, -1), Color.FromArgb("#F12B30")),
        new(4, new(0, -1, 0), new(1, 0, 0), new(0, 0, 1), Color.FromArgb("#B7121B"))
    ];

    public float Pitch { get; set; } = -0.34f;
    public float Yaw { get; set; } = 0.48f;
    public float Roll { get; set; } = -0.12f;

    public void SetFaceToFront(int value)
    {
        // Standard die opposites: 1/6, 2/5, 3/4.
        (Pitch, Yaw, Roll) = value switch
        {
            1 => (0f, 0f, 0f),
            6 => (0f, MathF.PI, 0f),
            5 => (0f, -MathF.PI / 2, 0f),
            2 => (0f, MathF.PI / 2, 0f),
            3 => (MathF.PI / 2, 0f, 0f),
            4 => (-MathF.PI / 2, 0f, 0f),
            _ => (0f, 0f, 0f)
        };
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.FillColor = Colors.Transparent;
        var centerX = dirtyRect.Center.X;
        var centerY = dirtyRect.Center.Y + 4;
        var rotation = Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
        var rendered = faces
            .Select(face => new RenderedFace(face, Transform(face.Normal, rotation).Z))
            .Where(face => face.Depth > 0.001f)
            .OrderBy(face => face.Depth)
            .ToArray();

        foreach (var renderedFace in rendered)
        {
            var face = renderedFace.Face;
            var normal = Transform(face.Normal, rotation);
            var center = normal;
            var u = Transform(face.U, rotation);
            var v = Transform(face.V, rotation);
            var corners = new[]
            {
                center - u - v, center + u - v,
                center + u + v, center - u + v
            };
            var polygon = new PathF();
            var first = Project(corners[0], centerX, centerY);
            polygon.MoveTo(first.X, first.Y);
            for (var i = 1; i < corners.Length; i++)
            {
                var point = Project(corners[i], centerX, centerY);
                polygon.LineTo(point.X, point.Y);
            }
            polygon.Close();

            canvas.FillColor = face.Color;
            canvas.FillPath(polygon);
            canvas.StrokeColor = Color.FromArgb("#8E1118");
            canvas.StrokeSize = 2.5f;
            canvas.DrawPath(polygon);

            foreach (var pip in Pips(face.Value))
            {
                var point3D = center + (u * pip.X) + (v * pip.Y) + (normal * 0.025f);
                var point2D = Project(point3D, centerX, centerY);
                var radius = ProjectedRadius(point3D, centerX, centerY, 0.115f);
                canvas.FillColor = Color.FromArgb("#8E1118");
                canvas.FillEllipse(point2D.X - radius * 0.92f + 1.2f, point2D.Y - radius * 0.92f + 2f, radius * 1.84f, radius * 1.84f);
                canvas.FillColor = Color.FromArgb("#FFFDF8");
                canvas.FillEllipse(point2D.X - radius, point2D.Y - radius, radius * 2, radius * 2);
                canvas.FillColor = Color.FromArgb("#FFFFFF");
                canvas.FillEllipse(point2D.X - radius * 0.56f, point2D.Y - radius * 0.64f, radius * 0.68f, radius * 0.58f);
            }
        }

        canvas.RestoreState();
    }

    private static Vector3 Transform(Vector3 value, Quaternion rotation) => Vector3.Transform(value, rotation);

    private static PointF Project(Vector3 point, float centerX, float centerY)
    {
        const float cameraDistance = 4.8f;
        const float focalLength = 300f;
        var scale = focalLength / (cameraDistance - point.Z);
        return new PointF(centerX + point.X * scale, centerY - point.Y * scale);
    }

    private static float ProjectedRadius(Vector3 point, float centerX, float centerY, float radius)
    {
        var a = Project(point, centerX, centerY);
        var b = Project(point + new Vector3(radius, 0, 0), centerX, centerY);
        return MathF.Max(2, MathF.Abs(b.X - a.X));
    }

    private static Vector2[] Pips(int value)
    {
        const float d = 0.48f;
        const float m = 0.53f;
        return value switch
        {
            1 => [Vector2.Zero],
            2 => [new(-d, -d), new(d, d)],
            3 => [new(-d, -d), Vector2.Zero, new(d, d)],
            4 => [new(-d, -d), new(d, -d), new(-d, d), new(d, d)],
            5 => [new(-d, -d), new(d, -d), Vector2.Zero, new(-d, d), new(d, d)],
            6 => [new(-d, -m), new(d, -m), new(-d, 0), new(d, 0), new(-d, m), new(d, m)],
            _ => []
        };
    }

    private sealed record Face(int Value, Vector3 Normal, Vector3 U, Vector3 V, Color Color);
    private sealed record RenderedFace(Face Face, float Depth);
}
