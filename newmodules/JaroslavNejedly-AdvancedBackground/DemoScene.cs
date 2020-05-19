//////////////////////////////////////////////////
// Externals.

using JaroslavNejedly;
using System.Linq.Expressions;

//////////////////////////////////////////////////
Debug.Assert(scene != null);
Debug.Assert(context != null);


PhongMaterial pm = new PhongMaterial(new double[] { 0.05, 0.05, 0.05 }, 0.05, 0.05, 0.3, 128);
pm.n = 1.45;
pm.Kt = 0.9;

//////////////////////////////////////////////////
// CSG scene.

CSGInnerNode root = new CSGInnerNode(SetOperation.Union);
root.SetAttribute(PropertyName.REFLECTANCE_MODEL, new PhongModel());
root.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] {1.0, 0.7, 0.1}, 0.05, 0.45, 0.05, 64));
scene.Intersectable = root;

// Background color.
scene.BackgroundColor = new double[] {0.3, 0.5, 0.65};
var advBackground = new AdvancedBackground(new double[] { 0.10, 0.22, 0.83 }, new double[] { 0.62, 0.74, 0.89 }, Vector3d.UnitY);
advBackground.SunDir = new Vector3d(-1.1, 2.5, 5.0);
advBackground.SunIntensity = new double[] {2.4, 2.3, 2.3};
advBackground.SunSmallness = 450;
scene.Background = advBackground;
//scene.Background = new DefaultBackground(scene);

// Camera.
scene.Camera = new StaticCamera(new Vector3d(0.7, 0.5, -5.0),
                                new Vector3d(0.0, 0.18, 1.0),
                                80.0);

// Light sources.
scene.Sources = new System.Collections.Generic.LinkedList<ILightSource>();
scene.Sources.Add(new AmbientLightSource(0.8));
//scene.Sources.Add(new PointLightSource(new Vector3d(-5.0, 4.0, -3.0), 1.2));
scene.Sources.Add(advBackground);

// --- NODE DEFINITIONS ----------------------------------------------------

// Transparent/mirror/diffuse sphere.
Sphere s;
s = new Sphere();
s.SetAttribute(PropertyName.MATERIAL, pm);
root.InsertChild(s, Matrix4d.Identity);

// Opaque sphere.
s = new Sphere();
root.InsertChild(s, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.2, 2.4));

// Infinite plane with checker.
Plane pl = new Plane();
pl.SetAttribute(PropertyName.COLOR, new double[] {0.3, 0.0, 0.0});
pl.SetAttribute(PropertyName.TEXTURE, new CheckerTexture(0.6, 0.6, new double[] {1.0, 1.0, 1.0}));
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));
