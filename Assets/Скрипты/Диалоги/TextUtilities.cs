using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Dialogues
{
    public static class TextUtilities
    {
        //основано на оф. примере TextMeshPro из раздела Examples
        public static IEnumerator AnimateText(TMP_Text textMesh, string text, float delay = 0f, Action doEveryCharacter = null)
        {            
            foreach (var i in text)
            {
                textMesh.text += i;
                doEveryCharacter?.Invoke();

                if (delay > 0)
                {
                    for (float j = delay; j > 0; j -= Time.deltaTime)
                        yield return new WaitForFixedUpdate();
                }
            }
        }
/*
        public static IEnumerator MakeTextTransparent(TMP_Text textMesh, int startPosition)
        {
            yield return AnimateVertexColors(textMesh, Color.clear, startPosition);
        }

        public static IEnumerator ForceOriginalColor(TMP_Text textMesh)
        {
            textMesh.ForceMeshUpdate();
            yield break;
        }*/
    }
}