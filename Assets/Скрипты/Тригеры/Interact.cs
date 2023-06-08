using UnityEngine;
using System;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : ActionBase
{   
    [SerializeField] private LayerMask playerMask; //Слой игрока
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    const float selectedBrightness = .7f;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (action != null && ((1 << c.gameObject.layer) | playerMask) == playerMask)
        {            
            var mn = c.GetComponent<InteractionController>();

            if (mn.currentInteraction.Item2 == null || (mn.currentInteraction.Item2.position - mn.transform.position).sqrMagnitude >
                (transform.position - mn.transform.position).sqrMagnitude)
            {    
                mn.currentInteraction = (action, transform);
                Array.ForEach(spriteRenderers, (i) => i.color = new Color(selectedBrightness, selectedBrightness, selectedBrightness, i.color.a));
            }
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (action != null && ((1 << c.gameObject.layer) | playerMask) == playerMask)
        {            
            Array.ForEach(spriteRenderers, (i) => i.color = new Color(1, 1, 1, i.color.a));
            var mn = c.GetComponent<InteractionController>();

            if (mn.currentInteraction.Item2 == transform)
                mn.currentInteraction = (null, null);
        }
    }
}
