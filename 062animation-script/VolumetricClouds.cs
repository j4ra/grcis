using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace JaroslavNejedly
{
  [Serializable]
  public class VolumetricClouds
  {
    IRayScene _scene;

    double _stepSize = 0.25;

    double extinctionFactor = 1.0;
    double scatteringFactor = 1.0;

    int maxDepth = 25;

    PerlinTexture pTex;
    VoronoiTexture vTex;

    public VolumetricClouds (IRayScene scene)
    {
      _scene = scene;
      pTex = new PerlinTexture(0, 256, 1, 1.4);
      vTex = new VoronoiTexture(12, 0);

      rf = (Intersection i, Vector3d dir, double importance, out RayRecursion rr) =>
      {
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

    public double SampleDens (Vector3d pos)
    {
      double perlin = pTex.Perlin3D(pos.X / 10, pos.Y / 10, pos.Z / 10, 1, 7);
      double voronoi = 1.0 - vTex.GetDistance3D(pos / 10);

      return perlin * voronoi;
    }

    public void RaymarchLight (Vector3d p0, Vector3d p1, ref double dens)
    {
      LinkedList<Intersection> intersections = _scene.Intersectable.Intersect(p0, p1);

      if (!intersections.First.Value.Enter && intersections.First.Value.T < _stepSize)// && intersections.First.Value.Solid == )
      {
        //we reached end of volume

        //return
        return;
      }

      Vector3d newP0 = p0 + p1.Normalized() * _stepSize;
      dens += SampleDens(newP0) * _stepSize;

      RaymarchLight(newP0, p1, ref dens);
    }

    public void Raymarch (Vector3d p0, Vector3d p1, ref double extinciton, ref double[] scattering, int depth = 0)
    {
      LinkedList<Intersection> intersections = _scene.Intersectable.Intersect(p0, p1);

      Intersection intersection = Intersection.FirstIntersection(intersections, ref p1);

      if (intersection == null)
        return;

      if (depth > maxDepth)
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
      double density = SampleDens(newP0);

      double scatteringCoef = scatteringFactor * density;
      double extinctionCoef = extinctionFactor * density;

      extinciton *= Math.Exp(-extinctionCoef * _stepSize);

      //Lightsources
      foreach (ILightSource ls in _scene.Sources)
      {
        Vector3d dir;
        double[] lightContrib = ls.GetIntensity(intersection, out dir);
        if (lightContrib == null)
          continue;
        if (dir.LengthSquared < 0)
        {
          Util.ColorAdd(lightContrib, scatteringCoef * extinctionCoef * _stepSize, scattering);
          continue;
        }
        //TODO: use raymarch light
        dir.Normalize();
        Util.ColorAdd(lightContrib, scatteringCoef * extinctionCoef * _stepSize, scattering);
      }

      Raymarch(newP0, p1, ref extinciton, ref scattering, depth + 1);
    }

    public readonly RecursionFunction rf;
  }
}
