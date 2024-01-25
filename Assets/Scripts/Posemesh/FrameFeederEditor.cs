using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.XR.ARFoundation;

namespace Auki.Integration.ARFoundation.Manna
{
    public class FrameFeederEditor : FrameFeederBase
    {
        private static readonly int TextureSingle = Shader.PropertyToID("_textureSingle");
        
        /// <summary>
        /// AR Camera Manager necessary to supply Manna with camera feed frames.
        /// </summary>
        [SerializeField] protected ARCameraBackground ArCameraBackground;

        private RenderTexture _videoTexture;

        protected override void Awake()
        {
            base.Awake();

            if (ArCameraBackground == null)
                ArCameraBackground = GetComponent<ARCameraBackground>();
        }

        /// <summary>
        /// Manna needs to be supplied with camera feed frames so it can detect QR codes and perform Instant Calibration.
        /// For this particular implementation, we use AR Foundations AR Camera Manager to retrieve the images on CPU side.
        /// </summary>
        protected override void ProcessFrame(ARCameraFrameEventArgs frameInfo)
        {
            CreateOrUpdateVideoTexture();
            if (_videoTexture == null) return;

            CopyVideoTexture();

            MannaInstance.ProcessVideoFrameTexture(
                _videoTexture,
                ArCamera.projectionMatrix,
                ArCamera.worldToCameraMatrix
            );
        }

        private void CreateOrUpdateVideoTexture()
        {
            if (_videoTexture != null) return;

            var textureNames = ArCameraBackground.material.GetTexturePropertyNames();
            for (var i = 0; i < textureNames.Length; i++)
            {
                var texture = ArCameraBackground.material.GetTexture(textureNames[i]);
                if (texture == null) continue;
                Debug.Log(
                    $"Creating video texture based on: {textureNames[i]}, format: {texture.graphicsFormat}, size: {texture.width}x{texture.height}");
                _videoTexture = new RenderTexture(texture.width, texture.height, 0, GraphicsFormat.R8_UNorm);
                break;
            }
        }

        private void CopyVideoTexture()
        {
            // Copy the camera background to a RenderTexture
            var textureY = ArCameraBackground.material.GetTexture(TextureSingle);
            Graphics.Blit(textureY, _videoTexture);
        }
    }
}