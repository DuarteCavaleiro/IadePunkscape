using UnityEngine;

namespace Mannequins.Interaction_Box
{
    public class CheckInteraction : MonoBehaviour {
        [SerializeField] CheckCompletionRate mannequin;
        private void OnCollisionEnter(Collision collision) {
            Debug.Log("Found something");
            if (collision.gameObject.CompareTag("MainCamera")){
                Debug.Log("Found the player");
                mannequin.inInteractionRange= true;
            }
        }

        private void OnCollisionExit(Collision collision) {
            if (collision.gameObject.CompareTag("MainCamera") & mannequin.recording==false){
                Debug.Log("Left the player");
                mannequin.inInteractionRange= false;
                mannequin.handGestureLeft.SetActive(false);
                mannequin.handGestureRight.SetActive(false);
            }
        }
    }
}
