using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Utilities;

namespace JaroslavNejedly
{
  [Serializable]
  public class VolumetricClouds
  {
    IRayScene _scene;
    ISolid _volume;

    double _coverage = 0.5;

    double _stepSize = 6.0 / 7.0;
    double _lightStepSize = 12.0 / 7.0;

    double extinctionFactor = 1.0;
    double scatteringFactor = 1.0;

    int maxDepth = 36;
    int maxLightDepth = 1;

    PerlinTexture pTex;
    VoronoiTexture vTex;

    public VolumetricClouds (IRayScene scene, ISolid volume)
    {
      _volume = volume;
      _scene = scene;
      pTex = new PerlinTexture(0, 256, 1, 1.4);
      vTex = new VoronoiTexture(12, 0);

      rf = (Intersection i, Vector3d dir, double importance, out RayRecursion rr) =>
      {
        //TODO: compute intersections once more. Find the length in volume, set step sizes and step counts.
        if (i.Enter)
        {
          double extiction = 1.0;
          double[] scattering = new double[] {0.0, 0.0, 0.0 };

          Raymarch(i.CoordWorld, dir.Normalized(), ref extiction, ref scattering);

          var rrrc = new RayRecursion.RayContribution(i, dir, importance);
          rrrc.coefficient = new double[] { extiction };

          rr = new RayRecursion(
          scattering,
          rrrc
          );
          //rr.Rays = new List<RayRecursion.RayContribution>();
          return 144L;
        }
        rr = new RayRecursion(new double[] { 0.0, 0.0, 0.0 }, new RayRecursion.RayContribution(i, dir, importance));
        return 0L;
      };
    }

    public double SampleDens (Vector3d pos, double coverage = 0.5)
    {
      //TODO: erode the edges
      //TODO: soften transition from the edges
      coverage = Math.Min(0.99, 1 - coverage);
      double voronoi = 1.0 - vTex.GetDistance3D(pos / 10);
      double voronoiSmall = 1.0 - vTex.GetDistance3D(pos / 5);

      double perlin = pTex.Perlin3D(pos.X / 10, pos.Y / 10, pos.Z / 10, 1, 8);
      double perlinBig = pTex.Perlin3D(pos.X / 250, pos.Z / 250, pos.Y / 125, 2, 8);
      double perlinSmall = 1.0 - pTex.Perlin3D(pos.Z / 5, pos.X / 5, pos.Y / 5, 1, 8);

      double baseShape = Math.Max(0, 1 / (1 - coverage) * (perlinBig - coverage));
      double worley = Math.Max(0, voronoi + voronoiSmall - 1.0);

      return worley * baseShape;
    }

    public void RaymarchLight (Vector3d p0, Vector3d p1, ILightSource ls, ref double extinction, ref double[] scattering, int depth = 0)
    {
      LinkedList<Intersection> intersections = _scene.Intersectable.Intersect(p0, p1);

      if (depth >= maxLightDepth)
        return;

      if (!intersections.First.Value.Enter && intersections.First.Value.T < _lightStepSize)// && intersections.First.Value.Solid == _volume)
      {
        //we reached end of volume

        //return
        return;
      }

      Vector3d newP0 = p0 + p1.Normalized() * _lightStepSize;
      double dens = SampleDens(newP0, _coverage);

      double scatteringCoef = scatteringFactor * dens;
      double extinctionCoef = extinctionFactor * dens;

      //TODO: accumulate alpha
      //TODO: exit early when alpha is 1.0
      extinction *= Math.Exp(-extinctionCoef * _stepSize);

      Intersection i = new Intersection(_volume);
      i.CoordWorld = newP0;
      i.Normal = p1.Normalized();

      //TODO: better lighting and shading model
      double[] lightContrib = ls.GetIntensity(i, out Vector3d _);

      Util.ColorAdd(lightContrib, scatteringCoef * extinction * _lightStepSize, scattering);

      RaymarchLight(newP0, p1, ls, ref extinction, ref scattering, depth + 1);
    }

    public void Raymarch (Vector3d p0, Vector3d p1, ref double extinction, ref double[] scattering, int depth = 0)
    {
      LinkedList<Intersection> intersections = _scene.Intersectable.Intersect(p0, p1);

      Intersection intersection = Intersection.FirstIntersection(intersections, ref p1);

      if (intersection == null)
        return;

      if (depth >= maxDepth)
        return;

      if (!intersection.Enter && intersection.T < _stepSize)
      {
        //we reached end of volume

        //return
        return;
      }
      if (intersection.T < _stepSize)
      {
        //We hit other object

        //return
        return;
      }

      //Direct sample
      Vector3d newP0 = p0 + p1.Normalized() * _stepSize;
      double density = SampleDens(newP0, _coverage);

      double scatteringCoef = scatteringFactor * density;
      double extinctionCoef = extinctionFactor * density;

      extinction *= Math.Exp(-extinctionCoef * _stepSize);

      //Lightsources
      foreach (ILightSource ls in _scene.Sources)
      {
        Vector3d dir;
        Intersection i = new Intersection(_volume);
        i.Normal = p1.Normalized();
        i.CoordWorld = newP0;
        double[] lightContrib = ls.GetIntensity(i, out dir);
        
        if (lightContrib == null)
          continue;
        if (dir.LengthSquared < 0.01)
        {
          Util.ColorAdd(lightContrib, scatteringCoef * extinction * _stepSize, scattering);
          continue;
        }
        //TODO: use raymarch light
        dir.Normalize();

        Util.ColorAdd(lightContrib, scatteringCoef * extinction * _stepSize, scattering);
        //double lightExtinction = extinction;
        //RaymarchLight(newP0, dir, ls, ref lightExtinction, ref scattering);
      }

      Raymarch(newP0, p1, ref extinction, ref scattering, depth + 1);
    }

    public readonly RecursionFunction rf;
  }
}
