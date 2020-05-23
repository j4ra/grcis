# Extension: Advanced background

![example0](imgs/img0.png)

## Author: Jaroslav Nejedlý

## Category: Background

## Namespace: JaroslavNejedly

## Class Name: AdvancedBackground : IBackground, ILightSource

## ITimeDependent: Not yet... 😉

## Source file: AdvancedBackground.cs

This extension provides you with nice looking sky (without clouds). The advanced background also implements ILightSource, it is needed for sun rendering. I tried to make all paramteres customizable. And I also provided default values that IMHO look good (or at least I tried to make them look good).

The list of customizable parameters will appear here (when the implementation is finalized):
 * 
 * 
 * 

## Examples &amp; sample scripts:

Examples will be provided when implementation is finalized.

```
AdvancedBackground adv = new AdvancedBackground(new double[] {0.1, 0.2, 0.4}, new double[] {0.45, 0.55, 0.65}, new double[] {0.1, 0.05, 0.02}, Vector3d.UnitY);
scene.background = adv;
scene.Sources.Add(adv);
```

#### Sample scene script: DemoScene.cs

## Issues and things to be aware of:

The advanced background adds quite a lot of light into the scene. Be aware of that. This might cause some lighting issues. The amount of light provided by the advanced background is substaintial, even if you do not add provided sun light source into the scene.

## Imges &amp; videos

![example0](imgs/img0.png)

![example1](imgs/img1.png)