using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class MovablePlatform : PlatformManager
    {
        [SerializeField] protected enum PlatformTypes 
        { 
            Ascending,
            PingPong,
            StopOnEnd
        }

        [SerializeField] protected PlatformTypes platformTypes;
        public List<Vector3> numberOfPaths = new List<Vector3>();
        [SerializeField] protected float speed;
        protected bool needToMove = true;
        protected bool moving;
        protected bool pingPongGoingDown;
        protected int currentPath = 0;
        protected int nextPath;

        protected override void Initialization()
        {
            base.Initialization();
            transform.position = numberOfPaths[currentPath];
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            FindThePath();
            MoveToPosition();
        }

        protected virtual void FindThePath()
        {
            for (int i = currentPath; i < numberOfPaths.Count; i++)
            {
                if (needToMove)
                {
                    needToMove = false;

                    if (!pingPongGoingDown)
                        currentPath = i;

                    else
                        currentPath = i - 2;

                    if(platformTypes == PlatformTypes.Ascending)
                    {
                        nextPath = i + 1;

                        if(nextPath == numberOfPaths.Count)
                        {
                            nextPath = 0;
                        }
                    }

                    if(platformTypes == PlatformTypes.PingPong)
                    {
                        if (!pingPongGoingDown)
                        {
                            nextPath = i + 1;

                            if(nextPath == numberOfPaths.Count)
                            {
                                nextPath = i - 2;
                                currentPath--;
                                pingPongGoingDown = true;
                            }
                        }

                        if (pingPongGoingDown)
                        {
                            nextPath = i - 1;

                            if(nextPath == 0)
                            {
                                nextPath = 0;
                                currentPath = 1;
                                pingPongGoingDown = false;
                            }
                        }
                    }

                    if(platformTypes == PlatformTypes.StopOnEnd)
                    {
                        nextPath = i + 1;

                        if(nextPath == numberOfPaths.Count)
                            return;
                    }

                    moving = true;
                }
            }
        }

        protected virtual void MoveToPosition()
        {
            if (moving)
            {
                if(transform.position == numberOfPaths[nextPath])
                {
                    moving = false;
                    needToMove = true;
                    currentPath ++;
                }

                if (transform.position == numberOfPaths[nextPath] && currentPath == numberOfPaths.Count)
                    currentPath = 0;

                    transform.position = Vector2.MoveTowards(transform.position, numberOfPaths[nextPath], speed * Time.deltaTime);
            }
        }
    }

}
