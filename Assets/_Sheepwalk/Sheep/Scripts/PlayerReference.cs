using UnityEngine;

namespace sheepwalk
{
    public class PlayerReference : MonoBehaviour
    {
        private static CharacterMovement _playerController;

        // Start is called before the first frame update
        public static CharacterMovement PlayerController
        {
            get
            {
                if (_playerController == null) _playerController = FindObjectOfType<CharacterMovement>();
                return _playerController;
            }
        }
    }
}
