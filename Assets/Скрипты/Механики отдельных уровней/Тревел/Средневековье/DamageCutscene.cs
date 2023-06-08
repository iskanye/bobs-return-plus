using UnityEngine;

public class DamageCutscene : MonoBehaviour
{
    public UninteractiveDialogueActivator dialogueActivator;
    public PlayerWarp playerWarp;
    public GameObject borders;
    public UninteractiveDialogue[] attempts = new UninteractiveDialogue[6];
    public UninteractiveDialogue gotDamage;

    int attempt;

    int StopCutscene(int lives)
    {        
        borders.SetActive(false);
        playerWarp.player.data.lives.livesCalculation = null;
        dialogueActivator.dialogues = gotDamage;
        dialogueActivator.Dialogue();
        return lives;
    }

    public void StartCutscene()
    {        
        borders.SetActive(true);
        playerWarp.player.data.lives.livesCalculation += StopCutscene;
    }

    public void EscapeAttempt()
    {
        attempt++;

        if (attempt >= 1 && attempt < 4)
            dialogueActivator.dialogues = attempts[attempt - 1];

        else if (attempt >= 4 && attempt < 7)
            dialogueActivator.dialogues = attempts[3];

        else if (attempt >= 7 && attempt < 10)
            dialogueActivator.dialogues = attempts[4];

        else if (attempt == 10)
        {
            dialogueActivator.dialogues = attempts[5];
            borders.SetActive(false);
            playerWarp.player.data.lives.livesCalculation -= StopCutscene;
        }
        
        dialogueActivator.Dialogue();

        if (attempt == 3)
            StartCoroutine(ThirdAttempt());
    }

    System.Collections.IEnumerator ThirdAttempt()
    {
        yield return DialogueSystem.Active.Sequence();

        var playerMov = playerWarp.player.data.movement;
        var anim1 = playerWarp.player.GetComponent<AnimationMovementController>();
        var anim2 = playerWarp.player.transform.GetChild(0).GetComponent<AnimationMovementController>();
        var newMov = playerWarp.player.gameObject.AddComponent<SimpleMovement>();

        anim1.movingObject = newMov;
        anim2.movingObject = newMov;
        newMov.path = new Vector2[] {transform.position};
        newMov.speed = 5;

        void Stop()
        {
            anim1.movingObject = playerMov;
            anim2.movingObject = playerMov;
            Destroy(newMov);
            playerWarp.Enabled = true;
        }
        
        playerWarp.player.data.lives.livesCalculation -= StopCutscene;
        playerWarp.player.data.lives.livesCalculation += i => 
        {
            StopAllCoroutines();
            playerWarp.player.data.lives.livesCalculation = null;
            Stop();
            return StopCutscene(i);
        };
        
        newMov.StartMove();

        yield return new WaitForSeconds(.1f);    
        yield return new WaitForEndOfFrame();
        playerWarp.Enabled = false;

        yield return new WaitUntil(() => Input.anyKeyDown);

        Stop();        
        playerWarp.player.data.lives.livesCalculation = null;
        playerWarp.player.data.lives.livesCalculation += StopCutscene;
    }
}
