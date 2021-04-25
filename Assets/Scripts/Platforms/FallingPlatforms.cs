using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class FallingPlatforms : PlatformManager
    {
        [SerializeField] protected enum TypesOfFallingPlatforms 
        { 
            Destructive,
            Donut
        }

        [SerializeField] protected TypesOfFallingPlatforms platformType;
        [SerializeField] protected float timeTillDoSomething;
        [SerializeField] protected float timeFalling;
        [SerializeField] protected float timeTillReset;
        [SerializeField] protected bool destroyPlatform;

        protected Vector3 originalPlaformPosition;
        protected float currentTimeTillDoSomething;
        protected float currentTimeFalling;
        protected bool platformFalling;
        protected bool destructivePlatform;

        protected override void Initialization()
        {
            base.Initialization();
            currentTimeTillDoSomething = timeTillDoSomething;
            currentTimeFalling = timeFalling;
            originalPlaformPosition = transform.position;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            PlatformCheck();
            PlatformFalling();
        }

        protected virtual void PlatformCheck()
        {
            if (CollisionCheck() || destructivePlatform)
            {
                if(platformType == TypesOfFallingPlatforms.Destructive)
                {
                    destructivePlatform = true;
                    DestructivePlatform();
                }
                    
                if (platformType == TypesOfFallingPlatforms.Donut)
                    DonutPlatform();
            }

            if (!CollisionCheck() && platformType == TypesOfFallingPlatforms.Donut)
                currentTimeTillDoSomething = timeTillDoSomething;
        }

        protected virtual void DestructivePlatform()
        {
            currentTimeTillDoSomething -= Time.deltaTime;

            if(currentTimeTillDoSomething < 0)
            {
                platformCollider.enabled = false;
                platformFalling = true;
            }
        }

        protected virtual void DonutPlatform()
        {
            currentTimeTillDoSomething -= Time.deltaTime;

            if (currentTimeTillDoSomething < 0)
            {
                platformFalling = true;
            }
        }

        protected virtual void PlatformFalling()
        {
            if (platformFalling)
            {
                currentTimeFalling -= Time.deltaTime;
                platformRb2d.bodyType = RigidbodyType2D.Dynamic;
                platformRb2d.constraints = RigidbodyConstraints2D.None;
                platformRb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

                if(currentTimeFalling < 0 && !destroyPlatform)
                {
                    gameObject.SetActive(false);
                    Invoke("PutPlatformBack", timeTillReset);
                }

                if (currentTimeFalling < 0 && destroyPlatform)
                {
                    Destroy(gameObject);
                }
            }
        }

        protected virtual void PutPlatformBack()
        {
            platformRb2d.bodyType = RigidbodyType2D.Kinematic;
            platformRb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            currentTimeTillDoSomething = timeTillDoSomething;
            currentTimeFalling = timeFalling;
            platformFalling = false;
            destructivePlatform = false;
            platformCollider.enabled = true;
            transform.position = originalPlaformPosition;
            gameObject.SetActive(true);
        }
    }

}
