using System.Collections;
using UnityEngine;
using TMPro;

namespace Dialogues
{
    public class IdleState : State<DialogueSystem>
    {
        public IdleState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Update()
        {
            mn.text.text = "";
            mn.character.text = "";

            if (mn.player != null)
                mn.player.enabled = true;

            mn.novelAnimator.ResetTrigger("Show");
            mn.novelAnimator.SetTrigger("Fade");

            while (true)
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, new Vector3(1, 0, 1), 8 * Time.deltaTime);
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 6 * Time.deltaTime);

                yield return base.Update();
            }
        }
    }

    public class PrintingState : State<DialogueSystem>
    {
        public PrintingState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Start()
        {
            if (mn.player != null)
            {
                mn.player.enabled = false;
                mn.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            mn.current = mn.dialogues[mn.index];
            mn.character.text = mn.current.character;

            if (mn.current.action != null)
                mn.current.action.Invoke(mn.obj);

            if (mn.current.clearPreviousText)
                mn.text.text = "";

            if (mn.current.characterSprite != null)
                mn.novelSprite.sprite = mn.current.characterSprite;

            mn.novelAnimator.ResetTrigger(mn.current.characterSprite != null ? "Fade" : "Show");
            mn.novelAnimator.SetTrigger(mn.current.characterSprite != null ? "Show" : "Fade");

            yield return base.Start();

            while (mn.dialogueBox.localScale != Vector3.one)
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, Vector3.one, 14 * Time.deltaTime);
                yield return base.Update();
            }

            yield return new WaitForSeconds(mn.current.startDelay);

            if (mn.current.showStraightaway)
                mn.text.text += mn.current.text;

            else
                foreach (var j in mn.current.text)
                {
                    mn.textSFX.pitch = Random.Range(.95f, 1.05f);
                    mn.textSFX.Play();

                    mn.text.text += j;
                    yield return new WaitForFixedUpdate();

                    mn.textSFX.Stop();
                }

            mn.ChangeState(mn.waitingState);

            yield return base.Start();
        }

        public override IEnumerator Update()
        {
            while (true) 
            {
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 6 * Time.deltaTime);
                yield return base.Update();
            }
        }
    }

    public class WaitingState : State<DialogueSystem>
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

                    mn.variantObjects.Add(variant);
                }
            }
        }

        public override IEnumerator Update()
        {
            while (mn.current.variants == null)
            {
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 6 * Time.deltaTime);
                yield return base.Update();
            }
        }
    }
}
