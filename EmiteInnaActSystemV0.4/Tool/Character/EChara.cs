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
                if (Mathf.Abs(planed.magnitude) > movementSpeed)
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
        public static bool TargetInAttackRange(Transform transform,Collider col,SpellAttackEvent eventData)
        {
            Vector4 pos = col.transform.position;
            pos.w = 1;
            pos = transform.worldToLocalMatrix * pos;
            bool inrange = false;
            if (eventData.areaType == AreaType.CUBE)
            {
                if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "AreaAttackDebug").Value)
                    Debug.Log("Type1 at " + Time.realtimeSinceStartup);
                Vector3 pv3 = pos;
                Vector3 off = pv3 - eventData.centerOffset;
                if (Mathf.Abs(off.x) <= eventData.extends.x &&
                   Mathf.Abs(off.y) <= eventData.extends.y &&
                   Mathf.Abs(off.z) <= eventData.extends.z)
                    inrange = true;
            }
            else if (eventData.areaType == AreaType.ELLIPSE)
            {
                if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "AreaAttackDebug").Value)
                    Debug.Log("Type2 at " + Time.realtimeSinceStartup);
                Vector3 pv3 = pos;
                Vector3 off = pv3 - eventData.centerOffset;
                
                if(Mathf.Abs(off.y)<=eventData.extends.y)
                {

                    off.y = 0;
                    float cos = Vector3.Dot(off.normalized, new Vector3(0, 0, 1));
                    float ang=Mathf.Acos(cos)*180/Mathf.PI;
                    if (off.x > 0) ang =  - ang;
                    if (ang < 0) ang += 360;
                    if(ang>=eventData.angle.x-eventData.angle.y&&ang<=eventData.angle.x+eventData.angle.y||
                        ang-360 >= eventData.angle.x - eventData.angle.y && ang-360 <= eventData.angle.x + eventData.angle.y)
                    {
                        if(Mathf.Abs(off.z)*Mathf.Abs(off.z)/eventData.extends.z/eventData.extends.z+
                           Mathf.Abs(off.x) * Mathf.Abs(off.x) / eventData.extends.x / eventData.extends.x<= 1){
                            inrange = true;
                        }
                    }
                }
            }
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "AreaAttackDebug").Value)
                Debug.Log(inrange);
            return inrange;
        }
    }
}