using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
namespace AnyMind
{
    public class BrickUnit : MonoBehaviour
    {
        #region Serialization Field
        [SerializeField]
        Image _imgBrick;
        [SerializeField]
        Color[] _arrColors;
        [SerializeField]
        int _life;
        public int Life
		{
            get { return _life; }
            set { _life = value; }
		}
        [SerializeField]
        LayerMask _ballMask;
        [SerializeField]
        GameObject _breakParticle;
        [SerializeField]
        MMUIShaker _shaker;
        [SerializeField]
        GameObject[] _bonusPrefabs;
        #endregion
        #region Private Field
        BallManager _manager;
        int _reducedLife =0;
		#endregion
		#region Public Methods
		public void InitBrick(int life,  BallManager manager)
		{
            _life = life;
            _reducedLife = 0;
            _imgBrick.color = _arrColors[_life - 1];
            _manager = manager;
		}

        #endregion
        #region Physics collision Detection Event
        void OnCollisionEnter2D(Collision2D collision)
		{
            // Check if the collided object is ball
            if (_ballMask == (_ballMask | (1 << collision.gameObject.layer))) 
			{
                _life--;
                _reducedLife++;
                collision.gameObject.GetComponent<BallController>().HitPusher();
                if (_life > 0)
				{
                    StartCoroutine(_shaker.Shake(Random.Range(0, 0.5f)));
                    _imgBrick.color = _arrColors[_life - 1];
                }
                else
				{
                    Vector3 v3ParticlePos = transform.localPosition;
                    v3ParticlePos.z = -100f;
                    GameObject objParticle = Instantiate(_breakParticle, transform.parent);
                    objParticle.transform.localPosition = v3ParticlePos;
                    DropRandomBonusItem();
                    _manager.DestroyBrick(this);

                }
			}
		}
		#endregion
		#region Private Methods
        private void DropRandomBonusItem()
		{
            int nBonusItemIndex = Random.Range(0, _bonusPrefabs.Length);
            int nOccurenceRange = 30 * _reducedLife;
            int nRandomChoose = Random.Range(0, 100);
            if (_reducedLife > 1)
                nRandomChoose = 0;
            if (nOccurenceRange > nRandomChoose)
			{
                GameObject objBonus = Instantiate(_bonusPrefabs[nBonusItemIndex], transform.parent);
                objBonus.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -20f);
                
			}
		}
		#endregion
	}
}

