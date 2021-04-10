using UnityEngine;

namespace Arif.Scripts
{
    public class Choice : MonoBehaviour
    {
        public Transform cardTransform;
        private CardSO _myChoiceProfile;
        
        public void DetermineChoice()
        {
            _myChoiceProfile = GameManager.instance.choiceCardList[Random.Range(0, GameManager.instance.choiceCardList.Count)];
            var clone =GameManager.instance.BuildAndGetCard(_myChoiceProfile.myID,cardTransform);
            GameManager.instance.choiceContainer.Add(clone);
        }

        public void OnChoice()
        {
            GameManager.instance.myDeckIDList.Add(_myChoiceProfile.myID);
            foreach (var cardBase in GameManager.instance.choiceContainer)
            {
                Destroy(cardBase);
            }
            GameManager.instance.NextLevel();
        }
    }
}
