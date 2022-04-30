using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyMind
{
	/************************************
	 * This class is used for level design
	 * with bricks and brick spawner is
	 * only visible on editor's scene window.
	 * **********************************/
    public class BrickSpawner : MonoBehaviour
    {
		#region Serialization Field
		[SerializeField]
		GameObject _brickPrefab;
		[SerializeField]
		int _life;
		#endregion

		#region Public Methods
		public BrickUnit GenerateBrick(BallManager manager)
		{
			GameObject objBrick = Instantiate(_brickPrefab, transform.parent);
			objBrick.transform.localPosition = transform.localPosition;
			objBrick.GetComponent<BrickUnit>().InitBrick(_life, manager);
			return objBrick.GetComponent<BrickUnit>();
		}
		#endregion
	}

}
