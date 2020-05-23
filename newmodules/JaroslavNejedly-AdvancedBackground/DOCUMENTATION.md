<a name='assembly'></a>
# AdvancedBackground

## Contents

- [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground')
  - [#ctor(preset)](#M-JaroslavNejedly-AdvancedBackground-#ctor-JaroslavNejedly-AdvancedBackgroundPreset- 'JaroslavNejedly.AdvancedBackground.#ctor(JaroslavNejedly.AdvancedBackgroundPreset)')
  - [#ctor(preset,upVector)](#M-JaroslavNejedly-AdvancedBackground-#ctor-JaroslavNejedly-AdvancedBackgroundPreset,OpenTK-Vector3d- 'JaroslavNejedly.AdvancedBackground.#ctor(JaroslavNejedly.AdvancedBackgroundPreset,OpenTK.Vector3d)')
  - [#ctor(upVector)](#M-JaroslavNejedly-AdvancedBackground-#ctor-OpenTK-Vector3d- 'JaroslavNejedly.AdvancedBackground.#ctor(OpenTK.Vector3d)')
  - [#ctor()](#M-JaroslavNejedly-AdvancedBackground-#ctor 'JaroslavNejedly.AdvancedBackground.#ctor')
  - [CurrentPreset](#F-JaroslavNejedly-AdvancedBackground-CurrentPreset 'JaroslavNejedly.AdvancedBackground.CurrentPreset')
  - [Sun](#P-JaroslavNejedly-AdvancedBackground-Sun 'JaroslavNejedly.AdvancedBackground.Sun')
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
- [SunLight](#T-JaroslavNejedly-SunLight 'JaroslavNejedly.SunLight')

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

<a name='T-JaroslavNejedly-SunLight'></a>
## SunLight `type`

##### Namespace

JaroslavNejedly

##### Summary

Directional light without any falloff. Used by [AdvancedBackground](#T-JaroslavNejedly-AdvancedBackground 'JaroslavNejedly.AdvancedBackground').
