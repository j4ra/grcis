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

scene.Background = new DefaultBackground();
scene.BackgroundColor = new double[] {0.2, 0.3, 0.5};

scene.Sources = new System.Collections.Generic.LinkedList<ILightSource>();
scene.Sources.Add(new AmbientLightSource(0.8));
scene.Sources.Add(new PointLightSource(new Vector3d(-0.25, 0.55, 0.75), 1.5));

scene.Camera = new StaticCamera(new Vector3d(0.7, 3.5, -6.0),
                                new Vector3d(0.0, -0.5, 1.0),
                                60.0);

Sphere s = new Sphere();
Plane pl = new Plane();

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Create new perlin texture with random seed 0, resolution of 128x128x128 (16MB). Starting amplitude 1.0 and bias 1.1. See README.md for more info.
var perTex = new PerlinTexture(/*seed*/0, /*resolution*/128, /*amplitude*/1.0, /*bias*/1.1);

//Assign a custom mapping function. The parameter i is Intersection of Ray intersectiong with surface.
perTex.Mapping = i =>
{
    //create color array with length equal to surface color lenght (so that the number of bands match).
    double[] color = new double[i.SurfaceColor.Length];
    //Sample the texture. The last two parameters controls which octaves are sampled (see README.md for more info)
    double perColor = perTex.Perlin3D(i.CoordLocal.X * 2, i.CoordLocal.Y * 2, i.CoordLocal.Z * 2, /*starting octave*/0, /*highest octave*/7);
    //This using the color extension functions. The FillFlat will fill all color bands with the the value provided as its parameter. 
    //                                          The BrightnessContrast modifies brightness and contrast (the last parameter is contrast neutral point).
    //                                          The Finalize() converts the IEnumerable<double> to double[] and saturates the colors (forces the bands between 0.0 and 1.0).
    color = color.FillFlat(perColor).BrightnessContrast(1.5, 2.0, 0.5).Finalize();
    //Apply color as surface color
    i.SurfaceColor = color;
    //Mark the texture applied
    i.textureApplied = true;
    //Return hash for variable multisampling (the texture is smooth so no need for changing hash).
    return 1L;
};
//Assign texture to the sphere.
s.SetAttribute(PropertyName.TEXTURE, perTex);

//Create new Perlin texture. With random seed 2, resolution 256 (134MB), amplitude 1.0 and bias 1.4
var tex = new PerlinTexture(/*seed*/2, /*resolution*/256, /*amplitude*/1.0, /*bias*/1.4);
//Create new Voronoi texture. With 16x16x16 cells (each cell is 24 bytes) and the random seed is 1.
var vorTex = new VoronoiTexture(/*resolution*/16, /*seed*/1);
//Again, we will override the mapping of the voronoi texture.
vorTex.Mapping = i =>
{
    //Create array with correct number of bands.
    double[] color = new double[i.SurfaceColor.Length];

    //Sample voronoi texture
    double vorCol = vorTex.GetDistance3D(new Vector3d(i.TextureCoord.X / 48, i.TextureCoord.Y / 40, (i.TextureCoord.X + i.TextureCoord.Y) / 40)); 

    //Sample perlin texture. Notice that we can sample multiple different textures in one mapping function.
    double colorPerl =  tex.Perlin2D(i.TextureCoord.X / 32, i.TextureCoord.Y / 24, /*starting octave*/7, /*highest octave*/12);
    
    //Assign color to the intersection surface.
    //                  The AsColor and AsFactor translates between double and IEnumerable<double> (so that we can use color extensions on it).
    //                  The Invert function inverts the color. (Creates negative image).
    i.SurfaceColor = color
                    .FillFlat(vorCol)
                    .Mul(colorPerl.AsColor().BrightnessContrast(0.8, 2.0, 0.8).AsFactor())
                    .Invert()
                    .BrightnessContrast(1.0, 2.0, 0.9)
                    .Finalize();
    return 1L;
};
//Assign texture with overriden mapping to the plane
pl.SetAttribute(PropertyName.TEXTURE, vorTex);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Crete and assign a shiny material to the plane (it should resemble polished marble)
PhongMaterial shiny = new PhongMaterial(new double[] { 0.05, 0.05, 0.05 }, 0.25, 0.25, 0.5, 128);
pl.SetAttribute(PropertyName.MATERIAL, shiny);

root.InsertChild(s, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.0, 2.4));
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));
