/* Author:  Takeshi
 * Date:    11/06/2021
 * purpose: A common Pass for convert color gamme to Fix UI opacity
 */
namespace UnityEngine.Rendering.Universal.Internal
{
    /// <summary>
    /// Fix Gamma of Render Target
    /// </summary>
    public class FixingGammaPass : ScriptableRenderPass
    {
        RenderTargetHandle m_Source;
        Material m_BlitMaterial;

        RenderTargetHandle m_TempBlit;
        ProfilingSampler m_ProfilingSampler;
        string m_ShaderKeyword;
        
        public FixingGammaPass(RenderPassEvent evt, Material blitMaterial, string profilerTag, string shaderKeyword)
        {
            //base.profilingSampler = new ProfilingSampler(nameof(FixingGammaPass));

            m_BlitMaterial = blitMaterial;
            renderPassEvent = evt;
            m_ProfilingSampler = new ProfilingSampler(profilerTag);
            m_ShaderKeyword = shaderKeyword;
            m_TempBlit.Init("_FixingGamma");
        }
        public FixingGammaPass(RenderPassEvent evt, Material blitMaterial, string profilerTag, string shaderKeyword,string blitRTName)
        {
            m_BlitMaterial = blitMaterial;
            renderPassEvent = evt;
            m_ProfilingSampler = new ProfilingSampler(profilerTag);
            m_ShaderKeyword = shaderKeyword;
            m_TempBlit.Init(blitRTName);
        }

        /// <summary>
        /// Configure the pass
        /// </summary>
        /// <param name="colorHandle"></param>
        public void Setup(in RenderTargetHandle colorHandle)
        {
            m_Source = colorHandle;
            
        }

        /// <inheritdoc/>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            ref CameraData cameraData = ref renderingData.cameraData;
            ref Camera camera = ref cameraData.camera;
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSampler))
            {
                cmd.SetGlobalTexture(ShaderPropertyId.sourceTex, m_Source.Identifier());
                // if (cameraData.isSceneViewCamera)
                // {
                    // Create a new RT.
                    RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
                    desc.depthBufferBits = 0;
                    cmd.GetTemporaryRT(m_TempBlit.id, desc);
                    
                    // Conversion Gamma
                    cmd.EnableShaderKeyword(m_ShaderKeyword);
                    cmd.Blit(m_Source.Identifier(),m_TempBlit.Identifier(),m_BlitMaterial);
                    cmd.DisableShaderKeyword(m_ShaderKeyword);
                
                    // Return to main RT
                    cmd.Blit(m_TempBlit.Identifier(),m_Source.Identifier());
                    
                // }
                // else
                // {
                //     RenderTargetIdentifier cameraTarget = new RenderTargetIdentifier(m_TempBlit.id);
                //     CoreUtils.SetRenderTarget(
                //         cmd,
                //         cameraTarget);
                //     
                //     cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
                //     cmd.SetViewport(cameraData.pixelRect);
                //     cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_BlitMaterial);
                //     cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
                //     
                // }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(m_TempBlit.id); 
        }
    }
}
