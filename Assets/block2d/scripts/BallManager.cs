using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
namespace AnyMind
{
    /**************************************************
     * BallManager is the overall level manager script.
     * It initialize the scene components such as ball,
     * ball pusher, touchZone etc and instantiate the
     * bricks from brick spawners. 
     * It also handles play state such as life lose, 
     * fail, success, quiting game etc.
     * ************************************************/
    public class BallManager : MonoBehaviour
    {
        #region Serialization Field
        [Header("Scene Level")]
        [SerializeField]
        int nLevel;
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
        [SerializeField]
        GameObject _objFailPanel;
        [SerializeField]
        GameObject _objSuccessPanel;
        [SerializeField]
        BrickSpawner[] _arrBrickSpawnPoints;
        #endregion
        #region Private Field
        float _fWaitCount = 0;
        float _fWaitDuration = 3f;
        List<BrickUnit> _brickList = new List<BrickUnit>();
        bool _isFinished = false;
		#endregion
		#region MonoBehaviour Callback
		// Start is called before the first frame update
		void Start()
        {
            Physics2D.gravity = Gravity;
            SpawnBricks();
        }

		private void Update()
		{
            CountWaitingTime();
        }

		#endregion
		#region Private Method
        //Waiting time counting whenever losing life and spawn with new life
        void CountWaitingTime()
		{
            if (!_objWaiting.active)
			{
                return;
			}
            if (_fWaitCount < _fWaitDuration)
			{
                _fWaitCount += Time.deltaTime;
                _txtWaiting.text = (int)(_fWaitDuration - _fWaitCount + 1) + "";
                if (_fWaitCount >= _fWaitDuration)
				{
                    _objWaiting.SetActive(false);
                    _fWaitCount = 0;
                    _txtWaiting.text = "";
                    _touchZoneHandler.PauseListener = false;
				}
			}
		}

        void InitializeHandlers()
		{
            _isFinished = false;
            _touchZoneHandler.PauseListener = true;
            _objWaiting.SetActive(true);
            _ball.InitBallController();
            _ballPusher.InitBallPusher();
        }

        void SpawnBricks()
		{
            //Clear current Bricks and list
            int nListCount = _brickList.Count;
            for (int i = 0; i < nListCount; i++)
			{
                Destroy(_brickList[0].gameObject);
                _brickList.RemoveAt(0);
			}
            //Generate new bricks
            for (int i = 0; i < _arrBrickSpawnPoints.Length; i++)
			{
                _brickList.Add(_arrBrickSpawnPoints[i].GenerateBrick(this));
			}
		}

        void ShowPlayFail()
		{
            _objFailPanel.SetActive(true);
            _objSuccessPanel.SetActive(false);
            _objWaiting.SetActive(false);
            InitializeHandlers();

        }
		#endregion
		#region Public Methods
		public void LoseLife()
		{
            if (_isFinished)
                return;
            if (_objFailPanel.active) // if failed status then wait
                return;
            CurrentLife--;
            if (CurrentLife > -1)
			{
                //set the correspond life icon as lose
                _arrLifeIcon[CurrentLife].LoseLife();
            }

            if (CurrentLife == 0)
			{
                //handle die 
                ShowPlayFail();
			} else
			{
                //respawn the ball and activate waiting panel
                InitializeHandlers();
            }
		}

        public void DestroyBrick(BrickUnit brick)
		{
            _brickList.Remove(brick);
            Destroy(brick.gameObject);
            // No brick remained then success
            if (_brickList.Count == 0)
			{
                _objSuccessPanel.SetActive(true);
                _isFinished = true;
                InitializeHandlers();
            }
		}
        //Wait panel status of the time counts of  3, 2, 1 go
        public bool IsWaitStatus()
		{
            return _objWaiting.active;
		}

        public void RestartLevel() // Initialize the game level scene again
		{
            SpawnBricks();
            _objFailPanel.SetActive(false);
            _objSuccessPanel.SetActive(false);
            CurrentLife = MaxLife;
            for (int i = 0; i < _arrLifeIcon.Length; i++)
			{
                _arrLifeIcon[i].InitColor();
			}
            _objWaiting.SetActive(true);
            InitializeHandlers();

        }

        public void LoadNextScene()
		{
            SceneManager.LoadScene("Level" + (nLevel + 1));
		}

        public void QuitGame()
		{
            Application.Quit();
		}
		#endregion
	}
}

