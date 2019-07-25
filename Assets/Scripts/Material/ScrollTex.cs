using UnityEngine;

namespace Assets.Scripts.Material
{
    public class ScrollTex : MonoBehaviour
    {

        public float scrollX = 0.05f;
        public float scrollY = 0.05f;

        // Update is called once per frame
        void Update()
        {
            float OffsetX = Time.time * scrollX;
            float OffsetY = Time.time * scrollY;
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        }
    }
}
