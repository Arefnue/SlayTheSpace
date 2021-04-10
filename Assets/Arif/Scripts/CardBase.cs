using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arif.Scripts
{
    public class CardBase : MonoBehaviour
    {
        [Tooltip("Mana required to use card")]
        [HideInInspector]public CardSO myProfile;
        
       
        public MeshRenderer meshRenderer;
        public Material material;

        private Vector2 _dissolveOffset = new Vector2(0.1f, 0);
        private Vector2 _dissolveSpeed = new Vector2(2f, 2f);
        private Color _dissolveColor;
        private Color _color;
        private Color _color2;

        [Header("Texts")] 
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descText;
        public TextMeshProUGUI manaText;
        public Image frontImage;
        public Image backImage;
        
        
        private bool isInactive;
        
        public void SetCard()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            material = meshRenderer.material; // Create material instance

            _color = material.GetColor("_Color");
            _color2 = material.GetColor("_OutlineColor");
            _dissolveColor = material.GetColor("_DissolveColor");
            
            nameText.text = myProfile.myName;
            descText.text = myProfile.myDescription;
            manaText.text = myProfile.myManaCost.ToString();
            frontImage.sprite = myProfile.mySprite;
        }
        
        
        public void Use() 
        {
            LevelManager.instance.DiscardCard(this);
            StartCoroutine("Dissolve");
        }

        public void Attack(EnemyBase targetEnemy)
        {
            LevelManager.instance.DiscardCard(this);
            StartCoroutine("Dissolve");
        }

        public void Discard()
        {
            LevelManager.instance.DiscardCard(this);
            StartCoroutine("Dissolve");
        }
        
        protected IEnumerator Dissolve() {
            Vector2 t = Vector2.zero - _dissolveOffset;
            while (t.x < 1) {
                t.x = (t.x + Time.deltaTime * _dissolveSpeed.x);
                if (t.y < 1) {
                    t.y = (t.y + Time.deltaTime * _dissolveSpeed.y);
                }
                material.SetVector("_Dissolve", t);
                material.SetColor("_DissolveColor", _dissolveColor * 4 * t.y);
                yield return null;
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// Use to swap the card material to an inactiveMaterial. If true should pass in the inactiveMaterial. false resets it to the regular card material so inactiveMaterial can be null.
        /// </summary>
        public void SetInactiveMaterialState(bool isInactive, Material inactiveMaterial = null) {
            if (isInactive == this.isInactive) {
                return; // No change
            }
            this.isInactive = isInactive;
            if (isInactive) {
                
                // Switch to Inactive Material
                meshRenderer.sharedMaterial = inactiveMaterial;
            } else {

                // Switch back to normal Material
                meshRenderer.sharedMaterial = material;
            }
        }
        
        public void OnDestroy() {
            if (material != null) Destroy(material);
        }
    }
}