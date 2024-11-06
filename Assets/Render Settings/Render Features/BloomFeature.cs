using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomFeature : ScriptableRendererFeature
{
    public LayerMask bloomLayerMask;
    public RenderTexture BloomTexture;

    public RenderPassEvent _BloomEvent = RenderPassEvent.AfterRenderingOpaques;

    BloomPass m_BloomPass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_BloomPass = new BloomPass(BloomTexture, bloomLayerMask);
        m_BloomPass.renderPassEvent = _BloomEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(m_BloomPass);
    }
}

class BloomPass : ScriptableRenderPass
{
    private ProfilingSampler m_ProfilingSampler;
    private FilteringSettings m_FilteringSettings;
    private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();
    private RenderTexture target;

    public BloomPass(RenderTexture targetTexture, LayerMask layerMask)
    {
        m_ProfilingSampler = new ProfilingSampler("RenderBloom");
        m_FilteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);

        target = targetTexture;

        m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        RenderTextureDescriptor descriptor = cameraTextureDescriptor;
        descriptor.depthBufferBits = 24;
        cmd.GetTemporaryRT(target.GetInstanceID(), descriptor);
        ConfigureTarget(target);
        ConfigureClear(ClearFlag.All, Color.black);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;
        SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
        DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, m_ProfilingSampler))
        {
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    // Cleanup any allocated resources that were created during the execution of this render pass.
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(target.GetInstanceID());
    }
}