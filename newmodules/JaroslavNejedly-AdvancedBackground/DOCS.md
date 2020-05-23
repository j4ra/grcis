<a name='assembly'></a>
# Advanced Background Module

## Contents

- [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground')
  - [#ctor(preset)](#M-JaroslavNejedly-AdvancedBackground-#ctor-JaroslavNejedly-AdvancedBackgroundPreset- 'JaroslavNejedly.AdvancedBackground.#ctor(JaroslavNejedly.AdvancedBackgroundPreset)')
  - [#ctor(preset,upVector)](#M-JaroslavNejedly-AdvancedBackground-#ctor-JaroslavNejedly-AdvancedBackgroundPreset,OpenTK-Vector3d- 'JaroslavNejedly.AdvancedBackground.#ctor(JaroslavNejedly.AdvancedBackgroundPreset,OpenTK.Vector3d)')
  - [#ctor(upVector)](#M-JaroslavNejedly-AdvancedBackground-#ctor-OpenTK-Vector3d- 'JaroslavNejedly.AdvancedBackground.#ctor(OpenTK.Vector3d)')
  - [#ctor()](#M-JaroslavNejedly-AdvancedBackground-#ctor 'JaroslavNejedly.AdvancedBackground.#ctor')
  - [CurrentPreset](#F-JaroslavNejedly-AdvancedBackground-CurrentPreset 'JaroslavNejedly.AdvancedBackground.CurrentPreset')
  - [Sun](#P-JaroslavNejedly-AdvancedBackground-Sun 'JaroslavNejedly.AdvancedBackground.Sun')
  - [GetColor()](#M-JaroslavNejedly-AdvancedBackground-GetColor-OpenTK-Vector3d,System-Double[]- 'JaroslavNejedly.AdvancedBackground.GetColor(OpenTK.Vector3d,System.Double[])')
- [AdvancedBackgroundPreset](#T-JaroslavNejedly-AdvancedBackgroundPreset 'JaroslavNejedly.AdvancedBackgroundPreset')
  - [Default](#F-JaroslavNejedly-AdvancedBackgroundPreset-Default 'JaroslavNejedly.AdvancedBackgroundPreset.Default')
  - [GroundColor](#F-JaroslavNejedly-AdvancedBackgroundPreset-GroundColor 'JaroslavNejedly.AdvancedBackgroundPreset.GroundColor')
  - [HorizonColor](#F-JaroslavNejedly-AdvancedBackgroundPreset-HorizonColor 'JaroslavNejedly.AdvancedBackgroundPreset.HorizonColor')
  - [InScatterColor](#F-JaroslavNejedly-AdvancedBackgroundPreset-InScatterColor 'JaroslavNejedly.AdvancedBackgroundPreset.InScatterColor')
  - [NightBackground](#F-JaroslavNejedly-AdvancedBackgroundPreset-NightBackground 'JaroslavNejedly.AdvancedBackgroundPreset.NightBackground')
  - [NightColor](#F-JaroslavNejedly-AdvancedBackgroundPreset-NightColor 'JaroslavNejedly.AdvancedBackgroundPreset.NightColor')
  - [OutScatterColor](#F-JaroslavNejedly-AdvancedBackgroundPreset-OutScatterColor 'JaroslavNejedly.AdvancedBackgroundPreset.OutScatterColor')
  - [SunIntensity](#F-JaroslavNejedly-AdvancedBackgroundPreset-SunIntensity 'JaroslavNejedly.AdvancedBackgroundPreset.SunIntensity')
  - [SunIntensityMultiplier](#F-JaroslavNejedly-AdvancedBackgroundPreset-SunIntensityMultiplier 'JaroslavNejedly.AdvancedBackgroundPreset.SunIntensityMultiplier')
  - [SunSmallness](#F-JaroslavNejedly-AdvancedBackgroundPreset-SunSmallness 'JaroslavNejedly.AdvancedBackgroundPreset.SunSmallness')
  - [SunTint](#F-JaroslavNejedly-AdvancedBackgroundPreset-SunTint 'JaroslavNejedly.AdvancedBackgroundPreset.SunTint')
  - [SunDirection](#P-JaroslavNejedly-AdvancedBackgroundPreset-SunDirection 'JaroslavNejedly.AdvancedBackgroundPreset.SunDirection')
  - [Interpolate(p0,p1,t)](#M-JaroslavNejedly-AdvancedBackgroundPreset-Interpolate-JaroslavNejedly-AdvancedBackgroundPreset,JaroslavNejedly-AdvancedBackgroundPreset,System-Double- 'JaroslavNejedly.AdvancedBackgroundPreset.Interpolate(JaroslavNejedly.AdvancedBackgroundPreset,JaroslavNejedly.AdvancedBackgroundPreset,System.Double)')
- [AnimatedAdvancedBackground](#T-JaroslavNejedly-AnimatedAdvancedBackground 'JaroslavNejedly.AnimatedAdvancedBackground')
  - [#ctor(upVector)](#M-JaroslavNejedly-AnimatedAdvancedBackground-#ctor-OpenTK-Vector3d- 'JaroslavNejedly.AnimatedAdvancedBackground.#ctor(OpenTK.Vector3d)')
  - [#ctor()](#M-JaroslavNejedly-AnimatedAdvancedBackground-#ctor 'JaroslavNejedly.AnimatedAdvancedBackground.#ctor')
  - [End](#P-JaroslavNejedly-AnimatedAdvancedBackground-End 'JaroslavNejedly.AnimatedAdvancedBackground.End')
  - [EndPreset](#P-JaroslavNejedly-AnimatedAdvancedBackground-EndPreset 'JaroslavNejedly.AnimatedAdvancedBackground.EndPreset')
  - [Start](#P-JaroslavNejedly-AnimatedAdvancedBackground-Start 'JaroslavNejedly.AnimatedAdvancedBackground.Start')
  - [StartPreset](#P-JaroslavNejedly-AnimatedAdvancedBackground-StartPreset 'JaroslavNejedly.AnimatedAdvancedBackground.StartPreset')
  - [SunDirectionAnimator](#P-JaroslavNejedly-AnimatedAdvancedBackground-SunDirectionAnimator 'JaroslavNejedly.AnimatedAdvancedBackground.SunDirectionAnimator')
  - [Time](#P-JaroslavNejedly-AnimatedAdvancedBackground-Time 'JaroslavNejedly.AnimatedAdvancedBackground.Time')
  - [Clone()](#M-JaroslavNejedly-AnimatedAdvancedBackground-Clone 'JaroslavNejedly.AnimatedAdvancedBackground.Clone')
  - [getSerial()](#M-JaroslavNejedly-AnimatedAdvancedBackground-getSerial 'JaroslavNejedly.AnimatedAdvancedBackground.getSerial')
- [AnimatedSunLight](#T-JaroslavNejedly-AnimatedSunLight 'JaroslavNejedly.AnimatedSunLight')
  - [End](#P-JaroslavNejedly-AnimatedSunLight-End 'JaroslavNejedly.AnimatedSunLight.End')
  - [EndPreset](#P-JaroslavNejedly-AnimatedSunLight-EndPreset 'JaroslavNejedly.AnimatedSunLight.EndPreset')
  - [Start](#P-JaroslavNejedly-AnimatedSunLight-Start 'JaroslavNejedly.AnimatedSunLight.Start')
  - [StartPreset](#P-JaroslavNejedly-AnimatedSunLight-StartPreset 'JaroslavNejedly.AnimatedSunLight.StartPreset')
  - [Time](#P-JaroslavNejedly-AnimatedSunLight-Time 'JaroslavNejedly.AnimatedSunLight.Time')
  - [Clone()](#M-JaroslavNejedly-AnimatedSunLight-Clone 'JaroslavNejedly.AnimatedSunLight.Clone')
  - [getSerial()](#M-JaroslavNejedly-AnimatedSunLight-getSerial 'JaroslavNejedly.AnimatedSunLight.getSerial')
- [SunLight](#T-JaroslavNejedly-SunLight 'JaroslavNejedly.SunLight')
  - [#ctor()](#M-JaroslavNejedly-SunLight-#ctor 'JaroslavNejedly.SunLight.#ctor')
  - [#ctor(preset)](#M-JaroslavNejedly-SunLight-#ctor-JaroslavNejedly-AdvancedBackgroundPreset- 'JaroslavNejedly.SunLight.#ctor(JaroslavNejedly.AdvancedBackgroundPreset)')
  - [#ctor(background)](#M-JaroslavNejedly-SunLight-#ctor-JaroslavNejedly-AdvancedBackground- 'JaroslavNejedly.SunLight.#ctor(JaroslavNejedly.AdvancedBackground)')
  - [position](#P-JaroslavNejedly-SunLight-position 'JaroslavNejedly.SunLight.position')
  - [GetIntensity()](#M-JaroslavNejedly-SunLight-GetIntensity-Rendering-Intersection,OpenTK-Vector3d@- 'JaroslavNejedly.SunLight.GetIntensity(Rendering.Intersection,OpenTK.Vector3d@)')

<a name='T-JaroslavNejedly-AdvancedBackground'></a>
## AdvancedBackground `type`

##### Namespace

JaroslavNejedly

##### Summary

Advanced background class. Simulates sky color, based on provided argumnets.

<a name='M-JaroslavNejedly-AdvancedBackground-#ctor-JaroslavNejedly-AdvancedBackgroundPreset-'></a>
### #ctor(preset) `constructor`

##### Summary

Creates an instance of [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground') and sets the current preset as `preset`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| preset | [JaroslavNejedly.AdvancedBackgroundPreset](#T-JaroslavNejedly-AdvancedBackgroundPreset 'JaroslavNejedly.AdvancedBackgroundPreset') | Settings preset to be used by this instance |

<a name='M-JaroslavNejedly-AdvancedBackground-#ctor-JaroslavNejedly-AdvancedBackgroundPreset,OpenTK-Vector3d-'></a>
### #ctor(preset,upVector) `constructor`

##### Summary

Creates an instance of [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground') and sets the current preset as `preset`.



Also changes the current up vector according to `upVector`.
This is important beacause the "top of the sky" is rendered in the direction up vector.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| preset | [JaroslavNejedly.AdvancedBackgroundPreset](#T-JaroslavNejedly-AdvancedBackgroundPreset 'JaroslavNejedly.AdvancedBackgroundPreset') |  |
| upVector | [OpenTK.Vector3d](#T-OpenTK-Vector3d 'OpenTK.Vector3d') |  |

<a name='M-JaroslavNejedly-AdvancedBackground-#ctor-OpenTK-Vector3d-'></a>
### #ctor(upVector) `constructor`

##### Summary

Creates an instance of [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground'). The settings preset used is the default one ([Default](#F-JaroslavNejedly-AdvancedBackgroundPreset-Default 'JaroslavNejedly.AdvancedBackgroundPreset.Default')).



Changes the current up vector according to `upVector`.
This is important beacause the "top of the sky" is rendered in the direction up vector.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| upVector | [OpenTK.Vector3d](#T-OpenTK-Vector3d 'OpenTK.Vector3d') |  |

<a name='M-JaroslavNejedly-AdvancedBackground-#ctor'></a>
### #ctor() `constructor`

##### Summary

Creates an instance of [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground'). The settings preset used is the default one ([Default](#F-JaroslavNejedly-AdvancedBackgroundPreset-Default 'JaroslavNejedly.AdvancedBackgroundPreset.Default')).

##### Parameters

This constructor has no parameters.

<a name='F-JaroslavNejedly-AdvancedBackground-CurrentPreset'></a>
### CurrentPreset `constants`

##### Summary

Current setting used to draw the advanced background.

<a name='P-JaroslavNejedly-AdvancedBackground-Sun'></a>
### Sun `property`

##### Summary

Sun light source of this sky. Add this to your scene light source collection for directional light.

<a name='M-JaroslavNejedly-AdvancedBackground-GetColor-OpenTK-Vector3d,System-Double[]-'></a>
### GetColor() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-JaroslavNejedly-AdvancedBackgroundPreset'></a>
## AdvancedBackgroundPreset `type`

##### Namespace

JaroslavNejedly

##### Summary

Struct holding preset settings for [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground').

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-Default'></a>
### Default `constants`

##### Summary

Default preset. Basic "realistic" sky.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-GroundColor'></a>
### GroundColor `constants`

##### Summary

Color of "ground". This color should not be visible in normal scenes.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-HorizonColor'></a>
### HorizonColor `constants`

##### Summary

Color on the horizon. Used to create gradient in the sky.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-InScatterColor'></a>
### InScatterColor `constants`

##### Summary

Color that remains in incoming light as it passes through the atmosphere. This is basicaly the color of sky during sunset/sunrise.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-NightBackground'></a>
### NightBackground `constants`

##### Summary

Optional. If set, uses this to shade the sky during night time. [StarBackground](#T-JosefPelikan-StarBackground 'JosefPelikan.StarBackground')

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-NightColor'></a>
### NightColor `constants`

##### Summary

Color of the sky during night.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-OutScatterColor'></a>
### OutScatterColor `constants`

##### Summary

Color that is scatter away from incoming light. This is basicaly the sky color.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-SunIntensity'></a>
### SunIntensity `constants`

##### Summary

Intensity of the sun light source.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-SunIntensityMultiplier'></a>
### SunIntensityMultiplier `constants`

##### Summary

Changes the intensity of the sun light source.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-SunSmallness'></a>
### SunSmallness `constants`

##### Summary

The bigger this value is, the smaller the sun will be.

<a name='F-JaroslavNejedly-AdvancedBackgroundPreset-SunTint'></a>
### SunTint `constants`

##### Summary

Color of the sun disk on the sky.

<a name='P-JaroslavNejedly-AdvancedBackgroundPreset-SunDirection'></a>
### SunDirection `property`

##### Summary

Direction of sun. This positions the sun onto the sky.

<a name='M-JaroslavNejedly-AdvancedBackgroundPreset-Interpolate-JaroslavNejedly-AdvancedBackgroundPreset,JaroslavNejedly-AdvancedBackgroundPreset,System-Double-'></a>
### Interpolate(p0,p1,t) `method`

##### Summary

Mixes to presets using parameter `t`.

##### Returns

Interpolated preset parameters

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| p0 | [JaroslavNejedly.AdvancedBackgroundPreset](#T-JaroslavNejedly-AdvancedBackgroundPreset 'JaroslavNejedly.AdvancedBackgroundPreset') | Preset to be used when `t` <= 0 |
| p1 | [JaroslavNejedly.AdvancedBackgroundPreset](#T-JaroslavNejedly-AdvancedBackgroundPreset 'JaroslavNejedly.AdvancedBackgroundPreset') | Preset to be used when `t` >= 1 |
| t | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | Mixing amount |

<a name='T-JaroslavNejedly-AnimatedAdvancedBackground'></a>
## AnimatedAdvancedBackground `type`

##### Namespace

JaroslavNejedly

##### Summary

Animated advanced background class. Supports animated parameters.

<a name='M-JaroslavNejedly-AnimatedAdvancedBackground-#ctor-OpenTK-Vector3d-'></a>
### #ctor(upVector) `constructor`

##### Summary

Creates new instance of [AnimatedAdvancedBackground](#T-JaroslavNejedly-AnimatedAdvancedBackground 'JaroslavNejedly.AnimatedAdvancedBackground'). Also sets the up vector to the value provided in parameter `upVector`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| upVector | [OpenTK.Vector3d](#T-OpenTK-Vector3d 'OpenTK.Vector3d') | The up vector of the scene. |

<a name='M-JaroslavNejedly-AnimatedAdvancedBackground-#ctor'></a>
### #ctor() `constructor`

##### Summary

Creates new instance of [AnimatedAdvancedBackground](#T-JaroslavNejedly-AnimatedAdvancedBackground 'JaroslavNejedly.AnimatedAdvancedBackground').

##### Parameters

This constructor has no parameters.

<a name='P-JaroslavNejedly-AnimatedAdvancedBackground-End'></a>
### End `property`

##### Summary

*Inherit from parent.*

<a name='P-JaroslavNejedly-AnimatedAdvancedBackground-EndPreset'></a>
### EndPreset `property`

##### Summary

Preset at the end of the animation

<a name='P-JaroslavNejedly-AnimatedAdvancedBackground-Start'></a>
### Start `property`

##### Summary

*Inherit from parent.*

<a name='P-JaroslavNejedly-AnimatedAdvancedBackground-StartPreset'></a>
### StartPreset `property`

##### Summary

Preset at the beggining of the animation

<a name='P-JaroslavNejedly-AnimatedAdvancedBackground-SunDirectionAnimator'></a>
### SunDirectionAnimator `property`

##### Summary

Function that is used to set sun direction. If null sun direction will be interpolated between [StartPreset](#P-JaroslavNejedly-AnimatedAdvancedBackground-StartPreset 'JaroslavNejedly.AnimatedAdvancedBackground.StartPreset') and [EndPreset](#P-JaroslavNejedly-AnimatedAdvancedBackground-EndPreset 'JaroslavNejedly.AnimatedAdvancedBackground.EndPreset').

<a name='P-JaroslavNejedly-AnimatedAdvancedBackground-Time'></a>
### Time `property`

##### Summary

*Inherit from parent.*

<a name='M-JaroslavNejedly-AnimatedAdvancedBackground-Clone'></a>
### Clone() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-JaroslavNejedly-AnimatedAdvancedBackground-getSerial'></a>
### getSerial() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-JaroslavNejedly-AnimatedSunLight'></a>
## AnimatedSunLight `type`

##### Namespace

JaroslavNejedly

##### Summary

Animated sun light (directional light without falloff). Animation is done in the same way as in [AnimatedAdvancedBackground](#T-JaroslavNejedly-AnimatedAdvancedBackground 'JaroslavNejedly.AnimatedAdvancedBackground')

<a name='P-JaroslavNejedly-AnimatedSunLight-End'></a>
### End `property`

##### Summary

*Inherit from parent.*

<a name='P-JaroslavNejedly-AnimatedSunLight-EndPreset'></a>
### EndPreset `property`

##### Summary

Preset at the end of the animation

<a name='P-JaroslavNejedly-AnimatedSunLight-Start'></a>
### Start `property`

##### Summary

*Inherit from parent.*

<a name='P-JaroslavNejedly-AnimatedSunLight-StartPreset'></a>
### StartPreset `property`

##### Summary

Preset at the beggining of the animation

<a name='P-JaroslavNejedly-AnimatedSunLight-Time'></a>
### Time `property`

##### Summary

*Inherit from parent.*

<a name='M-JaroslavNejedly-AnimatedSunLight-Clone'></a>
### Clone() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-JaroslavNejedly-AnimatedSunLight-getSerial'></a>
### getSerial() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-JaroslavNejedly-SunLight'></a>
## SunLight `type`

##### Namespace

JaroslavNejedly

##### Summary

Directional light without any falloff. Used by [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground').

<a name='M-JaroslavNejedly-SunLight-#ctor'></a>
### #ctor() `constructor`

##### Summary

Creates new instance of [SunLight](#T-JaroslavNejedly-SunLight 'JaroslavNejedly.SunLight') with default preset.

##### Parameters

This constructor has no parameters.

<a name='M-JaroslavNejedly-SunLight-#ctor-JaroslavNejedly-AdvancedBackgroundPreset-'></a>
### #ctor(preset) `constructor`

##### Summary

Creates new instance of [SunLight](#T-JaroslavNejedly-SunLight 'JaroslavNejedly.SunLight'). The preset parameters are in `preset`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| preset | [JaroslavNejedly.AdvancedBackgroundPreset](#T-JaroslavNejedly-AdvancedBackgroundPreset 'JaroslavNejedly.AdvancedBackgroundPreset') | Preset to be used by this sun light. |

<a name='M-JaroslavNejedly-SunLight-#ctor-JaroslavNejedly-AdvancedBackground-'></a>
### #ctor(background) `constructor`

##### Summary

Creates new instance of [SunLight](#T-JaroslavNejedly-SunLight 'JaroslavNejedly.SunLight'). The preset parameters are taken from `background` parameters.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| background | [JaroslavNejedly.AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground') |  |

<a name='P-JaroslavNejedly-SunLight-position'></a>
### position `property`

##### Summary

*Inherit from parent.*

<a name='M-JaroslavNejedly-SunLight-GetIntensity-Rendering-Intersection,OpenTK-Vector3d@-'></a>
### GetIntensity() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.
