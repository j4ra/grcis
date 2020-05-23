# Extension: Advanced background

![example0](imgs/img0.png)

### Author: Jaroslav Nejedlý

### Category: Background

### Namespace: JaroslavNejedly

### Class Name: AdvancedBackground : IBackground

### ITimeDependent: Not yet... 😉

### Source file: AdvancedBackground.cs

This extension provides you with a nice looking sky (without clouds). The advanced background also provides you with Sun light source. The `SunLight : ILightSource` is just simple implementation of directional light. I tried to make all paramteres customizable. And I also provided default values that IMHO look good (or at least I tried to make them look good). The parameters are stored in struct `AdvancedBackgroundPreset`.

The list of customizable parameters:
 * OutScatterColor - This is the color of the sky directly above. Type: `double[]`
 * GroundColor - This is color directly below. In "normal" scenes this shouldn't be visible. Type: `double[]`
 * HorizonColor - This is the color on the horizon, but only when the sun is high enough. If the sun is low above horizon, this color blends into InScatterColor. Type: `double[]`
 * InScatterColor - This is the color of sunrise/sunset. Type: `double[]`
 * SunTint - This modifies the color of the sun that is show on the sky (it does not interact with the underlaying light source). Type: `double[]`
 * SunSmallness - This modifies the size of the sun on the sky. The higher the value, the smaller the sun will be. Type: `double`
 * SunIntensity - This sets the intensity of the sun and it also contros the underlaying light source. Type: `double[]`
 * SunDirection - This controls the position of the sun on the sky. Type: `Vector3d`
 * NightColor - This is the color of the sky when the sun is down (in the night).  Type: `double[]`
 * SunIntensityMultiplier - This modifies the intensity of the sun light. Tou might want to lower the value when the sun is down. Type: `double`
 * NightBackground - *Optional* - This is the reference to `IBackground` object that is used to render the sky during night. You can use this to add [star background.](../JosefPelikan-StarBackground/README.md) Type: `IBackground`

### Examples &amp; sample scripts:

Examples will be provided when implementation is finalized.

```

```

#### Sample scene script: DemoScene.cs

## Issues and things to be aware of:

The advanced background adds quite a lot of light into the scene. Be aware of that. This might cause some lighting issues. The amount of light provided by the advanced background is substaintial, even if you do not add provided sun light source into the scene.

Also please beware what is the up direction in your scene. Default value is (0, 1, 0).

## Imges &amp; videos

#### Sky background.

![example1](imgs/img1.png)

#### With the sun!

![example0](imgs/img0.png)

#### Redish tones of sunrise.

![example2](imgs/img_early_morning.png)

#### Morning.

![example3](imgs/img_morning.png)

#### Night.

![example4](imgs/img_night.png)

#### Can incorporate [star background.](../JosefPelikan-StarBackground/README.md)
![example5](imgs/img_morning_with_stars.png)