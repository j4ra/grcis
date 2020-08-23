using JaroslavNejedly;              //Volumetric  scattering, animated spot light
using DavidSosvald_MichalTopfer;    //Animator, Animated Camera, Animated Node Transform
using FilipSedlak_SonaMolnarova;    //Fast triangle mesh, ObjLoader
using JosefPelikan;                 //Star Background
using System.IO;                    //Path
using System.Collections.Generic;   //Collections



//Standard scene setup:
Debug.Assert(scene != null);
Debug.Assert(scene is ITimeDependent);
Debug.Assert(context != null);

//DEFAULT NODE AND MATERIAL
PhongMaterial defaultMat = new PhongMaterial(new double[] {1.0, 0.7, 0.1}, 0.15, 0.4, 0.05, 64);
AnimatedCSGInnerNode root = new AnimatedCSGInnerNode(SetOperation.Union);
root.SetAttribute(PropertyName.REFLECTANCE_MODEL, new PhongModel());
root.SetAttribute(PropertyName.MATERIAL, defaultMat);

scene.Intersectable = root;

///////////////////////////////////////////////
//Materials
PhongMaterial darkMat = new PhongMaterial(new double[] {0.2, 0.15, 0.1}, 0.18, 0.72, 0.1, 64);
PhongMaterial greenMat = new PhongMaterial(new double[] {0.3, 0.6, 0.3}, 0.6, 0.25, 0.0, 64);


//////////////////////////////////////////////
//ANIMATOR
Animator a; // 'a' is used to register params (names, parsers, interpolators) during scene creation
if (context.ContainsKey("animator")) {
    scene.Animator = (ITimeDependent)((Animator)context["animator"]).Clone();
    a = null; // params were already registered when Animator was created (scene is the same)
}
else {
    string keyframes_file = Path.Combine(Path.GetDirectoryName((string)context[PropertyName.CTX_SCRIPT_PATH]), "animace.yaml");
    a = new Animator(keyframes_file);
    scene.Animator = a;
    context["animator"] = a;
}

///////////////////////////////////////////
//LIGHTS
scene.Sources = new LinkedList<ILightSource>();

scene.Sources.Add(new AmbientLightSource(0.05));
scene.Sources.Add(new AnimatedSpotLight(a, "light_p", "light_d", "light_a", "light_i"));
//scene.Sources.Add(new SpotLight(new Vector3d(0, 0.5, 2.0), 1.0, new Vector3d(0, 1, 0), 45));
//scene.Sources.Add(new PointLightSource(new Vector3d(1.0, 15.0, 10.0), 1.8));
//scene.Sources.Add(new AttenuatedPointLight(new Vector3d(0.0, 1.0, 2.5), 15.8, 15.8));

/////////////////////////////////////////
//CAMERA
scene.Camera = new KeyframesAnimatedStaticCamera(a, "camera_p", "camera_d");

/////////////////////////////////////////
//BACKGROUND
scene.BackgroundColor = new double[] {0.01, 0.03, 0.06};
scene.Background =  new StarBackground(scene.BackgroundColor, 600, 0.006, 0.5, 1.6, 1.0);


////////////////////////////////////////
//FOG
//FOG is done directly in renderer...
// Cube c = new Cube();

// c.SetAttribute(PropertyName.RECURSION, new VolumetricClouds(scene, c).Recursion);
// c.SetAttribute(PropertyName.NO_SHADOW, true);
// root.InsertChild(c, Matrix4d.Scale(4, 3.0, 2.0) * Matrix4d.CreateTranslation(-2.0, -2.0, 4.5));
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//DEBUG Objects
// Plane pl = new Plane();
// root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));
// c = new Cube();
// root.InsertChild(c, Matrix4d.CreateTranslation(-0.5, -0.5, 5.0));

// ///////////////////////////////////////////////////
// //OBJs
ObjLoader obj = new ObjLoader();

//////////////////////////////////////////////////
//TEREN
List<FastTriangleMesh> meshes = obj.ParseObjects("res/teren.obj", false);
foreach(var mesh in meshes)
{
    mesh.SetAttribute(PropertyName.MATERIAL, greenMat);
    root.InsertChild(mesh, Matrix4d.Scale(0.2) * Matrix4d.CreateTranslation(1.5, -0.7, -0.5));
}

/////////////////////////////////////////////////
//TREE
meshes = obj.ParseObjects("res/strom.obj", false);
foreach(var mesh in meshes)
{
    mesh.SetAttribute(PropertyName.MATERIAL, darkMat);
    root.InsertChild(mesh, Matrix4d.Scale(0.1) * Matrix4d.RotateY(-0.2) * Matrix4d.CreateTranslation(0.65, -0.525, 2.0));
}

//////////////////////////////////////////////////
//SAUCER
meshes = obj.ParseObjects("res/talir.obj", true);
AnimatedNodeTransform ant = new AnimatedNodeTransform(a, "talir_t", "talir_r", null, null, null, new Vector3d(0.2, 0.2, 0.2));
foreach(var mesh in meshes)
{
    mesh.SetAttribute(PropertyName.NO_SHADOW, true);
    ant.InsertChild(mesh, Matrix4d.Identity);
}
root.InsertChild(ant, Matrix4d.Identity);


//Animator finish
if (a != null) {
    a.LoadKeyframes();
}

AnimatedRayScene ascene = scene as AnimatedRayScene;
if (ascene != null)
{
  ascene.End = (double)context[PropertyName.CTX_END_ANIM];
  ascene.Start = (double)context[PropertyName.CTX_START_ANIM];
}

//TODO: animations (code + actual animations)
//          - Obj animations: code ok
//          - Camera animations: code ok
//          - Light animations: code ok
//TODO: materials
//          - gorund
//          - tree
//          - fog
//          - saucer
//TODO: Render!