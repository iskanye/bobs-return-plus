using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialogues
{
    public static class TextUtilities
    {
        //основано на оф. примере TextMeshPro из раздела Examples
        public static IEnumerator AnimateVertexColors(TMP_Text textMesh, Color32 c0, int startPos = 0, float delay = 0f, Action doEveryCharacter = null)
        {
            if (delay<0)
            {
                throw new ArgumentOutOfRangeException(nameof(delay), "less than 0");
            }
            
            if (startPos<0 || startPos>=textMesh.text.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startPos), "startPos OutOfRange");
            }
            
            TMP_TextInfo textInfo = textMesh.textInfo;

            Color32[] newVertexColors;
            int characterCount = textInfo.characterCount;
            if (characterCount == 0)
            {
                throw new NullReferenceException("empty text");
            }
            
            for (int i = startPos; i < characterCount; i++)
            {

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                // Only change the vertex color if the text element is visible.
                if (textInfo.characterInfo[i].isVisible)
                {
                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;
                    
                    // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                    textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    
                    // This last process could be done to only update the vertex data that has changed as opposed to all of the vertex data but it would require extra steps and knowing what type of renderer is used.
                    // These extra steps would be a performance optimization but it is unlikely that such optimization will be necessary.
                }
                doEveryCharacter?.Invoke();
                if (delay>0)
                {
                    for (float j = delay; j > 0; j -= Time.deltaTime)
                        yield return new WaitForFixedUpdate();
                }
            }
        }

        public static IEnumerator MakeTextTransparent(TMP_Text textMesh, int startPosition)
        {
            yield return AnimateVertexColors(textMesh, Color.clear, startPosition);
        }

        public static IEnumerator ForceOriginalColor(TMP_Text textMesh)
        {
            textMesh.ForceMeshUpdate();
            yield break;
        }
    }
}