# Extension: Advanced background

![example0](imgs/img0.png)

### Author: Jaroslav Nejedlý

### Category: Background

### Namespace: JaroslavNejedly

### Class Name: 
 #### AdvancedBackground : IBackground
 #### AnimatedAdvancedBackground : AdvancedBackground, ITimeDependent

### ITimeDependent: Yes

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
 * SunIntensityMultiplier - This modifies the intensity of the sun light. You might want to lower the value when the sun is down. The value of thos variable is always clamped between 0 and 1. Type: `double`
 * NightBackground - *Optional* - This is the reference to `IBackground` object that is used to render the sky during night. You can use this to add [star background.](../JosefPelikan-StarBackground/README.md) Type: `IBackground`

### Examples &amp; sample scripts:

#### Static version:

To use the advanced background simply create it: `var backround = new AdvancedBackground();`. If you don't provide any parameters into the constructor, the advanced background will use the default settings. But don't worry you can change them later by modifying: `background.CurrentPreset`. Then don't forget to add it to the scene: `scene.Background = background;`. Then you can add the sun into scene light source collection: `scene.Sources.Add(background.Sun);`, but this step is optional. If you don't want to add the sun, you might want to disable the sun rendering: `background.CurrentPreset.SunTint = new double[] {0, 0, 0};`.

Basic example:

```C#
//Don't forget to use correct namespaces
using JaroslavNejedly;

/*
 * SOME CODE WHERE YOU SET UP YOUR SCENE
 */

//Let's use the default preset as a starting point. You can change the parameters later.
var background = new AdvancedBackground();
//Add the backround into the scene.
scene.Background = background;
//Add the sun into the scene
scene.Sources.Add(background.Sun);

//You can change any parameters by modifying CurrentPreset property.
//Let's put the sun just above the horizon ¯\_(ツ)_/¯
background.CurrentPreset.SunDirection = new Vector3d(0, 0.1, 1.0);
```
See [DemoScene.cs](DemoScene.cs) for exact example.

If you wish to incorporate [the star background](../JosefPelikan-StarBackground/README.md), you can do so by modifying `background.CurrentPreset.NightBackground`:
```C#
using JosefPelikan; //For StarBackground
/*
 * SAME CODE AS IN EXAMPLE ABOVE
 */

background.CurrentPreset.NightBackground = new StarBackground(advBackground.CurrentPreset.NightColor);
```
Uncomment last line in [DemoScene.cs](DemoScene.cs) to see the effect.

For custom preset example you can look at [DemoSceneCustomPreset.cs.](DemoSceneCustomPreset.cs)

If you are using different up vector in your scene you have to set it correctly in the advanced background. The following line changes the default up vector to be in the Z-direction `background = new AdvancedBackground(Vector3d.UnitZ);` 

##### Sample scene script: [DemoScene.cs](DemoScene.cs)

##### Sample scene script with custom preset: [DemoSceneCustomPreset.cs](DemoSceneCustomPreset.cs)

#### Animations:

To created animated background you need to create AnimatedAdvancedBackground object: `var background = new AnimatedAdvancedBackground()`. You can also pass `Vector3d` that represent scene up direction. The animation simpy interpolates between two presets. So in the scene script you need to create two preset objects: `var startPrest = AdvancedBackgroundPreset.Default;` and `var endPreset = new AdvancedBackgroundPreset.Default;`. Then you can change any property to create interesting transition between the two states. Next you assign these presets into your background object: `background.StartPreset = startPreset;` and `background.EndPreset = endPreset;`. Do not forget to set start and end of your animation. For better interpolation of sun direction you can assign your own function: `background.SunDirectionAnimator = (t) => Vector3d.Lerp(startPreset.SunDirection, endPreset.SunDirection, t);` This function is the default one but you can use any function (`Func<double, Vector3d>`), the parameter that comes into the function is normalized between 0.0 and 1.0 and represents time of the animation.

To setup the sun light object you need to create it and then pass the same start and end presets:

```C#
var sun = new AnimatedSunLight();
sun.StartPreset = startPreset;
sun.EndPrest = endPreset;
```

You can also optionaly use the sun direction interpolation function. **It is important to keep the presets and interpolation function consistent between background and light objects.** The complete example code looks like this:

```C#
using JaroslavNejedly;
using JosefPelikan; //For StarBackground

/*
 * YOUR SCENE SETUP CODE
 */
//background:
//First we create start and end presets. We use the default preset as a default value.
var startPreset = AdvancedBackgroundPreset.Default;
var endPreset = AdvancedBackgroundPreset.Default;

//Next we modify the preset to create interesting transition
startPreset.SunIntensityMultiplier = -0.365;
startPreset.SunDirection = new Vector3d(0.6, -0.1, 1.0);

endPreset.SunIntensityMultiplier = 2.135;
endPreset.SunDirection = new Vector3d(0.5, 0.6, 0.9);

//We can add star background to one of the presets (The first one, preferably):
startPreset.NightBackground = new JosefPelikan.StarBackground(startPreset.NightColor);

//Now we create the background object
var advBackground = new AnimatedAdvancedBackground();
//We need to apply the presets
advBackground.StartPreset = startPreset;
advBackground.EndPreset = endPreset;
//You can use your own function for interpolating sun direction.
//advBackground.SunDirectionAnimator = (t) => Vector3d.Lerp(startPreset.SunDirection, endPreset.SunDirection, t);
//Do not forget to set start and end of your animation!
advBackground.Start = (double)context[PropertyName.CTX_START_ANIM];
advBackground.End = (double)context[PropertyName.CTX_END_ANIM];
//Apply background and background color.
scene.Background = advBackground;
scene.BackgroundColor = startPreset.NightColor;

//Sun:
//First we have to create animated sun object.
var sun = new AnimatedSunLight();
//Now we apply the starting and ending presets
sun.StartPreset = startPreset;
sun.EndPreset = endPreset;
//You can use your own function for interpolating sun direction.
//sun.SunDirectionAnimator = (t) => Vector3d.Lerp(startPreset.SunDirection, endPreset.SunDirection, t);
//Do not forget to set start and end of your animation!
sun.End = (double)context[PropertyName.CTX_END_ANIM];
sun.Start = (double)context[PropertyName.CTX_START_ANIM];
//Apply the light into the scene.
scene.Sources.Add(sun);
```

##### Sample scene script with animations: [AnimatedDemoScene.cs](AnimatedDemoScene.cs)

## Issues and things to be aware of:

The advanced background adds quite a lot of light into the scene. Be aware of that. This might cause some lighting issues. The amount of light provided by the advanced background is substaintial, even if you do not add provided sun light source into the scene.

Also please beware what is the up direction in your scene. Default value is (0, 1, 0).

## Imges &amp; videos

##### Sky background

![example1](imgs/img1.png)

##### With the sun!

![example0](imgs/img0.png)

##### Redish tones of sunrise

![example2](imgs/img_early_morning.png)

##### Morning

![example3](imgs/img_morning.png)

##### Night

![example4](imgs/img_night.png)

##### It can incorporate [star background](../JosefPelikan-StarBackground/README.md)

![example5](imgs/img_morning_with_stars.png)

##### Custom presets

![example6](imgs/img_custom_preset.png)
