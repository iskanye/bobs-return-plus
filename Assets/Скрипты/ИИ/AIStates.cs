using System.Collections;
using UnityEngine;

namespace AI
{
    public class IdleState : State<AIManager> 
    {
        public IdleState(AIManager stateManager) : base(stateManager) { }

        public override IEnumerator Update()
        {
            while (true)
            {
                if (mn.CanSeePlayer())
                {
                    mn.ChangeState(mn.chaseState);
                    yield break;
                }

                if (mn.isPatrol)
                {
                    mn.ChangeState(mn.patrolState);
                    yield break;
                }

                yield return base.Update();
            }
        }
    }

    public class ChaseState : State<AIManager>
    {
        public ChaseState(AIManager stateManager) : base(stateManager) { }

        public override IEnumerator Update()
        {
            mn.AI.maxSpeed = mn.spotSpeed; //Меняем ему скорость
            mn.AI.destination = mn.currentTarget.position; //Меняем ему цель на объект
            mn.AI.SearchPath(); //Ищем путь до объекта

            while (true)
            {
                if (mn.CanSeePlayer())
                {
                    mn.AI.destination = mn.currentTarget.position; //Меняем ему цель на объект
                    mn.AI.SearchPath(); //Ищем путь до объекта
                }

                else if (mn.AI.reachedEndOfPath)
                    mn.ChangeState(mn.searchState);

                if (mn.AI.velocity != Vector3.zero)
                    mn.direction = mn.AI.velocity.normalized;

                yield return base.Update();
            }
        }
    }

    public class PatrolState : State<AIManager>
    {
        public PatrolState(AIManager stateManager) : base(stateManager) { }

        public override IEnumerator Update()
        {
            mn.AI.maxSpeed = mn.speed; //Меняем скорость на обычную

            float waitTime = Random.Range(.5f, mn.patrolDelay);
            float time = 0;

            //Если ИИ достиг конца пути, не ищет путь и его время ожидания не равно бесконечности, 
            //то мы назначаем ему время после которого ему надо будет идти к другой точке патруля
            while (true)
            {
                if (mn.CanSeePlayer())
                {
                    mn.ChangeState(mn.chaseState);
                    yield break;
                }
                
                time += Time.deltaTime;                

                if (mn.AI.reachedEndOfPath && !mn.AI.pathPending && float.IsPositiveInfinity(waitTime))
                    waitTime = time + Random.Range(.5f, mn.patrolDelay);
                
                if (time >= waitTime)
                { 
                    waitTime = float.PositiveInfinity;                   
                    mn.currWay++; //Обновляем путь
                    mn.currWay %= mn.path.Length; //Вычисляем остаток от деления текущего пути на длины массива путей, чтобы текущий путь не превышал кол-во путей
                    mn.AI.destination = mn.path[mn.currWay]; //Назначаем ИИ путь
                    mn.AI.SearchPath(); //Если надо ищем этот самый путь
                }

                if (mn.AI.velocity != Vector3.zero)
                    mn.direction = mn.AI.velocity.normalized;

                yield return base.Update();
            }
        }
    }

    public class SearchState : State<AIManager>
    {
        public SearchState(AIManager stateManager) : base(stateManager) { }

        public override IEnumerator Start()
        {
            float time = 0;
            Vector2 startDirection = mn.direction;
            int sign = Random.Range(0, 2) * 2 - 1; /*рандом -1 или 1*/
            
            while (time < 1 / mn.searchSpeed)
            {
                var rotation = Mathf.Lerp(0f, 360f, time * mn.searchSpeed);
                mn.direction = Quaternion.Euler(0, 0, sign * rotation) * startDirection;
                time += Time.deltaTime;

                if (mn.CanSeePlayer())
                {
                    mn.ChangeState(mn.chaseState);
                    yield break;
                }

                yield return base.Update();
            }

            if (mn.isPatrol)
                mn.ChangeState(mn.patrolState);

            else
                mn.ChangeState(mn.idleState);

            yield return base.Update();
        }
    }
}