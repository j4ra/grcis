//////////////////////////////////////////////////
// Externals.

using JosefPelikan;
using JaroslavNejedly;

//////////////////////////////////////////////////
// Rendering params.

Debug.Assert(scene != null);
Debug.Assert(context != null);

//////////////////////////////////////////////////
// Preprocessing stage support.

// Uncomment the block if you need preprocessing.
/*
if (Util.TryParseBool(context, PropertyName.CTX_PREPROCESSING))
{
  double time = 0.0;
  bool single = Util.TryParse(context, PropertyName.CTX_TIME, ref time);
  // if (single) simulate only for a single frame with the given 'time'

  // TODO: put your preprocessing code here!
  //
  // It will be run only this time.
  // Store preprocessing results to arbitrary (non-reserved) context item,
  //  subsequent script calls will find it there...

  return;
}
*/

// Optional override of rendering algorithm and/or renderer.

//context[PropertyName.CTX_ALGORITHM] = new RayTracing();
//////////////////////////////////////////////////
// CSG scene.

AnimatedCSGInnerNode root = new AnimatedCSGInnerNode(SetOperation.Union);
root.SetAttribute(PropertyName.REFLECTANCE_MODEL, new PhongModel());
root.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] {1.0, 0.8, 0.1}, 0.1, 0.6, 0.4, 128));
scene.Intersectable = root;

// Optional Animator.
scene.Animator = null;

AnimatedRayScene ascene = scene as AnimatedRayScene;
if (ascene != null)
{
  ascene.End = (double)context[PropertyName.CTX_END_ANIM];
  ascene.Start = (double)context[PropertyName.CTX_START_ANIM];
}

// Background color.
var startPreset = AdvancedBackgroundPreset.Default;
var endPreset = AdvancedBackgroundPreset.Default;

startPreset.NightBackground = new JosefPelikan.StarBackground(startPreset.NightColor);
startPreset.SunIntensityMultiplier = -0.365;
startPreset.SunDirection = new Vector3d(0.6, -0.1, 1.0);

endPreset.SunIntensityMultiplier = 2.135;
endPreset.SunDirection = new Vector3d(0.5, 0.6, 0.9);

var advBackground = new AnimatedAdvancedBackground();
advBackground.StartPreset = startPreset;
advBackground.EndPreset = endPreset;
advBackground.Start = (double)context[PropertyName.CTX_START_ANIM];
advBackground.End = (double)context[PropertyName.CTX_END_ANIM];
scene.Background = advBackground;
scene.BackgroundColor = startPreset.NightColor;

// Camera.
scene.Camera = new StaticCamera(new Vector3d(0.7, 0.5, -5.0),
                                new Vector3d(0.0, 0.18, 1.0),
                                60.0);

// Light sources:
scene.Sources = new LinkedList<ILightSource>();
scene.Sources.Add(new AmbientLightSource(0.8));

var sun = new AnimatedSunLight();
sun.StartPreset = startPreset;
sun.EndPreset = endPreset;
sun.End = (double)context[PropertyName.CTX_END_ANIM];
sun.Start = (double)context[PropertyName.CTX_START_ANIM];
scene.Sources.Add(sun);

// --- NODE DEFINITIONS ----------------------------------------------------

// n = <index-of-refraction>
double n = 2;

// Transparent sphere.
Sphere s;
s = new Sphere();
PhongMaterial pm = new PhongMaterial(new double[] {0.0, 0.2, 0.1}, 0.03, 0.03, 0.08, 128);
pm.n  = n;
pm.Kt = 0.85;
s.SetAttribute(PropertyName.MATERIAL, pm);
root.InsertChild(s, Matrix4d.Identity);

// Opaque sphere.
s = new Sphere();
root.InsertChild(s, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.2, 2.4));

// Infinite plane with checker texture.
Plane pl = new Plane();
pl.SetAttribute(PropertyName.COLOR, new double[] {0.2, 0.03, 0.0});
pl.SetAttribute(PropertyName.TEXTURE, new CheckerTexture(0.6, 0.6, new double[] {1.0, 1.0, 1.0}));
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));