using JosefPelikan;  //StarBackground
using JaroslavNejedly; //AnimatedAdvancedBackground, AnimatedSunLight

Debug.Assert(scene != null);
Debug.Assert(context != null);

AnimatedCSGInnerNode root = new AnimatedCSGInnerNode(SetOperation.Union);
root.SetAttribute(PropertyName.REFLECTANCE_MODEL, new PhongModel());
root.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] {1.0, 0.8, 0.1}, 0.1, 0.6, 0.4, 128));
scene.Intersectable = root;

scene.Animator = null;

AnimatedRayScene ascene = scene as AnimatedRayScene;
if (ascene != null)
{
  ascene.End = (double)context[PropertyName.CTX_END_ANIM];
  ascene.Start = (double)context[PropertyName.CTX_START_ANIM];
}

scene.Camera = new StaticCamera(new Vector3d(0.7, 0.5, -5.0),
                                new Vector3d(0.0, 0.18, 1.0),
                                60.0);

// Light sources:
scene.Sources = new LinkedList<ILightSource>();
scene.Sources.Add(new AmbientLightSource(0.8));




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BACKGROUND:
//First we create start and end presets. We use the default preset as a default value.
var startPreset = AdvancedBackgroundPreset.Default;
var endPreset = AdvancedBackgroundPreset.Default;

//Next we modify the preset to create interesting transition
startPreset.NightBackground = new JosefPelikan.StarBackground(startPreset.NightColor);
startPreset.SunIntensityMultiplier = -0.365;
startPreset.SunDirection = new Vector3d(0.6, -0.1, 1.0);

endPreset.SunIntensityMultiplier = 2.135;
endPreset.SunDirection = new Vector3d(0.5, 0.6, 0.9);

//Now we create the background object
var advBackground = new AnimatedAdvancedBackground();
//We need to apply the presets
advBackground.StartPreset = startPreset;
advBackground.EndPreset = endPreset;
//You can use your own function for interpolating sun direction.
//advBackground.SunDirectionAnimator = (t) => Vector3d.Lerp(startPreset.SunDirection, endPreset.SunDirection, t);
//Do not forget to set start and end of your animation!
advBackground.Start = (double)context[PropertyName.CTX_START_ANIM];
advBackground.End = (double)context[PropertyName.CTX_END_ANIM];
//Apply background and background color.
scene.Background = advBackground;
scene.BackgroundColor = startPreset.NightColor;

//ANIMATED SUN:
//First we have to create animated sun object.
var sun = new AnimatedSunLight();
//Now we apply the starting and ending presets
sun.StartPreset = startPreset;
sun.EndPreset = endPreset;
//You can use your own function for interpolating sun direction.
//sun.SunDirectionAnimator = (t) => Vector3d.Lerp(startPreset.SunDirection, endPreset.SunDirection, t);
//Do not forget to set start and end of your animation!
sun.End = (double)context[PropertyName.CTX_END_ANIM];
sun.Start = (double)context[PropertyName.CTX_START_ANIM];
//Apply the light into the scene.
scene.Sources.Add(sun);
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////



double n = 2;

Sphere s;
s = new Sphere();
PhongMaterial pm = new PhongMaterial(new double[] {0.0, 0.2, 0.1}, 0.03, 0.03, 0.08, 128);
pm.n  = n;
pm.Kt = 0.85;
s.SetAttribute(PropertyName.MATERIAL, pm);
root.InsertChild(s, Matrix4d.Identity);

s = new Sphere();
root.InsertChild(s, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.2, 2.4));

Plane pl = new Plane();
pl.SetAttribute(PropertyName.COLOR, new double[] {0.2, 0.03, 0.0});
pl.SetAttribute(PropertyName.TEXTURE, new CheckerTexture(0.6, 0.6, new double[] {1.0, 1.0, 1.0}));
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));