using System.Collections;
using UnityEngine;

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
                mn.dialogueBox.transform.localScale = Vector3.Lerp(mn.dialogueBox.transform.localScale, new Vector3(1, 0, 1), 10 * Time.deltaTime);
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

            mn.firstVariant.gameObject.SetActive(false);
            mn.secondVariant.gameObject.SetActive(false);

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
                mn.dialogueBox.transform.localScale = Vector3.Lerp(mn.dialogueBox.transform.localScale, Vector3.one, 10 * Time.deltaTime);
                yield return base.Update();
            }
        }
    }

    public class WaitingState : State<DialogueSystem>
    {
        public WaitingState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Update()
        {
            mn.firstVariant.gameObject.SetActive(mn.current.isNonlinear);
            mn.firstVariantText.text = mn.current.firstVariant;

            mn.secondVariant.gameObject.SetActive(mn.current.isNonlinear);
            mn.secondVariantText.text = mn.current.secondVariant;

            while (true) 
            {
                mn.dialogueBox.transform.localScale = Vector3.Lerp(mn.dialogueBox.transform.localScale, Vector3.one, 10 * Time.deltaTime);
                yield return base.Update(); 
            }
        }
    }
}
