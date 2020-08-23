using DavidSosvald_MichalTopfer;
using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Utilities;

namespace JaroslavNejedly
{
  [Serializable]
  public class AttenuatedPointLight : PointLightSource
  {
    protected double attenuation = 1.0;

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

  [Serializable]
  public class SpotLight : ILightSource
  {
    protected Vector3d direction;
    protected double[] intensity;
    protected double angle { get => _angleRad; set => _angleRad = Math.Cos(Math.Max(value, 0) / 180.0 * Math.PI); }

    private double _angleRad;
    
    public SpotLight (Vector3d pos, double[] intensity, Vector3d direction, double angle = 20.0)
    {
      position = pos;
      this.intensity = intensity;
      this.direction = direction.Normalized();
      this.angle = angle;
    }

    public SpotLight (Vector3d pos, double intensity, Vector3d direction, double angle = 20.0)
    {
      position = pos;
      this.intensity = new double[] { intensity, intensity, intensity };
      this.direction = direction.Normalized();
      this.angle = angle;
    }

    public Vector3d? position { get; set; }

    public virtual double[] GetIntensity (Intersection intersection, out Vector3d dir)
    {
      dir = (Vector3d)position - intersection.CoordWorld;

      if (Vector3d.Dot(dir, intersection.Normal) <= 0.0)
        return null;

      if (Vector3d.Dot(dir.Normalized(), direction) < angle)
        return null;

      return intensity;
    }
  }

  [Serializable]
  public class AnimatedSpotLight : SpotLight, ITimeDependent
  {
    public double Start { get; set; }
    public double End { get; set; }
    public double Time { get => time; set => setTime(value); }

    double time;

    string _posAnimName;
    string _dirAnimName;
    string _intAnimName;
    string _angAnimName;

    Vector3d _defaultPos;
    Vector3d _defaultDir;
    double[] _defaultInt;
    double _defaultAngle;

    public AnimatedSpotLight (Animator animator, string posName, string dirName, string angName, string intName)
                        : this(animator, posName, dirName, angName, intName, Vector3d.Zero, Vector3d.UnitY, new double[] { 1.0, 1.0, 1.0 }, 20)
    { }

    public AnimatedSpotLight (Animator animator, string posName, string dirName, string angName, string intName,
                              Vector3d defaultPos, Vector3d defaultDir, double[] defaultInt, double defaultAngle)
                            : base(defaultPos, defaultInt, defaultDir, defaultAngle)
    {
      _posAnimName = posName;
      _dirAnimName = dirName;
      _intAnimName = intName;
      _angAnimName = angName;

      _defaultPos = defaultPos;
      _defaultInt = defaultInt;
      _defaultDir = defaultDir;
      _defaultAngle = defaultAngle;

      animator?.RegisterParams(GetParams());
    }

    public IEnumerable<Animator.Parameter> GetParams ()
    {
      List<Animator.Parameter> p = new List<Animator.Parameter>();

      if (_posAnimName != null)
        p.Add(new Animator.Parameter(_posAnimName, Animator.Parsers.ParseVector3, Animator.Interpolators.Catmull_Rom, true));
      if (_dirAnimName != null)
        p.Add(new Animator.Parameter(_dirAnimName, Animator.Parsers.ParseVector3, Animator.Interpolators.Catmull_Rom, true));
      if (_intAnimName != null)
        p.Add(new Animator.Parameter(_intAnimName, Animator.Parsers.ParseDoubleArray, Animator.Interpolators.Catmull_Rom, true));
      if (_angAnimName != null)
        p.Add(new Animator.Parameter(_angAnimName, Animator.Parsers.ParseDouble, Animator.Interpolators.Catmull_Rom, true));

      return p;
    }
    protected void setTime (double time)
    {
      this.time = time;
      if (MT.scene == null)
        return;
      Dictionary<string, object> p = ((Animator)MT.scene.Animator).getParams(time);
      ApplyParams(p);
    }

    public void ApplyParams (Dictionary<string, object> p)
    {
      intensity = _intAnimName != null ? (double[])p[_intAnimName] : _defaultInt;
      position = _posAnimName != null ? (Vector3d)p[_posAnimName] : _defaultPos;
      direction = _dirAnimName != null ? (Vector3d)p[_dirAnimName] : _defaultDir;
      angle = _angAnimName != null ? (double)p[_angAnimName] : _defaultAngle;
    }

    public object Clone ()
    {
      AnimatedSpotLight asl = new AnimatedSpotLight(null, _posAnimName, _dirAnimName, _angAnimName, _intAnimName,
                                                          _defaultPos, _defaultDir, _defaultInt, _defaultAngle);
      asl.Start = Start;
      asl.End = End;
      asl.Time = time;

      return asl;
    }

#if DEBUG
    private static volatile int nextSerial = 0;
    private readonly int serial = nextSerial++;
    public int getSerial () => serial;
#endif
  }

  [Serializable]
  public class AnimatedAttenuatedPointLight : AttenuatedPointLight, ITimeDependent
  {
    public double Start { get; set; }
    public double End { get; set; }
    public double Time { get => time; set => setTime(value); }

    double time;

    string _posAnimName;
    string _intAnimName;
    string _attAnimName;

    Vector3d _defaultPos;
    double[] _defaultInt;
    double _defaultAtt;

    public AnimatedAttenuatedPointLight (Animator animator, string posName, string attName, string intName)
                        : this(animator, posName, attName, intName, Vector3d.Zero, new double[] { 1.0, 1.0, 1.0 }, 20)
    { }

    public AnimatedAttenuatedPointLight (Animator animator, string posName, string attName, string intName,
                              Vector3d defaultPos, double[] defaultInt, double defaultAtt)
                            : base(defaultPos, defaultInt, defaultAtt)
    {
      _posAnimName = posName;
      _intAnimName = intName;
      _attAnimName = attName;

      _defaultPos = defaultPos;
      _defaultInt = defaultInt;
      _defaultAtt = defaultAtt;

      animator?.RegisterParams(GetParams());
    }

    public IEnumerable<Animator.Parameter> GetParams ()
    {
      List<Animator.Parameter> p = new List<Animator.Parameter>();

      if (_posAnimName != null)
        p.Add(new Animator.Parameter(_posAnimName, Animator.Parsers.ParseVector3, Animator.Interpolators.Catmull_Rom, true));
      if (_intAnimName != null)
        p.Add(new Animator.Parameter(_intAnimName, Animator.Parsers.ParseDoubleArray, Animator.Interpolators.Catmull_Rom, true));
      if (_attAnimName != null)
        p.Add(new Animator.Parameter(_attAnimName, Animator.Parsers.ParseDouble, Animator.Interpolators.Catmull_Rom, true));

      return p;
    }
    protected void setTime (double time)
    {
      this.time = time;
      if (MT.scene == null)
        return;
      Dictionary<string, object> p = ((Animator)MT.scene.Animator).getParams(time);
      ApplyParams(p);
    }

    public void ApplyParams (Dictionary<string, object> p)
    {
      intensity = _intAnimName != null ? (double[])p[_intAnimName] : _defaultInt;
      position = _posAnimName != null ? (Vector3d)p[_posAnimName] : _defaultPos;
      attenuation = _attAnimName != null ? (double)p[_attAnimName] : _defaultAtt;
    }

    public object Clone ()
    {
      AnimatedAttenuatedPointLight asl = new AnimatedAttenuatedPointLight(null, _posAnimName, _attAnimName, _intAnimName,
                                                                                _defaultPos, _defaultInt, _defaultAtt);
      asl.Start = Start;
      asl.End = End;
      asl.Time = time;

      return asl;
    }

#if DEBUG
    private static volatile int nextSerial = 0;
    private readonly int serial = nextSerial++;
    public int getSerial () => serial;
#endif
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
          Intersection exit = intersections.FirstOrDefault(x => x.Solid != null && x.Enter == false);

          if(exit == null)
          {
            rr = null;
            return 0;
          }

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
      extinction = 1.0;

      double scatteringWight = 1.0 / steps * (10.0) * 0.1;
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
