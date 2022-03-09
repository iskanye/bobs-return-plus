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
                mn.current.action.Invoke();

            if (mn.current.clearPreviousText) 
                mn.text.text = "";

            while (true)
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, Vector3.one, 8 * Time.deltaTime);
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 6 * Time.deltaTime);

                if (mn.dialogueBox.localScale == Vector3.one)
                    break;

                yield return base.Update();
            }

            foreach (var j in mn.current.text)
            {
                mn.text.text += j;
                yield return new WaitForFixedUpdate();
            }

            mn.ChangeState(mn.waitingState);

            yield return base.Start();
        }
    }

    public class WaitingState : State<DialogueSystem>
    {
        public WaitingState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Start()
        {
            yield return base.Start();

            if (mn.current.isNonlinear)
            {
                mn.variantBox.sizeDelta = new Vector2(mn.variantBox.sizeDelta.x, 28 * mn.current.variants.Length);

                while (true)
                {
                    mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, Vector3.one, 6 * Time.deltaTime);

                    if (mn.variantBox.localScale == Vector3.one)
                        break;
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
            while (true) 
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, Vector3.one, 8 * Time.deltaTime);
                yield return base.Update(); 
            }
        }
    }
}
