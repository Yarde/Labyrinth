using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Utils
{
    public class CutoutMaskUI : Image
    {
        private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

        public override Material materialForRendering
        {
            get
            {
                var material = new Material(base.materialForRendering);
                material.SetInt(StencilComp, (int) CompareFunction.NotEqual);
                return material;
            }
        }
    }
}