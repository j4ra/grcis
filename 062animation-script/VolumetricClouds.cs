using OpenTK;
using Rendering;
using System;

namespace JaroslavNejedly
{
  [Serializable]
  public class VolumetricClouds
  {
    public static readonly RecursionFunction rf = (Intersection i, Vector3d dir, double importance, out RayRecursion rr) =>
    {
      var rrrc = new RayRecursion.RayContribution(i, dir, importance);
      rrrc.coefficient = new double[] {0.2, 0.2, 0.2};

      rr = new RayRecursion(
        new double[] {0.0, 0.0, 0.0},
        rrrc
        );

      return 144L;
    };
  }
}
