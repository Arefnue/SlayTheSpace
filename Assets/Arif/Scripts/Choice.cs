using UnityEngine;

namespace Arif.Scripts
{
    public class Choice : MonoBehaviour
    {
        public Transform cardTransform;
        private CardSO _myChoiceProfile;
        
        public void DetermineChoice()
        {
            do
            {
                _myChoiceProfile = GameManager.instance.choiceCardList[Random.Range(0, GameManager.instance.choiceCardList.Count)];
                if (LevelManager.instance._sameChoiceContainer.Count>= GameManager.instance.choiceCardList.Count)
                {
                    break;
                }
            } while (LevelManager.instance._sameChoiceContainer.Contains(_myChoiceProfile.myID));
            
            LevelManager.instance._sameChoiceContainer.Add(_myChoiceProfile.myID);
            var clone =GameManager.instance.BuildAndGetCard(_myChoiceProfile.myID,cardTransform);
            GameManager.instance.choiceContainer.Add(clone);
        }

        public void OnChoice()
        {
            GameManager.instance.myDeckIDList.Add(_myChoiceProfile.myID);
            LevelManager.instance._sameChoiceContainer.Clear();
            foreach (var cardBase in GameManager.instance.choiceContainer)
            {
                Destroy(cardBase);
            }
            GameManager.instance.NextLevel();
        }
    }
}
