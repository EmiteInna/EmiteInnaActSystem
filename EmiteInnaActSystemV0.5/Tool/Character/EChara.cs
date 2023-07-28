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
        public static void CharacterMove(Rigidbody rb,Vector3 direction,float movementSpeed,float rotationSpeed,float accelerate=4f)
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
        /// 使用RootMotion中的属性来进行移动。
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="direction"></param>
        /// <param name="movementSpeed"></param>
        /// <param name="rotationSpeed"></param>
        /// <param name="accelerate"></param>
        public static void CharacterMoveWithRootMotion(Rigidbody rb,CharacterAnimationController anim,Vector3 direction,float movementSpeed,float rotationSpeed,float accelerate = 4f)
        {
            anim.animator.applyRootMotion = false;
            Vector3 fwd = anim.animator.deltaPosition;
            Quaternion rtt = anim.animator.deltaRotation;
           // Debug.Log(fwd);
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
        /// <summary>
        /// 角色长跳跃。
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="jumpHeight"></param>
        public static void CharacterLongJump(CharacterControllerBase ch, float jumpHeight)
        {
            float velo = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
            StartSpawnIllusionsInSecond(ch, 0.3f, 0.5f, 4);
            ch.rb.velocity = new Vector3(ch.rb.velocity.x, ch.rb.velocity.y + velo, ch.rb.velocity.z);
        }
        /// <summary>
        /// 判断目标是否在Attack攻击范围盒里。
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="col"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取目标角色脚下的地面点的位置。
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Vector3 GetGroundPointUnderTransform(Transform transform)
        {
            Ray ray = new Ray(transform.position,Vector3.down);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("TERRAIN")))
            {
                return hit.point;
            }
            return new Vector3(0, -114514, 0);
        }
        /// <summary>
        /// 获取目标角色脚下的地面点的位置。
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Vector3 GetGroundPointUnderTransform(Transform transform, Vector3 offset)
        {
            Ray ray = new Ray(transform.position, Vector3.down+offset);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("TERRAIN")))
            {
                return hit.point;
            }
            return new Vector3(0, -114514, 0);
        }
        /// <summary>
        /// 角色向某个位置冲刺，无法穿墙，冲刺的时候不受重力影响，冲刺结束之后回到Idle状态。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="destination"></param>
        /// <param name="time"></param>
        public static void CharacterDashToPoint(CharacterControllerBase ch,Vector3 destination,float time)
        {
            ch.StartCoroutine(DoDashToPoint(ch, destination, time));
        }
        public static IEnumerator DoDashToPoint(CharacterControllerBase ch,Vector3 destination,float time)
        {
            ch.PlayAnimation(ch.configure.DashAnimation,0,0);
            StartSpawnIllusionsInSecond(ch, time, 0.5f, 4);
            ch.rb.useGravity = false;
            float ms = ch.rb.drag;
            ch.rb.drag = 0;
            ch.EnterState("Casting");
            time += 0.00001f;
            Vector3 delta = destination - ch.transform.position;
            Vector3 lookat = ch.transform.position + delta;
            lookat.y = ch.transform.position.y;
            ch.transform.LookAt(lookat);
            Vector3 velo = delta / time;
            float timer = 0;
            while (timer < time)
            {
                ch.rb.velocity = velo;
                Debug.Log(velo);
                timer += Time.fixedDeltaTime;
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            ch.rb.useGravity = true;
            ch.rb.drag = ms;
            ch.rb.velocity = Vector3.zero;
            ch.EnterState("Idle");
            yield return null;
        }
        /// <summary>
        /// 在若干秒之内生成若干个该角色的幻象，每个持续一定时间。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="duration"></param>
        /// <param name="count"></param>
        public static void StartSpawnIllusionsInSecond(CharacterControllerBase ch,float duration,float illusionDuration,int count)
        {
            ch.StartCoroutine(DoStartSpawnIllusionInSecond(ch, duration, illusionDuration, count));
        }
        public static IEnumerator DoStartSpawnIllusionInSecond(CharacterControllerBase ch, float duration,float illusionDuration, int count)
        {
            float timer = 0;
            int cnt = -1;
            while (timer < duration*count)
            {
                int cntnow = (int)(timer / duration);
                if (cntnow > cnt)
                {
                    cnt++;
                    SpawnCharacterIllusion(ch, illusionDuration);
                }
                timer += Time.fixedDeltaTime * count;
                yield return CoroutineTool.WaitForFixedUpdate();
            }
        }
        /// <summary>
        /// 在某个位置生成某个角色的幻象。
        /// 默认原物体的skinnedMeshRenderer产生在物体View层的第二个儿子处。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="duration"></param>
        public static void SpawnCharacterIllusion(CharacterControllerBase ch,float duration)
        {
            Material m = ConfigureInstance.GetValue<Material>("material", "illusion");
            Mesh mesh = EPoolInstance.Get<Mesh>("NewMesh", false);
            SkinnedMeshRenderer sk = ch.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            sk.BakeMesh(mesh);
            GameObject illusion = EPoolInstance.Get<GameObject>("Illusions", true);
            illusion.transform.position = ch.transform.position;
            illusion.transform.rotation = ch.transform.rotation;
            illusion.transform.localScale = ch.transform.localScale;
            illusion.GetComponent<MeshFilter>().mesh = mesh;
            Material mat = illusion.GetComponent<MeshRenderer>().sharedMaterial;
            mat.SetFloat("_Transparency", 1);
            ch.StartCoroutine(DoSpawnIllusion(illusion, mat, mesh, duration));
        }
        public static IEnumerator DoSpawnIllusion(GameObject g,Material mat,Mesh mesh,float duration)
        {
            float timer = duration;
            while (timer >= 0)
            {
                float ratio = timer / duration;
                timer -= Time.fixedDeltaTime;
                mat.SetFloat("_Transparency", ratio);
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            EPoolInstance.Push(g, "Illusions", true);
            EPoolInstance.Push(mesh, "NewMesh",false);
        }
        /// <summary>
        /// 判断Transform是否在地面上。
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsTransformGrounded(Transform transform,Collider col)
        {
            float rc = 0.05f;
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("TERRAIN")))
            {
                float dy = transform.position.y - hit.point.y;
                if (dy <= col.bounds.extents.y+rc)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}