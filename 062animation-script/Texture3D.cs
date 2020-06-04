using MathSupport;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaroslavNejedly.Extensions
{
  public static class ColorExtensions
  {
    public static IEnumerable<double> Add (this IEnumerable<double> input, IEnumerable<double> other)
    {
      return input.Zip(other, (c0, c1) => c0 + c1);
    }

    public static IEnumerable<double> Add (this IEnumerable<double> input, double constant)
    {
      return input.Select(c => c + constant);
    }

    public static IEnumerable<double> Mul (this IEnumerable<double> input, IEnumerable<double> other)
    {
      return input.Zip(other, (c0, c1) => c0 * c1);
    }

    public static IEnumerable<double> Mul (this IEnumerable<double> input, double factor)
    {
      return input.Select(c => c * factor);
    }

    public static IEnumerable<double> Gamma(this IEnumerable<double> input, double gamma)
    {
      return input.Select(c => Math.Pow(c, gamma));
    }

    public static IEnumerable<double> Mix (this IEnumerable<double> input, IEnumerable<double> other, double ratio = 0.5)
    {
      return input.Zip(other, (c0, c1) => c0 + ratio * (c1 - c0));
    }

    public static IEnumerable<double> BrightnessContrast (this IEnumerable<double> input, double brightness, double contrast, double contrastMidpoint = 0.5, double brightnessShift = 0.0)
    {
      return input.Select(c => brightnessShift + brightness * (contrastMidpoint + contrast * (c - contrastMidpoint)));
    }

    public static IEnumerable<double> Invert (this IEnumerable<double> input)
    {
      return input.Select(c => 1 - c);
    }

    public static IEnumerable<double> FillFlat(this IEnumerable<double> input, double color)
    {
      return input.Select(i => color);
    }

    public static double[] Finalize (this IEnumerable<double> color)
    {
      return color.Select(c => Math.Min(Math.Max(0.0, c), 1.0)).ToArray();
    }
  }
}

namespace JaroslavNejedly
{

  public abstract class Texture3D : ITexture
  {
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

    public virtual Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

    public virtual long Apply (Intersection inter)
    {
       return Mapping(inter);
    }

    public abstract long GetTexel (Vector3d texCoord, double[] color);
  }

  public class PerlinTexture : Texture3D
  {
    private static double[] _seed;

    private long randSeed = 0;
    private int res = 512;
    private int maxOctaves;
    private int maxOctave;
    private int minOctave;

    private double amp = 1.0;
    private double bias = 1.4;

    public PerlinTexture(long seed = 0, int res = 512, double amp = 1.0, double bias = 2.0, int minOctave = 0, int maxOctave = 9) : base()
    {
      randSeed = seed;
      this.res = res;
      this.amp = amp;
      this.bias = bias;
      this.minOctave = minOctave;
      this.maxOctave = maxOctave;

      Random rand = new Random((int)randSeed);
      _seed = new double[res * res * res];

      for(int i = 0; i < res * res * res; i++)
      {
        _seed[i] = rand.NextDouble();
      }

      maxOctaves = (int)Math.Log(res, 2);
      if (this.maxOctave > maxOctaves)
        this.maxOctave = maxOctaves;
    }

    public override long GetTexel (Vector3d texCoord, double[] color)
    {
      double value =  Perlin2D(texCoord.X, texCoord.Y);
      for(int i = 0; i < color.Length; i++)
      {
        color[i] = value;
      }

      return (long)value;
    }

    public double Perlin3D (double x, double y, double z)
    {
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

        double result0Z0 = _seed[sampleZ0 * res * res + sampleY0 * res + sampleX0] + blendX * (_seed[sampleZ0 * res * res + sampleY0 * res + sampleX1] - _seed[sampleZ0 * res * res + sampleY0 * res + sampleX0]);
        double result1Z0 = _seed[sampleZ0 * res * res + sampleY1 * res + sampleX0] + blendX * (_seed[sampleZ0 * res * res + sampleY1 * res + sampleX1] - _seed[sampleZ0 * res * res + sampleY1 * res + sampleX0]);
        double result0Z1 = _seed[sampleZ1 * res * res + sampleY0 * res + sampleX0] + blendX * (_seed[sampleZ1 * res * res + sampleY0 * res + sampleX1] - _seed[sampleZ1 * res * res + sampleY0 * res + sampleX0]);
        double result1Z1 = _seed[sampleZ1 * res * res + sampleY1 * res + sampleX0] + blendX * (_seed[sampleZ1 * res * res + sampleY1 * res + sampleX1] - _seed[sampleZ1 * res * res + sampleY1 * res + sampleX0]);

        double result0 = result0Z0 + blendY * (result1Z0 - result0Z0);
        double result1 = result0Z1 + blendY * (result1Z1 - result0Z1);

        result += (result0 + blendZ * (result1 - result0)) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }

    public double Perlin2D(double x, double y)
    {
      double result = 0.0;
      double ampAcc = 0.0;
      double curAmp = amp;

      x = x - Math.Floor(x);
      y = y - Math.Floor(y);
      int pX = (int)(x * res);
      int pY = (int)(y * res);

      for (int i = minOctave; i < maxOctave; i++)
      {
        int pitch = res >> i;

        int sampleX0 = (pX / pitch) * pitch;
        int sampleY0 = (pY / pitch) * pitch;

        int sampleX1 = (sampleX0 + pitch) % res;
        int sampleY1 = (sampleY0 + pitch) % res;

        double blendX = (x * res - sampleX0) / pitch;
        double blendY = (y * res - sampleY0) / pitch;

        double result0 = _seed[sampleY0 * res + sampleX0] + blendX * (_seed[sampleY0 * res + sampleX1] - _seed[sampleY0 * res + sampleX0]);
        double result1 = _seed[sampleY1 * res + sampleX0] + blendX * (_seed[sampleY1 * res + sampleX1] - _seed[sampleY1 * res + sampleX0]);

        result += (result0 + blendY * (result1 - result0)) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }

    public double Perlin1D(double pos)
    {
      double result = 0.0;
      double ampAcc = 0.0;
      double curAmp = amp;

      int p = (int)(pos * res) % res;

      for(int i = minOctave; i < maxOctave; i++)
      {
        int pitch = res >> i;
        int sample0 = (p / pitch) * pitch;
        int sample1 = (sample0 + pitch) % res;

        double blend = (pos - sample0) / pitch;

        result += (_seed[sample0] + blend * (_seed[sample1] - _seed[sample0])) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }
  }

  public class VoronoiTexture : Texture3D
  {
    private Vector3d[,,] seed;
    private readonly int res;

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

    public override long GetTexel (Vector3d texCoord, double[] color)
    {
      double value = GetDistance3D(texCoord);
      for(int i = 0; i < color.Length; i++)
      {
        color[i] = value;
      }
      return 2L;
    }

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
