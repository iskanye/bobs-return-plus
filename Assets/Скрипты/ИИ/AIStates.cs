using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class AIState
    {
        protected AIManager sm;
        public Coroutine UpdateCoroutine;
        public AIState(AIManager stateManager)
        {
            sm = stateManager;
        }

        public virtual IEnumerator Start()
        {
            sm.StartCoroutine(Update());
            yield break;
        }
        
        //Считать как за один проход по циклу. Если нужен сам цикл - использовать while(true)
        public virtual IEnumerator Update() 
        {
            yield break;
        }

        public virtual IEnumerator Stop()
        {
            sm.StopAllCoroutines();
            yield break;
        }
    }

    public class ChaseState : AIState
    {
        public ChaseState(AIManager stateManager) : base(stateManager) { }

        public override IEnumerator Update()
        {
            sm.AI.maxSpeed = sm.spotSpeed; //Меняем ему скорость

            while (true)
            {
                if (sm.CanSeePlayer())
                {
                    sm.AI.destination = sm.currentTarget.position; //Меняем ему цель на объект
                    sm.AI.SearchPath(); //Ищем путь до объекта
                }
                else if (sm.AI.reachedEndOfPath)
                {
                    sm.ChangeState(sm.searchState);
                }
                yield return base.Update();
            }
        }
    }

    public class PatrolState : AIState
    {
        public PatrolState(AIManager stateManager) : base(stateManager) { }

        public override IEnumerator Start()
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
            yield return base.Start();
        }


        public override IEnumerator Update()
        {
            sm.AI.maxSpeed = sm.patrolSpeed; //Меняем скорость на обычную
            sm.AI.destination = sm.path[sm.currWay]; //Назначаем ИИ путь
            sm.AI.SearchPath(); //Если надо ищем этот самый путь

            float waitTime = float.PositiveInfinity;

            //Если ИИ достиг конца пути, не ищет путь и его время ожидания не равно бесконечности, 
            //то мы назначаем ему время после которого ему надо будет идти к другой точке патруля
            while (true)
            {
                if (sm.CanSeePlayer())
                    sm.ChangeState(sm.chaseState);

                yield return base.Update();

                if (sm.AI.reachedEndOfPath && !sm.AI.pathPending && float.IsPositiveInfinity(waitTime))
                    waitTime = Time.time + Random.Range(.5f, 6);
                
                if (Time.time >= waitTime)
                { 
                    waitTime = float.PositiveInfinity;                   
                    sm.currWay++; //Обновляем путь
                    sm.currWay %= sm.path.Length; //Вычисляем остаток от деления текущего пути на длины массива путей, чтобы текущий путь не превышал кол-во путей
                    sm.AI.destination = sm.path[sm.currWay]; //Назначаем ИИ путь
                    sm.AI.SearchPath(); //Если надо ищем этот самый путь
                }

                if (sm.AI.velocity != Vector3.zero)
                    sm.direction = sm.AI.velocity.normalized;

                yield return base.Update();
            }
        }
    }

    public class SearchState : AIState
    {
        public SearchState(AIManager stateManager) : base(stateManager) { }

        public override IEnumerator Start()
        {
            float time = 0;
            Vector2 startDirection = sm.direction;
            int sign = Random.Range(0, 2) * 2 - 1; /*рандом -1 или 1*/
            while (time < 2f)
            {
                var rotation = Mathf.Lerp(0f, 360f, time / 2f);
                sm.direction = Quaternion.Euler(0, 0, sign * rotation) * startDirection;
                time += Time.deltaTime;
                if (sm.CanSeePlayer())
                {
                    sm.ChangeState(sm.chaseState);
                    yield break;
                }
                yield return base.Update();
            }
            if (sm.isPatrol)
                sm.ChangeState(sm.patrolState);

            yield return base.Update();
        }
    }
}