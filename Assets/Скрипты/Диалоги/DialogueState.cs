using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

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

        public override IEnumerator Update()
        {
            mn.text.text = "";
            mn.character.text = "";

            if (mn.player != null)
                mn.player.Enable();

            while (true)
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, new Vector3(1, 0, 1), 8 * Time.deltaTime);
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 6 * Time.deltaTime);
                mn.novelTransform.anchoredPosition = new Vector3(Mathf.Lerp(mn.novelTransform.anchoredPosition.x, mn.novelTransform.sizeDelta.x, 8 * Time.deltaTime), 0, 0);

                yield return base.Update();
            }
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

            if (mn.current.dialogueCharacter != null)
            {
                var sprite = mn.current.dialogueCharacter.emotions[mn.current.emotion];
                mn.novelSprite.sprite = sprite;
                mn.novelSprite.rectTransform.sizeDelta = new Vector2(sprite.texture.width, mn.novelSprite.rectTransform.sizeDelta.y);
            }

            yield return base.Start();

            while (mn.dialogueBox.localScale != Vector3.one)
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, Vector3.one, 14 * Time.deltaTime);

                yield return base.Update();
            }

            yield return new WaitForSeconds(mn.current.startDelay);

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

        public override IEnumerator Update()
        {
            while (true)
            {
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 6 * Time.deltaTime);
                mn.novelTransform.anchoredPosition = new Vector3(Mathf.Lerp(mn.novelTransform.anchoredPosition.x,
                    mn.current.dialogueCharacter == null ? mn.novelTransform.sizeDelta.x : -32, 8 * Time.deltaTime), 0, 0);

                yield return base.Update();
            }
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
                mn.index++;

                if (mn.index >= mn.dialogues.Length)
                {
                    mn.ChangeState(mn.idleState);
                    yield break;
                }

                mn.ChangeState(mn.printingState);
            }

            yield return base.Start();

            if (mn.current.variants != null)
            {
                mn.variantBox.sizeDelta = new Vector2(mn.variantBox.sizeDelta.x, 32 * mn.current.variants.Length);

                while (mn.variantBox.localScale != Vector3.one)
                {
                    mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, Vector3.one, 14 * Time.deltaTime);
                    yield return base.Update();
                }

                for (int i = 0; i < mn.current.variants.Length; i++)
                {
                    var variant = Object.Instantiate(mn.variantPrefab, mn.variantBox);
                    variant.GetComponent<TMP_Text>().text = mn.current.variants[i].variant;
                    variant.GetComponent<TMP_Text>().faceColor = i == 0 ? Color.white : Color.grey;
                    variant.index = i;
                    variant.system = mn;

                    mn.variantObjects.Add(variant.gameObject);
                }
            }
        }

        public override IEnumerator Update()
        {
            while (true)
            {
                if (mn.current.variants == null)
                    mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 6 * Time.deltaTime);

                mn.novelTransform.anchoredPosition = new Vector3(Mathf.Lerp(mn.novelTransform.anchoredPosition.x,
                    mn.current.dialogueCharacter == null ? mn.novelTransform.sizeDelta.x : 0, 8 * Time.deltaTime), 0, 0);
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, Vector3.one, 14 * Time.deltaTime);

                yield return base.Update();
            }
        }

        public override void Submit(InputAction.CallbackContext c)
        {
            if (mn.current.variants != null)
                mn.ChooseVariant();

            else
                mn.ChangeDialogue();
        }

        public override void GUIInput()
        {
            if (mn.current.variants == null)
                mn.ChangeDialogue();
        }

        public override void Move(InputAction.CallbackContext c)
        {
            if (mn.current.variants != null)
                mn.ChangeVariant(mn.currentVariant + (c.ReadValue<Vector2>().y > 0 ? -1 : 1));
        }
    }
}