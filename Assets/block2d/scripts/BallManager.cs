using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
namespace AnyMind
{
    public class BallManager : MonoBehaviour
    {
        #region Serialization Field
        [Header("DEBUG")]
        [SerializeField]
        AudioSource DebugAudioTransient;
        [SerializeField]
        AudioSource DebugAudioContinuous;
        [SerializeField]
        MMUIShaker Logo;
        [Header("Ball")]
        [SerializeField]
        Vector2 Gravity = new Vector2(0, -50f);
		#endregion
		#region MonoBehaviour Callback
		// Start is called before the first frame update
		void Start()
        {
            Physics2D.gravity = Gravity;
        }
		#endregion
	}
}

