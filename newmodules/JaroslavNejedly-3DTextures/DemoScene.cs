using JaroslavNejedly;
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
scene.Sources.Add(new AmbientLightSource(10.4));
//scene.Sources.Add(new PointLightSource(new Vector3d(-5.0, 4.0, -3.0), 1.2));

scene.Camera = new StaticCamera(new Vector3d(0.7, 10.5, -8.0),
                                new Vector3d(0.0, -1.0, 0.2),
                                90.0);


Sphere s;
s = new Sphere();
s.SetAttribute(PropertyName.MATERIAL, pm);
root.InsertChild(s, Matrix4d.Identity);
s = new Sphere();
root.InsertChild(s, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.2, 2.4));
Plane pl = new Plane();
pl.SetAttribute(PropertyName.COLOR, new double[] {0.3, 0.0, 0.0});
var tex = new PerlinTexture(0, 2048, 1.0, 1.1, 0, 10);
tex.Mapping = i =>
{
    tex.GetTexel(new Vector3d(i.TextureCoord.X / 10, i.TextureCoord.Y / 10, 0), i.SurfaceColor);
    i.textureApplied = true;
    return 1L; 
};
pl.SetAttribute(PropertyName.TEXTURE, tex);
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));