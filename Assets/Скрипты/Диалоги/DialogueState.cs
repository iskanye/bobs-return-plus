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
            if (mn.player != null)
                mn.player.enabled = true;

            while (true)
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, new Vector3(1, 0, 1), 10 * Time.deltaTime);
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 10 * Time.deltaTime);

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

            yield return base.Start();

            foreach (var j in mn.current.text)
            {
                mn.text.text += j;
                yield return new WaitForFixedUpdate();
            }

            mn.ChangeState(mn.waitingState);

            yield return base.Update();
        }

        public override IEnumerator Update()
        {
            while (true)
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, Vector3.one, 10 * Time.deltaTime);
                mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, new Vector3(1, 0, 1), 10 * Time.deltaTime);

                yield return base.Update();
            }
        }
    }

    public class WaitingState : State<DialogueSystem>
    {
        public WaitingState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Start()
        {
            if (mn.current.isNonlinear)
                for (int i = 0; i < mn.current.variants.Length; i++)
                {
                    var variant = Object.Instantiate(mn.variantPrefab, mn.variantBox);
                    variant.GetComponent<TMP_Text>().text = mn.current.variants[i].variant;
                    variant.GetComponent<TMP_Text>().faceColor = i == 0 ? Color.white : Color.grey;

                    mn.variantObjects.Add(variant);
                }

            yield return base.Start();
        }

        public override IEnumerator Update()
        {
            while (true) 
            {
                mn.dialogueBox.localScale = Vector3.Lerp(mn.dialogueBox.localScale, Vector3.one, 10 * Time.deltaTime);

                if (mn.current.isNonlinear) 
                    mn.variantBox.localScale = Vector3.Lerp(mn.variantBox.localScale, Vector3.one, 10 * Time.deltaTime);

                yield return base.Update(); 
            }
        }
    }
}
