using JaroslavNejedly;
using JaroslavNejedly.Extensions;
using System.Linq;
using System.Linq.Expressions;

Debug.Assert(scene != null);
Debug.Assert(context != null);


PhongMaterial pm = new PhongMaterial(new double[] { 0.05, 0.05, 0.05 }, 0.05, 0.05, 0.3, 128);
pm.n = 2;
pm.Kt = 0.6;
CSGInnerNode root = new CSGInnerNode(SetOperation.Union);
root.SetAttribute(PropertyName.REFLECTANCE_MODEL, new PhongModel());
root.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] {1.0, 0.7, 0.1}, 0.05, 0.45, 0.05, 64));
scene.Intersectable = root;

scene.Background = new DefaultBackground();
scene.BackgroundColor = new double[] {0.2, 0.3, 0.5};

scene.Sources = new System.Collections.Generic.LinkedList<ILightSource>();
scene.Sources.Add(new AmbientLightSource(0.8));
scene.Sources.Add(new PointLightSource(new Vector3d(-5.0, 4.0, -3.0), 75));

scene.Camera = new StaticCamera(new Vector3d(0.7, 2.5, -8.0),
                                new Vector3d(0.0, -0.1, 0.2),
                                90.0);


Sphere s;
s = new Sphere();
s.SetAttribute(PropertyName.MATERIAL, pm);
root.InsertChild(s, Matrix4d.Identity);
s = new Sphere();
root.InsertChild(s, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.2, 2.4));
Plane pl = new Plane();
pl.SetAttribute(PropertyName.COLOR, new double[] {0.3, 0.0, 0.0});
var tex = new PerlinTexture(0, 2048, 1.0, 1.4, 5, 11);
tex.Mapping = i =>
{
    double[] color0 = new double[i.SurfaceColor.Length];
    tex.GetTexel(new Vector3d(i.TextureCoord.X / 4, i.TextureCoord.Y / 4, 0), color0);
    double[] color1 = new double[color0.Length];
    tex.GetTexel(new Vector3d(i.TextureCoord.X / 3.4 + 0.123, i.TextureCoord.Y / 3.4 - 0.31, 0), color1);

    i.SurfaceColor = color0.Zip(color1, (c0, c1) => 1 - (0.7  + 2.8 * (c1 * c0 - 0.7))).ToArray();
    i.textureApplied = true;
    return 1L;
};

var vorTex = new VoronoiTexture();
vorTex.Mapping = i =>
{
    double[] color = new double[i.SurfaceColor.Length];
    vorTex.GetTexel(new Vector3d(i.TextureCoord.X / 24, i.TextureCoord.Y / 32, 0), color);
    double[] colorPerl = new double[color.Length];
    tex.GetTexel(new Vector3d(i.TextureCoord.X / 2, i.TextureCoord.Y / 4, 0), colorPerl);


    i.SurfaceColor = color.Invert().Mul(color).Invert().Mul(color).Mul(colorPerl).Invert().BrightnessContrast(1.0, 5, 0.8).Mul(0.65).Finalize();
    i.textureApplied = true;
    return 1L;
};

PhongMaterial shiny = new PhongMaterial(new double[] { 0.05, 0.05, 0.05 }, 0.15, 0.25, 0.6, 128);
pl.SetAttribute(PropertyName.MATERIAL, shiny);
pl.SetAttribute(PropertyName.TEXTURE, vorTex);
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));