namespace AbilityEditor
{
    using UnityEngine;
    using System.Collections;
    [ExecuteInEditMode]
    public class SelfRange : MonoBehaviour
    {

        public float BodyRadius = 0;
        public float BodyHeight = 0;
        public float Scale = 1;
        public Transform CharacterTransform;
        public Animator Animator;
        public MeshCollider mc = null;
        public float HitTime = -1;

        private Renderer _renderer;
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            mc = GetComponent<MeshCollider>();
            if (mc == null)
            {
                mc = gameObject.AddComponent<MeshCollider>();
                mc.convex = true;
                mc.isTrigger = true;
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
            }
            CharacterTransform = transform.parent;
            var scale = Scale == 0 ? 1 : Scale;
            transform.localScale = new Vector3(BodyRadius * 2 / scale, BodyHeight / scale, BodyRadius * 2 / scale);
            transform.position = new Vector3(CharacterTransform.position.x, CharacterTransform.position.y + BodyHeight / 2, CharacterTransform.position.z);
            Animator = CharacterTransform.GetComponent<Animator>();
            SetColor(Color.yellow);
        }

        public void SetRangeActive(bool state)
        {
            _renderer.enabled = state;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "WeaponCollider")
            {
                //CinemaDirector.RangeAttack ra = other.gameObject.GetComponent<CinemaDirector.RangeAttack>();
                //if (ra != null)
                //{
                //    if (!ra.Attacked(gameObject))
                //    {
                //        ra.AddBeAttacker(gameObject);
                //        Debug.Log(string.Format("OnTriggerEnter ~~~~~~~~~~~~~~~{0} is be attack!", other.gameObject.name));
                //        if (Animator != null)
                //        {
                //            Animator.Play("hit");
                //        }
                //        SetColor(Color.red);
                //        BeAttackEffectManager.Instance.Add(ra.m_BeAttackEffectItem);
                //        HitTime = Time.time;
                //    }
                //}

            }
        }


        //public void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.tag == "WeaponCollider")
        //    {
        //        Debug.Log(string.Format("OnTriggerExit ~~~~~~~~~~~~~~~{0}!", other.gameObject.name));
        //        SetColor(Color.yellow);
        //    }
        //}

        public void SetColor(Color color)
        {
            //Debug.Log("SetColor" + color);
            color.a = 0.3f;
            //gameObject.renderer.material.color = color;

            if (gameObject.GetComponent<Renderer>().sharedMaterial.HasProperty("_Color"))
            {
                gameObject.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
            }
        }
    }
}