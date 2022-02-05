using System.Collections;
using UnityEngine;

namespace Dialogues
{
    public class IdleState : State<DialogueSystem>
    {
        public IdleState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Update()
        {
            mn.player.enabled = true;

            while (true)
            {
                mn.dialogueBox.transform.localScale = Vector3.Lerp(mn.dialogueBox.transform.localScale, Vector3.zero, .2f);
                yield return base.Update();
            }
        }
    }

    public class PrintingState : State<DialogueSystem>
    {
        public PrintingState(DialogueSystem sys) : base(sys) { }

        public override IEnumerator Start()
        {
            yield return base.Start();

            mn.player.enabled = false;
            mn.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            mn.current = mn.dialogues[mn.index];

            mn.character.text = mn.current.character;

            if (mn.current.action != null) 
                mn.current.action.Invoke();

            mn.text.text = "";

            foreach (var j in mn.current.text)
            {
                mn.text.text += j;
                yield return new WaitForFixedUpdate();
            }

            mn.ChangeState(mn.waitingState);
        }

        public override IEnumerator Update()
        {
            while (true)
            {
                mn.dialogueBox.transform.localScale = Vector3.Lerp(mn.dialogueBox.transform.localScale, Vector3.one, .2f);
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
                mn.dialogueBox.transform.localScale = Vector3.Lerp(mn.dialogueBox.transform.localScale, Vector3.one, .2f);

                if (!mn.current.isNonlinear && Input.GetKeyDown(KeyCode.Space))
                {
                    mn.index++;
                    if (mn.index >= mn.dialogues.Length)
                    {
                        mn.ChangeState(mn.idleState);
                        yield break;
                    }
                    mn.ChangeState(mn.printingState);
                }

                yield return base.Update(); 
            }
        }
    }
}
