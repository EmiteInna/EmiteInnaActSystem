using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace EmiteInnaACT
{
    public static class EChara
    {
        public static void MoveTransform(Transform transform,Vector3 direction,float movementSpeed,float rotationSpeed)
        {
            if (direction == Vector3.zero) return;
            direction = direction.normalized;
            float angle = Vector3.Angle(transform.forward, direction);
            if (Mathf.Abs(angle) > 3f)
            {
                if (rotationSpeed > 10000)
                {
                    transform.LookAt(transform.position + direction);
                }
                else
                {
                    Vector3 cross = Vector3.Cross(transform.forward, direction);
                    if (cross.y < 0)
                    {
                        angle=-angle;
                    }
                    transform.Rotate(Vector3.up, Mathf.Clamp(angle, -rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime));

                }
            }
            else
            {
                transform.position = transform.position + direction * movementSpeed * Time.fixedDeltaTime;
            }
        }
        /// <summary>
        /// 移动角色。
        /// </summary>
        /// <param name="character"></param>
        /// <param name="direction"></param>
        /// <param name="movementSpeed"></param>
        /// <param name="rotationSpeed"></param>
        public static void CharacterMove(Rigidbody rb,Vector3 direction,float movementSpeed,float rotationSpeed,float accelerate=0.5f)
        {
            if (direction == Vector3.zero) return;
            direction = direction.normalized;
            float angle = Vector3.Angle(rb.gameObject.transform.forward, direction);
            if (Mathf.Abs(angle) > 3f)
            {
                if (rotationSpeed > 10000)
                {
                    rb.gameObject.transform.LookAt(rb.gameObject.transform.position + direction);
                }
                else
                {
                    Vector3 cross = Vector3.Cross(rb.gameObject.transform.forward, direction);
                    if (cross.y < 0)
                    {
                        angle = -angle;
                    }
                    rb.gameObject.transform.Rotate(Vector3.up, Mathf.Clamp(angle, -rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime));

                }
            }
            else
            {

                rb.velocity = rb.velocity + direction * accelerate;
                Vector3 planed = rb.velocity;
                planed.y = 0;
                if (Mathf.Abs(planed.sqrMagnitude) > movementSpeed)
                {
                    
                    planed = planed.normalized * movementSpeed;
                    planed.y = rb.velocity.y;
                    rb.velocity = planed;
                }
                //character.gameObject.transform.position = character.gameObject.transform.position + direction * movementSpeed * Time.fixedDeltaTime;
            }
            
        }
        /// <summary>
        /// 角色跳跃。
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="jumpHeight"></param>
        public static void CharacterJump(Rigidbody rb,float jumpHeight)
        {
            float velo = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
            
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + velo, rb.velocity.z);
        }
    }
}