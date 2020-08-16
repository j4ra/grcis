using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace JaroslavNejedly
{
  [Serializable]
  public class AttenuatedPointLight : PointLightSource
  {
    double attenuation = 1.0;

    public AttenuatedPointLight(Vector3d pos, double[] intensity, double att = 1.0) : base(pos, intensity)
    {
      attenuation = att;
    }

    public AttenuatedPointLight (Vector3d pos, double intensity, double att = 1.0) : base(pos, intensity)
    {
      attenuation = att;
    }

    public override double[] GetIntensity (Intersection intersection, out Vector3d dir)
    {
      Vector3d dif = (Vector3d)position - intersection.CoordWorld;
      dir = dif;
      if (Vector3d.Dot(dir, intersection.Normal) <= 0.0)
        return null;

      return intensity.Select(d => d / (1.0 + attenuation * dif.LengthSquared)).ToArray();
    }
  }


  //TODO: animations
  [Serializable]
  public class VolumetricClouds
  {
    const double OneOver4PI = 1.0 / (4.0 * Math.PI);

    IRayScene _scene;
    ISolid _volume;

    int steps = 32;

    public VolumetricClouds (IRayScene scene, ISolid volume)
    {
      _volume = volume;
      _scene = scene;

      Recursion = (Intersection i, Vector3d dir, double importance, out RayRecursion rr) =>
      {
        if (i.Enter)
        {
          LinkedList<Intersection> intersections = scene.Intersectable.Intersect(i.CoordWorld, dir);
          Intersection exit = intersections.First(x => x.Solid != null && x.Solid == _volume && x.Enter == false);

          Vector3d volumeRay = exit.CoordWorld - i.CoordWorld;
          double step = volumeRay.LengthFast / (steps + 1);

          double extiction = 1.0;
          double[] scattering = new double[] {0.0, 0.0, 0.0 };

          Vector3d direction = dir.Normalized();
          long h = Raymarch(i.CoordWorld + direction * Intersection.RAY_EPSILON, direction, ref extiction, ref scattering, step);

          var rrrc = new RayRecursion.RayContribution(i, dir, importance);
          rrrc.coefficient = new double[] { extiction };

          rr = new RayRecursion(
          scattering,
          rrrc
          );

          return 144L + h;
        }

        rr = new RayRecursion(new double[] { 0.0, 0.0, 0.0 }, new RayRecursion.RayContribution(i, dir, importance));
        return 0L;
      };
    }

    private int Raymarch (Vector3d p0, Vector3d p1, ref double extinction, ref double[] scattering, double step)
    {
      extinction = 0.25;

      double scatteringWight = 1.0 / steps * (1.0 - extinction) * 0.1;
      int hash = 0;
      for (int i = 1; i <= steps; i++)
      {
        Vector3d pos = p0 + p1 * step * i;
        foreach (ILightSource ls in _scene.Sources)
        {
          if (ls is AmbientLightSource)
            continue;

          Intersection inte = new Intersection(_volume);
          inte.CoordWorld = pos;
          ls.GetIntensity(inte, out Vector3d dir);
          inte.Normal = dir;
          double[] intensity = ls.GetIntensity(inte, out _);

          if (intensity != null)
          {
            var intersections = _scene.Intersectable.Intersect(pos, dir);
            Intersection si = Intersection.FirstRealIntersection(intersections, ref dir);
            if (si != null && !si.Far(1.0, ref dir))
              continue;

            Util.ColorAdd(intensity, scatteringWight, scattering);
            hash++;
          }
        }
      }
      return hash;
    }

    public RecursionFunction Recursion { get; private set; }
  }
}
