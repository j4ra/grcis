using JaroslavNejedly;              //3D textures
using JaroslavNejedly.Extensions;   //Color functions


//Standard scene setup:
Debug.Assert(scene != null);
Debug.Assert(context != null);

PhongMaterial pm = new PhongMaterial(new double[] { 0.05, 0.05, 0.05 }, 0.05, 0.05, 0.3, 128);
pm.n = 2;
pm.Kt = 0.6;
CSGInnerNode root = new CSGInnerNode(SetOperation.Union);
root.SetAttribute(PropertyName.REFLECTANCE_MODEL, new PhongModel());
root.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] {1.0, 0.7, 0.1}, 0.15, 0.4, 0.05, 64));
scene.Intersectable = root;

var background = new AdvancedBackground();
scene.Background = background;
scene.BackgroundColor = new double[] {0.4, 0.6, 0.9};

scene.Sources = new System.Collections.Generic.LinkedList<ILightSource>();
scene.Sources.Add(background.Sun);
scene.Sources.Add(new AmbientLightSource(0.1));
//scene.Sources.Add(new PointLightSource(new Vector3d(-0.25, 0.55, 0.75), 1.5));

scene.Camera = new StaticCamera(new Vector3d(0.7, 3.5, -6.0),
                                new Vector3d(0.0, 0.2, 1.0),
                                60.0);

background.CurrentPreset.SunDirection = new Vector3d(0, 1.0, 1.0);

Cube c = new Cube();
Plane pl = new Plane();

var volClouds = new VolumetricClouds(scene, c);

c.SetAttribute(PropertyName.RECURSION, volClouds.rf);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Crete and assign a shiny material to the plane (it should resemble polished marble)
PhongMaterial shiny = new PhongMaterial(new double[] { 0.05, 0.05, 0.05 }, 0.25, 0.25, 0.5, 128);
//pl.SetAttribute(PropertyName.MATERIAL, shiny);

root.InsertChild(c, Matrix4d.Scale(35, 10, 30) * Matrix4d.CreateTranslation(-16, 5, 20));
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, 0.0, 0.0));
