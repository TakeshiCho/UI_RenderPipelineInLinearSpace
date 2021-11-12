# UI Render Pipeline In Linear Space ( Developing... )
This project is an Unity render pipeline and shader framework for UI which is based on *Unity URP*.
We created it for the purpose of fixing the alpha value of the UI images ( opacity of images )  which is wrong in *linear color space*, 
that keep the sRGB workflow for the UI designers in Unity.

#### 线性空间 UI 渲染管线 ( 开发中... )

这个项目是一套基于 Unity URP 的 UI 渲染管线和 Shader 框架，
为了修复线性色彩空间中的的 UI 切图的透明度错误，
能够让 UI 设计师在 Unity 中保持原有的 sRBG 工作流。

#### リニア UI レンダーパイプライン ( 開発中... )

このプロジェクトは Unity URP に基づいて作った UI レンダーパイプラインとシェーダーフレームワークです、
Unity リニア の色空間にある UI 画像の透明さをなおす為に作れたものです。
ならば、UI デザイナーはいつもように sRGB の業務フローをすることができます。

# Versions and Schecdule

### Unity:
* Unity: 2020.3.21 f1c1  
* Unity UI: com.unity.render-pipelines.universal@10.6.0  
* Universal RP: com.unity.ugui@1.0.0  

### Feature:
* 针对 Game 视图，对 UI 透明度进行矫正，使其和 Ps 里的不透明度保持一致。

![Opacity_Comparison](./Readme/Opacity_Comparison.png)

### Update:
* 修复了 Reflection Probe 烘焙错误;
* 修复了 scene 视图 UI 颜色错误;
* 增加了对非 post-Processing 环境下的校色支持;
* 改善了 校色后的 3D 渲染色阶失真。

### Plan:
* 修复 scene 视图 Reflection Probe 显示不正确;

# Pipeline Flowchart
![UI_RenderPipelineFlowChart](./Readme/UI_RenderPipelineFlowChart.png)![UI_RenderPipeline](./Readme/UI_RenderPipeline.png)

## Why using *RGBA32 UNorm* for the UI Buffer
When the final 3D render image is blit into the UI buffer, and transform to the Gamma Space, 
we can compare the resolutions of Color Depth in different graphics format of the UI Buffer.
Evidently the RGBA32 UNorm has the more details.  

![UI_RenderPipeline](./Readme/ColorDepthComparison.png)

