using OpenTK;
using Rendering;
using System;
using System.Runtime.InteropServices;
using Utilities;

namespace JaroslavNejedly
{
  /// <summary>
  /// Advanced background class. Simulates sky color, based on provided argumnets.
  /// </summary>
  [Serializable]
  public class AdvancedBackground : IBackground, ILightSource
  {
    double[] _groundColor;
    double[] _horizonColor;
    double[] _skyColor;

    double[] _sunIntensity;

    Vector3d _sunDir = (new Vector3d(1, 1, 1)).Normalized();
    Vector3d _upVector = Vector3d.UnitZ;

    public AdvancedBackground(in double[] skyColor, in double[] horizonColor, in double[] groundColor)
    {
      if (skyColor.Length != horizonColor.Length || horizonColor.Length != groundColor.Length)
      {
        throw new ArgumentException("Color channels disagree.");
      }

      _groundColor = (double[])groundColor.Clone();
      _horizonColor = (double[])horizonColor.Clone();
      _skyColor = (double[])skyColor.Clone();

      _sunIntensity = new double[groundColor.Length];
      for (int i = 0; i < _sunIntensity.Length; i++)
      {
        _sunIntensity[i] = 1.2;
      }
    }

    public AdvancedBackground(in double[] skyColor, in double[] horizonColor, in double[] groundColor, Vector3d upVector) : this(skyColor, horizonColor, groundColor)
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
        Util.ColorCopy(_groundColor, color);
      }
      else if (dot < 0)
      {
        dot *= -10;
        Util.ColorCopy(Lerp(_horizonColor, _groundColor, dot), color);
      }
      else
      {
        double param = 1 - dot;
        param *= param;
        param = 1 - param;
        Util.ColorCopy(Lerp(_horizonColor, _skyColor, param), color);
      }

      return 1L;
    }

    public double[] GetIntensity(Intersection intersection, out Vector3d dir)
    {
      dir = _sunDir;
      return _sunIntensity;
    }

    private double[] Lerp(double[] c0, double[] c1, double param)
    {
      double[] res = new double[c0.Length];
      for (int i = 0; i < c0.Length; i++)
      {
        res[i] = c0[i] + param * (c1[i] - c0[i]);
      }

      return res;
    }
  }
}
