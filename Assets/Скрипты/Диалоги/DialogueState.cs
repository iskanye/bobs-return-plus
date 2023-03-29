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
            mn.prevText = "";

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
            ((RectTransform)mn.text.transform).anchoredPosition = mn.current.dialogueCharacter != null ? new Vector2(-64, -6) : new Vector2(0, -6);
            ((RectTransform)mn.text.transform).sizeDelta = mn.current.dialogueCharacter != null ? new Vector2(320, 96) : new Vector2(448, 96);

            if (mn.current.dialogueCharacter != null)
            {
                var sprite = mn.current.dialogueCharacter.emotions.First(i => i.id == mn.current.emotionId).sprite;
                mn.novelSprite.sprite = sprite;
                mn.novelSprite.rectTransform.sizeDelta = new Vector2(sprite.texture.width, mn.novelSprite.rectTransform.sizeDelta.y);
            }

            yield return base.Start();    
            yield return new WaitForSeconds(mn.current.startDelay + (mn.index == 0 ? .35f : 0));

            if (mn.current.clearPreviousText)
                mn.text.text = mn.prevText = "";

            if (!mn.current.showStraightaway)
            {
                yield return TextUtilities.AnimateText(mn.text, mn.current.text, 0.015f, () =>
                {
                    mn.textSFX.pitch = Random.Range(.95f, 1.05f);
                    mn.textSFX.Play();
                });
                mn.textSFX.Stop();
            }

            else
                mn.text.text += mn.current.text;

            mn.ChangeState(mn.waitingState);
        }

        public override IEnumerator Stop()
        {            
            mn.prevText = mn.text.text = mn.prevText + mn.current.text;
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