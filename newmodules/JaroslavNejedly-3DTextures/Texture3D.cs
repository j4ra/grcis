using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JaroslavNejedly.Extensions
{
  /// <summary>
  /// Extension functions for IEnumerable&lt;double&gt;
  /// </summary>
  public static class ColorExtensions
  {
    /// <summary>
    /// Converts double into IEnumerable&lt;double&gt; containing only one element (itself)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static IEnumerable<double> AsColor(this double input)
    {
      yield return input;
    }

    /// <summary>
    /// Converts IEnumerable&lt;double&gt; back to double. Uses the first element of the collection. 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static double AsFactor(this IEnumerable<double> input)
    {
      return input.FirstOrDefault();
    }

    /// <summary>
    /// Adds two colors together.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static IEnumerable<double> Add (this IEnumerable<double> input, IEnumerable<double> other)
    {
      return input.Zip(other, (c0, c1) => c0 + c1);
    }

    /// <summary>
    /// Adds constant to all bands of the color.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="constant"></param>
    /// <returns></returns>
    public static IEnumerable<double> Add (this IEnumerable<double> input, double constant)
    {
      return input.Select(c => c + constant);
    }

    /// <summary>
    /// Multiplies two color band by band.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static IEnumerable<double> Mul (this IEnumerable<double> input, IEnumerable<double> other)
    {
      return input.Zip(other, (c0, c1) => c0 * c1);
    }

    /// <summary>
    /// Multiplies color by factor
    /// </summary>
    /// <param name="input"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    public static IEnumerable<double> Mul (this IEnumerable<double> input, double factor)
    {
      return input.Select(c => c * factor);
    }

    /// <summary>
    /// Multiplies color by saturated factor
    /// </summary>
    /// <param name="input"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    public static IEnumerable<double> MulSaturated (this IEnumerable<double> input, double factor)
    {
      double f = Math.Min(1.0, Math.Max(0.0, factor));
      return input.Select(c => c * f);
    }

    /// <summary>
    /// Applies gamma to the color
    /// </summary>
    /// <param name="input"></param>
    /// <param name="gamma"></param>
    /// <returns></returns>
    public static IEnumerable<double> Gamma(this IEnumerable<double> input, double gamma)
    {
      return input.Select(c => Math.Pow(c, gamma));
    }

    /// <summary>
    /// Mixes two colors together according to <paramref name="ratio"/>
    /// </summary>
    /// <param name="input"></param>
    /// <param name="other"></param>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static IEnumerable<double> Mix (this IEnumerable<double> input, IEnumerable<double> other, double ratio = 0.5)
    {
      return input.Zip(other, (c0, c1) => c0 + ratio * (c1 - c0));
    }

    /// <summary>
    /// Changes brightness and contrast.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="brightness"></param>
    /// <param name="contrast"></param>
    /// <param name="contrastMidpoint">value unaffected by contrast changes</param>
    /// <param name="brightnessShift">Additive constant</param>
    /// <returns></returns>
    public static IEnumerable<double> BrightnessContrast (this IEnumerable<double> input, double brightness, double contrast, double contrastMidpoint = 0.5, double brightnessShift = 0.0)
    {
      return input.Select(c => brightnessShift + brightness * (contrastMidpoint + contrast * (c - contrastMidpoint)));
    }

    /// <summary>
    /// Creates negative color.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static IEnumerable<double> Invert (this IEnumerable<double> input)
    {
      return input.Select(c => 1 - c);
    }

    /// <summary>
    /// Fills all bands of color with the value of <paramref name="color"/>.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static IEnumerable<double> FillFlat(this IEnumerable<double> input, double color)
    {
      return input.Select(i => color);
    }

    /// <summary>
    /// Basic color ramp. Based on factor it mixes values from <paramref name="colors"/> parameter.
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static IEnumerable<double> ColorRamp(double factor, params IEnumerable<double>[] colors)
    {
      if (colors.Length < 2)
        return colors.FirstOrDefault();

      factor = Math.Min(Math.Max(factor, 0.0), 1.0);

      int steps = (colors.Length - 1);
      double stepSize = 1.0 / steps;
      int c0 = (int)(factor * steps);
      int c1 = c0 + 1;

      if (c1 >= colors.Length)
        return colors[c0];

      double mixFactor = (factor - (c0 * stepSize)) / stepSize;
      return colors[c0].Mix(colors[c1], mixFactor);
    }

    /// <summary>
    /// Converts IEnumerable&lt;double&gt; into array and saturates all bands to range 0.0 and 1.0
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static double[] Finalize (this IEnumerable<double> color)
    {
      return color.Select(c => Math.Min(Math.Max(0.0, c), 1.0)).ToArray();
    }

    /// <summary>
    /// Creates color from HSV. Addapted from <a href="https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb">stack overflow</a>.
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="saturation"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double[] ColorFromHSV (double hue, double saturation, double value)
    {
      int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
      double f = hue / 60 - Math.Floor(hue / 60);

      value = value * 255;
      int v = Convert.ToInt32(value);
      int p = Convert.ToInt32(value * (1 - saturation));
      int q = Convert.ToInt32(value * (1 - f * saturation));
      int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

      if (hi == 0)
        return new double[] { v / 255.0, t / 255.0, p / 255.0 };
      else if (hi == 1)
        return new double[] { q / 255.0, v / 255.0, p / 255.0 };
      else if (hi == 2)
        return new double[] { p / 255.0, v / 255.0, t / 255.0 };
      else if (hi == 3)
        return new double[] { p / 255.0, q / 255.0, v / 255.0 };
      else if (hi == 4)
        return new double[] { t / 255.0, p / 255.0, v / 255.0 };
      else
        return new double[] { v / 255.0, p / 255.0, q / 255.0 };
    }
  }
}

namespace JaroslavNejedly
{
  /// <summary>
  /// Abstract class representing 3D texture.
  /// </summary>
  public abstract class Texture3D : ITexture
  {
    /// <summary>
    /// Mapping function. It takes <see cref="Intersection"/> as an argument, modifies it and returns hash code, that is used for adaptive multisampling.
    /// </summary>
    public virtual Func<Intersection, long> Mapping { get; set; }

    public Texture3D()
    {
      Mapping = i =>
      {
        long ret = GetTexel(new Vector3d(i.TextureCoord.X, i.TextureCoord.Y, 0), i.SurfaceColor);
        i.textureApplied = true;
        return ret;
      };
    }

    /// <inheritdoc/>
    public virtual long Apply (Intersection inter)
    {
       return Mapping(inter);
    }

    /// <summary>
    /// Basic implementation of 3d texture sample. (Used by default <see cref="Mapping"/>).
    /// </summary>
    /// <param name="texCoord"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public abstract long GetTexel (Vector3d texCoord, double[] color);
  }

  /// <summary>
  /// Perlin texture implementation.
  /// </summary>
  public class PerlinTexture : Texture3D
  {
    private double[] _seed;

    private readonly int res = 256;

    private double amp = 1.0;
    private double bias = 1.4;

    /// <summary>
    /// Creates an iunstance of Perlin texture.
    /// </summary>
    /// <param name="seed">Random generator seed. Use same seed for same results.</param>
    /// <param name="res">Resolution of the texture (beware as the memory used is proporitonal to the cube of resolution)</param>
    /// <param name="amp">Amplitude of the first octave.</param>
    /// <param name="bias">The attenuation factor of higher octaves. Amplitude of each higher octave is lower based on this parameter.</param>
    public PerlinTexture (long seed = 0, int res = 256, double amp = 1.0, double bias = 2.0) : base()
    {
      this.res = res;
      this.amp = amp;
      this.bias = bias;

      Random rand = new Random((int)seed);
      _seed = new double[res * res * res];

      for(int i = 0; i < res * res * res; i++)
      {
        _seed[i] = rand.NextDouble();
      }
    }

    /// <inheritdoc/>
    public override long GetTexel (Vector3d texCoord, double[] color)
    {
      double value =  Perlin2D(texCoord.X, texCoord.Y);
      for(int i = 0; i < color.Length; i++)
      {
        color[i] = value;
      }

      return (long)value;
    }

    /// <summary>
    /// Smaples the 3D perlin nosie at a position <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="minOctave">Starting octave.</param>
    /// <param name="maxOctave">Ending octave.</param>
    /// <returns></returns>
    public double Perlin3D (double x, double y, double z, int minOctave = 0, int maxOctave = 8)
    {
      int maxOctaves = (int)Math.Log(res, 2);
      if (maxOctave > maxOctaves)
      {
        maxOctave = maxOctaves;
      }

      double result = 0.0;
      double ampAcc = 0.0;
      double curAmp = amp;

      x = x - Math.Floor(x);
      y = y - Math.Floor(y);
      z = z - Math.Floor(z);
      int pX = (int)(x * res);
      int pY = (int)(y * res);
      int pZ = (int)(z * res);

      for (int i = minOctave; i < maxOctave; i++)
      {
        int pitch = res >> i;

        int sampleX0 = (pX / pitch) * pitch;
        int sampleY0 = (pY / pitch) * pitch;
        int sampleZ0 = (pZ / pitch) * pitch;

        int sampleX1 = (sampleX0 + pitch) % res;
        int sampleY1 = (sampleY0 + pitch) % res;
        int sampleZ1 = (sampleZ0 + pitch) % res;

        double blendX = (x * res - sampleX0) / pitch;
        double blendY = (y * res - sampleY0) / pitch;
        double blendZ = (z * res - sampleZ0) / pitch;

        double result0Z0 = SampleNoise3D(sampleZ0, sampleY0, sampleX0) + blendX * (SampleNoise3D(sampleZ0, sampleY0, sampleX1) - SampleNoise3D(sampleZ0, sampleY0, sampleX0));
        double result1Z0 = SampleNoise3D(sampleZ0, sampleY1, sampleX0) + blendX * (SampleNoise3D(sampleZ0, sampleY1, sampleX1) - SampleNoise3D(sampleZ0, sampleY1, sampleX0));
        double result0Z1 = SampleNoise3D(sampleZ1, sampleY0, sampleX0) + blendX * (SampleNoise3D(sampleZ1, sampleY0, sampleX1) - SampleNoise3D(sampleZ1, sampleY0, sampleX0));
        double result1Z1 = SampleNoise3D(sampleZ1, sampleY1, sampleX0) + blendX * (SampleNoise3D(sampleZ1, sampleY1, sampleX1) - SampleNoise3D(sampleZ1, sampleY1, sampleX0));

        double result0 = result0Z0 + blendY * (result1Z0 - result0Z0);
        double result1 = result0Z1 + blendY * (result1Z1 - result0Z1);

        result += (result0 + blendZ * (result1 - result0)) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }

    /// <summary>
    /// Smaples the 2D perlin nosie at a position <paramref name="x"/>, <paramref name="y"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="minOctave">Starting octave.</param>
    /// <param name="maxOctave">Ending octave.</param>
    /// <returns></returns>
    public double Perlin2D(double x, double y, int minOctave = 0, int maxOctave = 12)
    {
      int maxOctaves = (int)Math.Log(res * res * res, 2);
      maxOctaves = maxOctaves / 2;
      if (maxOctave > maxOctaves)
      {
        maxOctave = maxOctaves;
      }

      int r = (int)Math.Pow(2, maxOctaves);

      double result = 0.0;
      double ampAcc = 0.0;
      double curAmp = amp;

      x = x - Math.Floor(x);
      y = y - Math.Floor(y);
      int pX = (int)(x * r);
      int pY = (int)(y * r);

      for (int i = minOctave; i < maxOctave; i++)
      {
        int pitch = r >> i;

        int sampleX0 = (pX / pitch) * pitch;
        int sampleY0 = (pY / pitch) * pitch;

        int sampleX1 = (sampleX0 + pitch) % r;
        int sampleY1 = (sampleY0 + pitch) % r;

        double blendX = (x * r - sampleX0) / pitch;
        double blendY = (y * r - sampleY0) / pitch;

        double result0 = SampleNoise2D(sampleX0, sampleY0, r) + blendX * (SampleNoise2D(sampleX1, sampleY0, r) - SampleNoise2D(sampleX0, sampleY0, r));
        double result1 = SampleNoise2D(sampleX0, sampleY1, r) + blendX * (SampleNoise2D(sampleX1, sampleY1, r) - SampleNoise2D(sampleX0, sampleY1, r));

        result += (result0 + blendY * (result1 - result0)) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }

    /// <summary>
    /// Smaples the 1D perlin nosie at a position <paramref name="x"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="minOctave">Starting octave.</param>
    /// <param name="maxOctave">Ending octave.</param>
    /// <returns></returns>
    public double Perlin1D(double x, int minOctave = 0, int maxOctave = 24)
    {
      int r = res * res * res;
      int maxOctaves = (int)Math.Log(r, 2);
      if (maxOctave > maxOctaves)
      {
        maxOctave = maxOctaves;
      }

      double result = 0.0;
      double ampAcc = 0.0;
      double curAmp = amp;

      x = x - Math.Floor(x);
      int pX = (int)(x * r);

      for(int i = minOctave; i < maxOctave; i++)
      {
        int pitch = r >> i;
        int sample0 = (pX / pitch) * pitch;
        int sample1 = (sample0 + pitch) % r;

        double blend = (x * r - sample0) / pitch;

        result += (SampleNoise1D(sample0) + blend * (SampleNoise1D(sample1) - SampleNoise1D(sample0))) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }

    private double SampleNoise3D(int x, int y, int z)
    {
      x = (x + res) % res;
      y = (y + res) % res;
      z = (z + res) % res;

      return _seed[x + y * res + z * res * res];
    }

    private double SampleNoise2D (int x, int y, int r)
    {
      x = (x + r) % r;
      y = (y + r) % r;

      return _seed[x + y * r];
    }

    private double SampleNoise1D (int x)
    {
      int r = res * res * res;
      x = (x + r) % r;

      return _seed[x];
    }

  }

  /// <summary>
  /// Voronoi texture implementation.
  /// </summary>
  public class VoronoiTexture : Texture3D
  {
    private Vector3d[,,] seed;
    private readonly int res;

    /// <summary>
    /// Creates instance of voronoi texture.
    /// </summary>
    /// <param name="res">Determines the number of cells.</param>
    /// <param name="randSeed">Random seed. Use same seed for same results.</param>
    public VoronoiTexture(int res = 16, int randSeed = 0)
    {
      this.res = res;   

      Random rand = new Random(randSeed);
      seed = new Vector3d[res , res , res];
        for(int x = 0; x < res; x++)
          for(int y = 0; y < res; y++)
            for(int z = 0; z < res; z++)
            {
              seed[x, y, z] = new Vector3d(
                  (rand.NextDouble() + x) / res,
                  (rand.NextDouble() + y) / res,
                  (rand.NextDouble() + z) / res
                );
            }
    }

    /// <inheritdoc/>
    public override long GetTexel (Vector3d texCoord, double[] color)
    {
      double value = GetDistance3D(texCoord);
      for(int i = 0; i < color.Length; i++)
      {
        color[i] = value;
      }
      return 2L;
    }

    /// <summary>
    /// Sample 3D voronoi texture at a position defined by <paramref name="pos"/> parameter.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public double GetDistance3D(Vector3d pos)
    {
      double maxDist = 1.0 / res * Math.Sqrt(3);

      double x = pos.X - Math.Floor(pos.X);
      double y = pos.Y - Math.Floor(pos.Y);
      double z = pos.Z - Math.Floor(pos.Z);

      int xIndex = (int)(x * res) % res;
      int yIndex = (int)(y * res) % res;
      int zIndex = (int)(z * res) % res;

      Vector3d cubePos = new Vector3d(x,y,z);
      double minDist = double.MaxValue;
      for(int i = xIndex - 1; i < xIndex + 2; i++)
      for(int j = yIndex - 1; j < yIndex + 2; j++)
      for(int k = zIndex - 1; k < zIndex + 2; k++)
          {
            double temp = Vector3d.DistanceSquared(SampleWraped3D(i, j, k), cubePos);
            if (temp < minDist)
              minDist = temp;
          }

      return Math.Sqrt(minDist) / maxDist;
    }

    /// <summary>
    /// Sample 2D voronoi texture at a position defined by <paramref name="pos"/> parameter.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public double GetDistance2D (Vector2d pos)
    {
      double maxDist = 1.0 / res * Math.Sqrt(2);

      double x = pos.X - Math.Floor(pos.X);
      double y = pos.Y - Math.Floor(pos.Y);

      int xIndex = (int)(x * res) % res;
      int yIndex = (int)(y * res) % res;

      Vector2d cubePos = new Vector2d(x,y);
      double minDist = double.MaxValue;
      for (int i = xIndex - 1; i < xIndex + 2; i++)
        for (int j = yIndex - 1; j < yIndex + 2; j++)
        {
          
          double temp = Vector2d.DistanceSquared(SampleWraped2D(i, j), cubePos);
          if (temp < minDist)
            minDist = temp;
        }

      return Math.Sqrt(minDist) / maxDist;
    }

    private Vector3d SampleWraped3D(int i, int j, int k)
    {
      int wrapX, wrapY, wrapZ;
      Vector3d temp = seed[Wrap(i, out wrapX), Wrap(j, out wrapY), Wrap(k, out wrapZ)];
      return new Vector3d(temp.X - wrapX, temp.Y - wrapY, temp.Z - wrapZ);
    }

    private Vector2d SampleWraped2D (int i, int j)
    {
      int wrapX, wrapY;
      Vector3d temp = seed[Wrap(i, out wrapX), Wrap(j, out wrapY), 0];
      return new Vector2d(temp.X - wrapX, temp.Y - wrapY);
    }

    private int Wrap(int index, out int wraped)
    {
      if (index < 0)
      {
        wraped = 1;
        return index + res;
      }
      if (index >= res)
      {
        wraped = -1;
        return index - res;
      }
      wraped = 0;
      return index;
    }
  }
}
