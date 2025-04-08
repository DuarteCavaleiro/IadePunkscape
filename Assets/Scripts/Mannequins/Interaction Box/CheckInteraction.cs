using UnityEngine;

namespace Mannequins.Interaction_Box
{
    public class CheckInteraction : MonoBehaviour {
        [SerializeField] CheckCompletionRate mannequin;
        private void OnTriggerEnter(Collider collision) {
            if (collision.gameObject.CompareTag("Player")) {
                mannequin.inInteractionRange= true;
                mannequin.handGestureLeft.SetActive(true);
                mannequin.handGestureRight.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider collision) {
            if (collision.gameObject.CompareTag("Player") && mannequin.recording==false){
                mannequin.inInteractionRange= false;
                mannequin.handGestureLeft.SetActive(false);
                mannequin.handGestureRight.SetActive(false);
            }
        }
    }
}
