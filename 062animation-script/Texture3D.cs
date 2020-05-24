using MathSupport;
using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      Random rj = new Random((int)randSeed);
      _seed = new double[res * res];

      for(int i = 0; i < res * res; i++)
      {
        _seed[i] = rj.NextDouble();
      }

      maxOctaves = (int)Math.Log(res, 2);
      if (maxOctave > maxOctaves)
        maxOctave = maxOctaves;
    }

    public override long GetTexel (Vector3d texCoord, double[] color)
    {
      double value =  Perlin2D(texCoord.X / 5, texCoord.Y / 5, _seed);
      for(int i = 0; i < color.Length; i++)
      {
        color[i] = value;
      }

      return (long)value;
    }

    private double Perlin2D(double x, double y, double[] seed)
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

        double result0 = seed[sampleY0 * res + sampleX0] + blendX * (seed[sampleY0 * res + sampleX1] - seed[sampleY0 * res + sampleX0]);
        double result1 = seed[sampleY1 * res + sampleX0] + blendX * (seed[sampleY1 * res + sampleX1] - seed[sampleY1 * res + sampleX0]);

        result += (result0 + blendY * (result1 - result0)) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }

    private double Perlin1D(double pos, double[] seed, int octaves)
    {
      double result = 0.0;
      double ampAcc = 0.0;
      double curAmp = amp;

      if (octaves > maxOctaves)
        octaves = maxOctaves;

      int p = (int)(pos * res) % res;

      for(int i = 0; i < octaves; i++)
      {
        int pitch = res >> i;
        int sample0 = (p / pitch) * pitch;
        int sample1 = (sample0 + pitch) % res;

        double blend = (pos - sample0) / pitch;

        result += (seed[sample0] + blend * (seed[sample1] - seed[sample0])) * curAmp;
        ampAcc += curAmp;
        curAmp /= bias;
      }

      return result / ampAcc;
    }
  }
}
