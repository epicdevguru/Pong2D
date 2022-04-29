using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
namespace AnyMind
{
    public class LifeIcon : MonoBehaviour
    {
		#region Serialization Field
        [SerializeField]
        MMUIShaker _lifeShaker;
        [SerializeField]
        Image _imgIcon;
        [SerializeField]
        Color[] _arrColor;
		[SerializeField]
		float _shakeTime = 0.5f;
		#endregion
		#region Public Methods
		public void InitColor()
		{
			_imgIcon.color = _arrColor[0];
		}
		public void LoseLife()
		{
			_imgIcon.color = _arrColor[1];
			StartCoroutine(_lifeShaker.Shake(_shakeTime));

		}
		#endregion

	}
}

