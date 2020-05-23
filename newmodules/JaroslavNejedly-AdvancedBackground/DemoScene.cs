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


////////////////////////////////////////////////////////////////////////////////////////////
// BACKGROUND
//CREATE DEFAULT SKY (With default preset)
var advBackground = new AdvancedBackground();
//APPLY BACKGROUND
scene.Background = advBackground;
scene.BackgroundColor = advBackground.CurrentPreset.NightColor;


scene.Sources = new System.Collections.Generic.LinkedList<ILightSource>();
scene.Sources.Add(new AmbientLightSource(0.4));
//ADD THE SUN LIGHT SOURCE FROM THE BACKGROUND INTO THE SCENE
scene.Sources.Add(advBackground.Sun);


scene.Camera = new StaticCamera(new Vector3d(0.7, 0.5, -5.0),
                                new Vector3d(0.0, 0.18, 1.0),
                                60.0);


Sphere s;
s = new Sphere();
s.SetAttribute(PropertyName.MATERIAL, pm);
root.InsertChild(s, Matrix4d.Identity);
s = new Sphere();
root.InsertChild(s, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.2, 2.4));
Plane pl = new Plane();
pl.SetAttribute(PropertyName.COLOR, new double[] {0.3, 0.0, 0.0});
pl.SetAttribute(PropertyName.TEXTURE, new CheckerTexture(0.6, 0.6, new double[] {1.0, 1.0, 1.0}));
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));

//YOU CAN CHANGE PRESET PARAMETERS ANYWHERE
advBackground.CurrentPreset.SunDirection = new Vector3d(0.6, -0.1, 1.0);
advBackground.CurrentPreset.SunIntensityMultiplier = 0.1;
//IT IS POSSIBLE TO INTEGRATE StarBackground when the sun is down.
//advBackground.CurrentPreset.NightBackground = new JosefPelikan.StarBackground(advBackground.CurrentPreset.NightColor);
