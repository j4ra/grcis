using OpenTK;
using Rendering;
using System;
using Utilities;

namespace JaroslavNejedly
{
  /// <summary>
  /// Advanced background class. Simulates sky color, based on provided argumnets.
  /// </summary>
  [Serializable]
  public class AdvancedBackground : IBackground
  {
    //TODO: Might wnat to expose this var to the API.
    private const double groundCutoff = -0.2;

    private Vector3d _upVector = Vector3d.UnitY;

    /// <summary>
    /// Sun light source of this sky. Add this to your scene light source collection for directional light.
    /// </summary>
    public SunLight Sun { get; private set; }

    /// <summary>
    /// Current setting used to draw the advanced background.
    /// </summary>
    public AdvancedBackgroundPreset CurrentPreset;

    #region Constructors
    /// <summary>
    /// Creates an instance of <see cref="AdvancedBackground"/> and sets the current preset as <paramref name="preset"/>
    /// <para/><i>Note: Default up vector is <see cref="Vector3d.UnitY"/></i>
    /// </summary>
    /// <param name="preset">Settings preset to be used by this instance</param>
    public AdvancedBackground(AdvancedBackgroundPreset preset)
    {
      CurrentPreset = preset;
      Sun = new SunLight(this);
    }

    /// <summary>
    /// Creates an instance of <see cref="AdvancedBackground"/> and sets the current preset as <paramref name="preset"/>.
    /// <para/>Also changes the current up vector according to <paramref name="upVector"/>.
    /// This is important beacause the "top of the sky" is rendered in the direction up vector.
    /// </summary>
    /// <param name="preset"></param>
    /// <param name="upVector"></param>
    public AdvancedBackground(AdvancedBackgroundPreset preset, Vector3d upVector) : this(preset)
    {
      _upVector = upVector.Normalized();
    }

    /// <summary>
    /// Creates an instance of <see cref="AdvancedBackground"/>. The settings preset used is the default one (<see cref="AdvancedBackgroundPreset.Default"/>).
    /// <para/>Changes the current up vector according to <paramref name="upVector"/>.
    /// This is important beacause the "top of the sky" is rendered in the direction up vector.
    /// </summary>
    /// <param name="upVector"></param>
    public AdvancedBackground(Vector3d upVector) : this(AdvancedBackgroundPreset.Default, upVector)
    { }

    /// <summary>
    /// Creates an instance of <see cref="AdvancedBackground"/>. The settings preset used is the default one (<see cref="AdvancedBackgroundPreset.Default"/>).
    /// <para/><i>Note: Default up vector is <see cref="Vector3d.UnitY"/></i>
    /// </summary>
    public AdvancedBackground() : this(AdvancedBackgroundPreset.Default)
    { }
    #endregion

    #region IBackground
    public virtual long GetColor(Vector3d p1, double[] color)
    {
      //Create basic sky gradient based on current sun direction, ray direction and up vector.
      p1.NormalizeFast();

      double rayElevParam = Vector3d.Dot(p1, _upVector);
      double sunElevParam = Vector3d.Dot(CurrentPreset.SunDirection, _upVector);

      //Compute night color
      double nightCoeff = 0.0;
      double[] nightColor = CalcNightColor(sunElevParam, out nightCoeff, p1);

      double[] horizonColor = Lerp(CurrentPreset.HorizonColor, CurrentPreset.InScatterColor, nightCoeff);

      if (rayElevParam < groundCutoff)
      {
        //use only ground color
        Util.ColorCopy(CurrentPreset.GroundColor, color);
      }
      else if (rayElevParam < 0)
      {
        //ease in the ground color:
        double groundParam = rayElevParam / groundCutoff;

        if (nightCoeff > 0.0)
          Util.ColorCopy(Lerp(Lerp(horizonColor, CurrentPreset.GroundColor, groundParam), nightColor, nightCoeff), color);
        else
          Util.ColorCopy(Lerp(horizonColor, CurrentPreset.GroundColor, groundParam), color);
      }
      else
      {
        //Blend horizon and out scatter colors based on "elevation"
        double param = 1 - rayElevParam;
        param *= param;

        //Blend in night color
        if (nightCoeff > 0.0)
        {
          Util.ColorCopy(Lerp(Lerp(CurrentPreset.OutScatterColor, horizonColor, param), nightColor, nightCoeff), color);
        }
        else
        {
          Util.ColorCopy(Lerp(CurrentPreset.OutScatterColor, horizonColor, param), color);
        }

      }

      if (sunElevParam > groundCutoff)
      {
        //Add sun
        double sunRayParam = Vector3d.Dot(CurrentPreset.SunDirection, p1);

        if (sunRayParam > 0)
        {
          //we are looking in the direction of sun
          //shirnk the sun size
          double sunParam = Math.Pow(sunRayParam, CurrentPreset.SunSmallness);

          //Tint the sun color
          double[] sunColor = Mul(CurrentPreset.SunIntensity, CurrentPreset.SunTint);
          //Additively add sun into the sky
          Util.ColorAdd(sunColor, sunParam, color);
        }
      }

      return 1L;
    }
    #endregion

    #region Private methods

    private double[] CalcNightColor(double sunElev, out double nightCoeff, in Vector3d p1)
    {
      nightCoeff = 0.0;
      double[] nightColor = CurrentPreset.NightColor;
      if (sunElev < -2 * groundCutoff)
      {
        //Calculate this only at night time.
        if (CurrentPreset.NightBackground != null)
        {
          nightColor = new double[CurrentPreset.NightColor.Length];
          CurrentPreset.NightBackground.GetColor(p1, nightColor);
        }
        nightCoeff = sunElev < groundCutoff ? 1.0 : (sunElev + 2 * groundCutoff) / (3 * groundCutoff);
      }

      return nightColor;
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

    private double[] Mul(double[] c0, double[] c1)
    {
      double[] res = new double[c0.Length];
      for (int i = 0; i < c0.Length; i++)
      {
        res[i] = c0[i] * c1[i];
      }

      return res;
    }

    private double[] Inverse(double[] c)
    {
      double[] res = new double[c.Length];
      for (int i = 0; i < c.Length; i++)
      {
        res[i] = 1 - c[i];
      }

      return res;
    }

    #endregion
  }

  /// <summary>
  /// Directional light without any falloff. Used by <see cref="AdvancedBackground"/>.
  /// </summary>
  public class SunLight : ILightSource
  {
    private AdvancedBackgroundPreset? _preset = null;
    private AdvancedBackground _background = null;

    #region Constructors

    public SunLight()
    {
      _preset = AdvancedBackgroundPreset.Default;
    }

    public SunLight(AdvancedBackgroundPreset preset)
    {
      _preset = preset;
    }

    public SunLight(AdvancedBackground background)
    {
      _background = background;
    }

    #endregion

    #region ILigtSource
    public Vector3d? position { get; set; }

    public double[] GetIntensity(Intersection intersection, out Vector3d dir)
    {
      if (_preset == null)
      {
        dir = _background.CurrentPreset.SunDirection;
        return Util.ColorClone(_background.CurrentPreset.SunIntensity, _background.CurrentPreset.SunIntensityMultiplier);
      }

      dir = (Vector3d)_preset?.SunDirection;
      return Util.ColorClone(_preset?.SunIntensity, (double)_preset?.SunIntensityMultiplier);
    }
    #endregion
  }

  /// <summary>
  /// Struct holding preset settings for <see cref="AdvancedBackground"/>.
  /// </summary>
  public struct AdvancedBackgroundPreset
  {
    /// <summary>
    /// Default preset. Basic "realistic" sky.
    /// </summary>
    public static readonly AdvancedBackgroundPreset Default = new AdvancedBackgroundPreset
    {
      OutScatterColor = new double[] { 0.1, 0.2, 0.7 },
      GroundColor = new double[] { 0.08, 0.05, 0.03 },
      HorizonColor = new double[] { 0.62, 0.74, 0.89 },
      InScatterColor = new double[] { 0.9, 0.4, 0.3 },
      SunTint = new double[] { 1.0, 0.9, 0.7 },
      SunSmallness = 400,
      SunIntensity = new double[] { 2.4, 2.3, 2.3 },
      SunDirection = new Vector3d(1.0, 1.0, 1.0),
      NightColor = new double[] { 0.01, 0.01, 0.02 },
      SunIntensityMultiplier = 1.0
    };

    /// <summary>
    /// Color that is scatter away from incoming light. This is basicaly the sky color.
    /// </summary>
    public double[] OutScatterColor;

    /// <summary>
    /// Color that remains in incoming light as it passes through the atmosphere. This is basicaly the color of sky during sunset/sunrise.
    /// </summary>
    public double[] InScatterColor;

    /// <summary>
    /// Color on the horizon. Used to create gradient in the sky.
    /// </summary>
    public double[] HorizonColor;

    /// <summary>
    /// Color of "ground". This color should not be visible in normal scenes.
    /// </summary>
    public double[] GroundColor;

    /// <summary>
    /// Color of the sky during night.
    /// </summary>
    public double[] NightColor;

    /// <summary>
    /// Optional. If set, uses this to shade the sky during night time. <see cref="JosefPelikan.StarBackground"/>
    /// </summary>
    public IBackground NightBackground;

    /// <summary>
    /// Color of the sun disk on the sky. <para /><i>Note: This is added additevely into the sky. If you set this to black, no sun will be visible.</i>
    /// </summary>
    public double[] SunTint;

    /// <summary>
    /// The bigger this value is, the smaller the sun will be.
    /// </summary>
    public double SunSmallness;

    /// <summary>
    /// Intensity of the sun light source.
    /// </summary>
    public double[] SunIntensity;

    /// <summary>
    /// Changes the intensity of the sun light source.
    /// </summary>
    public double SunIntensityMultiplier;

    /// <summary>
    /// Direction of sun. This positions the sun onto the sky.
    /// </summary>
    public Vector3d SunDirection { get => _sunDir; set => _sunDir = value.Normalized(); }
    private Vector3d _sunDir;
  }
}
