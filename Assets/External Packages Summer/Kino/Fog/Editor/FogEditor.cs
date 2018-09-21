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
using UnityEditor;

namespace Kino
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Fog))]
    public class FogEditor : Editor
    {
        SerializedProperty _script;

        SerializedProperty _fogMode;
        SerializedProperty _fogLinearStartEnd;
        SerializedProperty _fogDensity;

        SerializedProperty _startDistance;

        SerializedProperty _useRadialDistance;

        void OnEnable()
        {
            _script = serializedObject.FindProperty("m_Script");

            _fogMode = serializedObject.FindProperty("_fogMode");
            _fogLinearStartEnd = serializedObject.FindProperty("_fogLinearStartEnd");
            _fogDensity = serializedObject.FindProperty("_fogDensity");

            _startDistance = serializedObject.FindProperty("_startDistance");

            _useRadialDistance = serializedObject.FindProperty("_useRadialDistance");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_script, true);
            }

            EditorGUILayout.PropertyField(_fogMode);
            int fogMode = _fogMode.intValue;
            if (fogMode == (int)FogMode.Linear)
            {
                Vector2 startEnd = _fogLinearStartEnd.vector2Value;
                startEnd.x = EditorGUILayout.FloatField("Start", startEnd.x);
                startEnd.y = EditorGUILayout.FloatField("End", startEnd.y);
                _fogLinearStartEnd.vector2Value = startEnd;
            }
            else
            {
                EditorGUILayout.PropertyField(_fogDensity);
            }

            EditorGUILayout.PropertyField(_startDistance);

            EditorGUILayout.PropertyField(_useRadialDistance);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
