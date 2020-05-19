using OpenTK;
using Rendering;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Utilities;

namespace JaroslavNejedly
{
  /// <summary>
  /// Advanced background class. Simulates sky color, based on provided argumnets.
  /// </summary>
  [Serializable]
  public class AdvancedBackground : IBackground, ILightSource
  {
    public double[] GroundColor { get; set; }
    public double[] HorizonColor { get; set; }
    public double[] SkyColor { get; set; }

    public double[] SunIntensity { get; set; }
    public double SunSmallness { get; set; }
    private Vector3d _sunDir;
    public Vector3d SunDir
    {
      get
      {
        return _sunDir;
      }
      set
      {
        _sunDir = value.Normalized();
      }
    }

    Vector3d _upVector = Vector3d.UnitZ;

    public AdvancedBackground(in double[] skyColor, in double[] horizonColor)
    {
      if (skyColor.Length != horizonColor.Length)
      {
        throw new ArgumentException("Number of color bands disagree.");
      }

      GroundColor = new double[] { 0.12, 0.08, 0.05 };
      HorizonColor = (double[])horizonColor.Clone();
      SkyColor = (double[])skyColor.Clone();

      SunIntensity = new double[skyColor.Length];
      for (int i = 0; i < SunIntensity.Length; i++)
      {
        SunIntensity[i] = 1.2;
      }

      SunDir = new Vector3d(1, 1, 1);
      SunSmallness = 400;
    }

    public AdvancedBackground(in double[] skyColor, in double[] horizonColor, Vector3d upVector) : this(skyColor, horizonColor)
    {
      _upVector = upVector;
    }

    public Vector3d? position { get; set; }

    public virtual long GetColor(Vector3d p1, double[] color)
    {
      p1.NormalizeFast();
      double dot = Vector3d.Dot(p1, _upVector);
      if (dot < -0.1)
      {
        Util.ColorCopy(GroundColor, color);
      }
      else if (dot < 0)
      {
        dot *= -10;
        Util.ColorCopy(Lerp(HorizonColor, GroundColor, dot), color);
      }
      else
      {
        double param = 1 - dot;
        param *= param;
        param = 1 - param;
        Util.ColorCopy(Lerp(HorizonColor, SkyColor, param), color);
      }

      //Sun
      double sunParam = Vector3d.Dot(SunDir, p1);
      if (sunParam < 0)
      {
        return 1L;
      }

      sunParam = Math.Pow(sunParam, SunSmallness);

      Util.ColorAdd(SunIntensity, sunParam, color);

      return 1L;
    }

    public double[] GetIntensity(Intersection intersection, out Vector3d dir)
    {
      dir = SunDir;
      return SunIntensity;
    }

    private double[] Lerp(double[] c0, double[] c1, double t)
    {
      double[] res = new double[c0.Length];
      for (int i = 0; i < c0.Length; i++)
      {
        res[i] = c0[i] + t * (c1[i] - c0[i]);
      }

      return res;
    }
  }
}
