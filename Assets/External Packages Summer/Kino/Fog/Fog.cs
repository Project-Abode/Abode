//
// KinoFog - Deferred fog effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;

namespace Kino
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Kino Image Effects/Fog")]
    public class Fog : MonoBehaviour
    {
        #region Public Properties

        [SerializeField]
        FogMode _fogMode = FogMode.Exponential;

        [SerializeField]
        Vector2 _fogLinearStartEnd = new Vector2(0, 300);

        [SerializeField]
        float _fogDensity = 0.07f;

        // Start distance
        [SerializeField]
        float _startDistance = 1;

        public float startDistance {
            get { return _startDistance; }
            set { _startDistance = value; }
        }

        // Use radial distance
        [SerializeField]
        bool _useRadialDistance = true;

        public bool useRadialDistance {
            get { return _useRadialDistance; }
            set { _useRadialDistance = value; }
        }

        #endregion

        #region Private Properties


        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            Shader.EnableKeyword("KINO_FOG_USE_SKYBOX");
        }

        void OnDisable()
        {
            Shader.DisableKeyword("KINO_FOG_USE_SKYBOX");
        }

        void OnPreRender()
        {
            _startDistance = Mathf.Max(_startDistance, 0.0f);
            Shader.SetGlobalFloat("_FogDistanceOffset", _startDistance);

            var mode = _fogMode;
            if (mode == FogMode.Linear)
            {
                var start = _fogLinearStartEnd.x;
                var end = _fogLinearStartEnd.y;
                var invDiff = 1.0f / Mathf.Max(end - start, 1.0e-6f);
                Shader.SetGlobalFloat("_FogLinearGrad", -invDiff);
                Shader.SetGlobalFloat("_FogLinearOffs", end * invDiff);
                Shader.DisableKeyword("KINO_FOG_EXP");
                Shader.DisableKeyword("KINO_FOG_EXP2");
                Shader.EnableKeyword("KINO_FOG_LINEAR");
            }
            else if (mode == FogMode.Exponential)
            {
                const float coeff = 1.4426950408f; // 1/ln(2)
                var density = _fogDensity;
                Shader.SetGlobalFloat("_FogDensity", coeff * density);
                Shader.EnableKeyword("KINO_FOG_EXP");
                Shader.DisableKeyword("KINO_FOG_EXP2");
                Shader.DisableKeyword("KINO_FOG_LINEAR");
            }
            else // FogMode.ExponentialSquared
            {
                const float coeff = 1.2011224087f; // 1/sqrt(ln(2))
                var density = _fogDensity;
                Shader.SetGlobalFloat("_FogDensity", coeff * density);
                Shader.DisableKeyword("KINO_FOG_EXP");
                Shader.EnableKeyword("KINO_FOG_EXP2");
                Shader.DisableKeyword("KINO_FOG_LINEAR") ;
            }

            if (_useRadialDistance)
                Shader.EnableKeyword("KINO_FOG_RADIAL_DIST");
            else
                Shader.DisableKeyword("KINO_FOG_RADIAL_DIST");

            // Transfer the skybox parameters.
            var skybox = RenderSettings.skybox;
                
            // Modified for Vertical Gradient Skybox
            Shader.SetGlobalColor("_FirstGradient1", skybox.GetColor("_FirstGradient1").linear);
            Shader.SetGlobalColor("_FirstGradient2", skybox.GetColor("_FirstGradient2").linear);
            Shader.SetGlobalColor("_FirstGradient3", skybox.GetColor("_FirstGradient3").linear);
            Shader.SetGlobalColor("_FirstGradient4", skybox.GetColor("_FirstGradient4").linear);
            Shader.SetGlobalVector("_FirstGradientKeys", skybox.GetVector("_FirstGradientKeys"));
            Shader.SetGlobalColor("_SecondGradient1", skybox.GetColor("_SecondGradient1").linear);
            Shader.SetGlobalColor("_SecondGradient2", skybox.GetColor("_SecondGradient2").linear);
            Shader.SetGlobalColor("_SecondGradient3", skybox.GetColor("_SecondGradient3").linear);
            Shader.SetGlobalColor("_SecondGradient4", skybox.GetColor("_SecondGradient4").linear);
            Shader.SetGlobalVector("_SecondGradientKeys", skybox.GetVector("_SecondGradientKeys"));
            Shader.SetGlobalColor("_ThirdGradient1", skybox.GetColor("_ThirdGradient1").linear);
            Shader.SetGlobalColor("_ThirdGradient2", skybox.GetColor("_ThirdGradient2").linear);
            Shader.SetGlobalColor("_ThirdGradient3", skybox.GetColor("_ThirdGradient3").linear);
            Shader.SetGlobalColor("_ThirdGradient4", skybox.GetColor("_ThirdGradient4").linear);
            Shader.SetGlobalVector("_ThirdGradientKeys", skybox.GetVector("_ThirdGradientKeys"));
            Shader.SetGlobalColor("_FourthGradient1", skybox.GetColor("_FourthGradient1").linear);
            Shader.SetGlobalColor("_FourthGradient2", skybox.GetColor("_FourthGradient2").linear);
            Shader.SetGlobalColor("_FourthGradient3", skybox.GetColor("_FourthGradient3").linear);
            Shader.SetGlobalColor("_FourthGradient4", skybox.GetColor("_FourthGradient4").linear);
            Shader.SetGlobalVector("_FourthGradientKeys", skybox.GetVector("_FourthGradientKeys"));
            Shader.SetGlobalFloat("_Progress", skybox.GetFloat("_Progress"));
        }

        #endregion
    }
}
