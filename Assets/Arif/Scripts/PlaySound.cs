using UnityEngine;

namespace Arif.Scripts
{
    public class PlaySound : MonoBehaviour
    {
        public SoundProfile myProfile;

        public void PlaySfx()
        {
            AudioManager.instance.PlayOneShot(myProfile.GetRandomClip());
        }

        public void PlayButton()
        {
            AudioManager.instance.PlayOneShotButton(myProfile.GetRandomClip());
        }
    }
}
