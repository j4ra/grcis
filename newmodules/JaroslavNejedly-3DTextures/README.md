# Extension: 3D noise textures

![example0](imgs/img0.png)

### Author: Jaroslav Nejedlý

### Category: Texture

### Namespace: JaroslavNejedly, JaroslavNejedly.Extensions

### Class Name: 
 #### PerlinTexture
 #### VoronoiTexture

### ITimeDependent: No

### Source file: Texture3D.cs

Description.

### Examples &amp; sample scripts:

Example.

### Issues and things to be aware of:

---

## Color Extensions

The source file also contains useful extension functions that work on `IEnumerable<double>`. So you can use them on any type that derives from this interface. The synax is similar to LINQ fluent syntax.

Sample code looks like this:
```C#
double[] color = new double[3];
double[] modifiedColor = color.BrightnessContrast(0.7, 1.4).Invert().Mul(1.3).Gamma(2.2).Finalize();
```

The available functions are:
 - Add  
   - adds other color (`IEnumerable<double>`) or scalar value
 - Mul  
   - multiplies by other color or scalar value
 - MulSaturated
   - multiplies by scalar value that is saturated (clamped between 0.0 and 1.0)
 - Gamma
   - applies gamma (exponent)
 - Mix
   - mixes two colors together
 - BrightnessContrast
   - Applies brightness/contrast operator (see [here](https://docs.nvidia.com/deeplearning/dali/user-guide/docs/examples/image_processing/brightness_contrast_example.html))
 - Invert
   - Creates negative of the color
 - FillFlat
   - Fills each band of a color with provided value
 - Finalize
   - Converts `IEnumerable<double>` into array `double[]` and saturates each band (clapms it between 0.0 and 1.0)
   - If you don't want the stauration, call the LINQ function `ToArray()` (you will have to include System.LINQ namespace)

Additionally there are functions that do not operate on `IEnumerable<double>`:
 - AsColor
   - `double` extesion that converts the value into `IEnumerable<double>` with only one element (itself), useful if you want to apply any of the color operations described above
 - AsFactor
   - Converts `IEnumerable<double>` back to `double`. Used after applying color operators after AsColor function.
 - ColorRamp
   - UNTESTED. This should interpolate between colors (passed as second argument) based on parameter "factor".
 - ColorFromHSV
   - UNTESTED. This should create `double[]` containing rgb values corresponding to hsv color.

## Images

##### 1D Perlin Low Octaves

![perlin 1D low octaves](imgs/Perlin1DLowOctaves.png)

##### 1D Perlin High Octaves

![perlin 1D high octaves](imgs/Perlin1DHighOctaves.png)

##### 2D Perlin Low Octaves

![perlin 2D low octaves](imgs/Perlin2DLowOctaves.png)

##### 2D Perlin High Octaves

![perlin 2D high octaves](imgs/Perlin2DHighOctaves.png)

##### 3D Perlin (single slice)

![perlin 3D (slice)](imgs/Perlin3D.png)

##### 2D Voronoi

![2D voronoi](imgs/Voronoi2D.png)

##### 3D Voronoi

![3D voronoi](imgs/voronoi3D.png)

Notice how the cells from "above" influences the texture in comparison with the 2D version.

##### By using clever mapping we can simulate different materials.

![Material](imgs/img0.png)
