using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        BallPusher _ballPusher;
        [SerializeField]
        BallController _ball;

        [SerializeField]
        LifeIcon[] _arrLifeIcon;
        [Header("Ball")]
        [SerializeField]
        Vector2 Gravity = new Vector2(0, -50f);
        [SerializeField]
        int MaxLife = 3;
        [SerializeField]
        int CurrentLife = 3;
        [SerializeField]
        GameObject _objWaiting;
        [SerializeField]
        Text _txtWaiting;
        [SerializeField]
        TouchZoneHandler _touchZoneHandler;
        #endregion
        #region Private Field
        float _fWaitCount = 0;
        float _fWaitDuration = 3f;
		#endregion
		#region MonoBehaviour Callback
		// Start is called before the first frame update
		void Start()
        {
            Physics2D.gravity = Gravity;

        }

		private void Update()
		{
            CountWaitingTime();

        }

		#endregion
		#region Private Method
        void CountWaitingTime()
		{
            if (!_objWaiting.active)
			{
                return;
			}
            if (_fWaitCount < _fWaitDuration)
			{
                _fWaitCount += Time.deltaTime;
                _txtWaiting.text = (int)(_fWaitDuration - _fWaitCount + 1) + "Sec";
                if (_fWaitCount >= _fWaitDuration)
				{
                    _objWaiting.SetActive(false);
                    _fWaitCount = 0;
                    _txtWaiting.text = "";
                    _touchZoneHandler.PauseListener = false;
				}
			}
		}
		#endregion
		#region Public Methods
		public void LoseLife()
		{
            CurrentLife--;
            if (CurrentLife > -1)
			{
                _arrLifeIcon[CurrentLife].LoseLife();
            }

            if (CurrentLife == 0)
			{
                //handle die 
			} else
			{
                _touchZoneHandler.PauseListener = true;
                _objWaiting.SetActive(true);
                _ball.InitBallController();
                _ballPusher.InitBallPusher();
			}
		}

        public void RestartLevel()
		{

		}
		#endregion
	}
}

