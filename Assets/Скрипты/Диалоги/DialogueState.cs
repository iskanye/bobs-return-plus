using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;

namespace Dialogues
{
    public class DialoguesState : State<DialogueSystem>
    {
        public DialoguesState(DialogueSystem sys) : base(sys) { }

        public virtual void Submit(InputAction.CallbackContext c) { }

        public virtual void GUIInput() { }

        public virtual void Move(InputAction.CallbackContext c) { }
    }

    public class IdleState : DialoguesState
    {
        public IdleState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Start() 
        { 
            if (mn.text.text != "")                
                mn.dialogueBoxAnimator.Play("Closing");

            mn.novelAnimator.SetBool("Novel", false);               
            mn.text.text = "";
            mn.character.text = "";

            yield return new WaitForSeconds(.1f);
            yield return base.Start();
        }

        public override IEnumerator Update()
        {
            if (mn.player != null)
                mn.player.Enable();

            yield return base.Update();
        }
    }

    public class PrintingState : DialoguesState
    {
        public PrintingState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Start()
        {
            if (mn.player != null)
            {
                mn.player.Disable();
            }

            mn.current = mn.dialogues[mn.index];
            mn.character.text = mn.current.character;

            if (mn.current.action != null)
                mn.current.action.Invoke(mn.obj);

            mn.novelAnimator.SetBool("Novel", mn.current.dialogueCharacter != null);

            if (mn.current.dialogueCharacter != null)
            {
                var sprite = mn.current.dialogueCharacter.emotions.First(i => i.id == mn.current.emotionId).sprite;
                mn.novelSprite.sprite = sprite;
                mn.novelSprite.rectTransform.sizeDelta = new Vector2(sprite.texture.width, mn.novelSprite.rectTransform.sizeDelta.y);
                ((RectTransform)mn.text.transform).anchoredPosition = new Vector2(-64, -6);
                ((RectTransform)mn.text.transform).sizeDelta = new Vector2(320, 96);
            }

            else 
            {
                ((RectTransform)mn.text.transform).anchoredPosition = new Vector2(0, -6);
                ((RectTransform)mn.text.transform).sizeDelta = new Vector2(448, 96);
            }

            yield return base.Start();    
            yield return new WaitForSeconds(mn.current.startDelay + (mn.index == 0 ? .35f : 0));

            mn.text.text = GetCurrentText();

            if (!mn.current.showStraightaway)
            {
                int startPosition = mn.text.text.Length - mn.current.text.Length;
                mn.text.ForceMeshUpdate();
                yield return TextUtilities.MakeTextTransparent(mn.text, startPosition);
                yield return TextUtilities.AnimateVertexColors(mn.text, Color.white, startPosition, 0.015f, () =>
                {
                    mn.textSFX.pitch = Random.Range(.95f, 1.05f);
                    mn.textSFX.Play();
                });
                mn.textSFX.Stop();
            }

            mn.ChangeState(mn.waitingState);
        }

        public override IEnumerator Stop()
        {
            mn.text.text = GetCurrentText();
            mn.prevText = mn.text.text;
            yield return TextUtilities.ForceOriginalColor(mn.text);
            yield return base.Stop();
        }

        public override void GUIInput()
        {
            if (mn.current.cantSkip)
                return;
            
            mn.ChangeState(mn.waitingState);
        }

        public override void Submit(InputAction.CallbackContext c)
        {
            GUIInput();
        }

        private string GetCurrentText()
        {
            return mn.current.clearPreviousText ? mn.current.text : mn.prevText + mn.current.text;
        }
    }

    public class WaitingState : DialoguesState
    {
        public WaitingState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Start()
        {
            if (mn.current.dontWait)
            {
                mn.ChangeDialogue();
                yield break;
            }

            if (mn.current.variants != null && mn.current.variants.Length != 0)
            { 
                for (int i = 0; i < mn.current.variants.Length; i++)
                {
                    var variant = GameObject.Instantiate(mn.variantPrefab, mn.variantBox);
                    variant.GetComponent<TMP_Text>().text = mn.current.variants[i].variant;
                    variant.GetComponent<TMP_Text>().faceColor = i == 0 ? Color.white : Color.grey;
                    variant.index = i;
                    variant.system = mn;

                    mn.variantObjects.Add(variant.gameObject);
                } 

                mn.variantBox.sizeDelta = new Vector2(mn.variantBox.sizeDelta.x, 32 * mn.current.variants.Length);
            }

            yield return base.Start();
        }

        public override void Submit(InputAction.CallbackContext c)
        {
            if (mn.current.variants != null && mn.current.variants.Length != 0)
                mn.ChooseVariant();

            else
                mn.ChangeDialogue();
        }

        public override void GUIInput()
        {
            if (mn.current.variants == null || mn.current.variants.Length == 0)
                mn.ChangeDialogue();
        }

        public override void Move(InputAction.CallbackContext c)
        {
            if (mn.current.variants != null && mn.current.variants.Length != 0)
                mn.ChangeVariant(mn.currentVariant + (c.ReadValue<Vector2>().y > 0 ? -1 : 1));
        }
    }
}