using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyMind
{
	/***************************************************
	 * WallHandler is simple class and its work is
	 * extending box collider 2D size of 4 walls based
	 * on their stretched sizes in runtime.
	 * ************************************************/
    public class WallHandler : MonoBehaviour
    {
		#region protected Field
		protected RectTransform _rectTransform;
		protected BoxCollider2D _boxCollider2D;
		#endregion
		#region Protected Methods
		void InitializeColliderSize()
		{
			_rectTransform = this.gameObject.GetComponent<RectTransform>();
			_boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
			_boxCollider2D.size = new Vector2(_rectTransform.rect.size.x, _rectTransform.rect.size.y);
		}
		#endregion

		#region MonoBehaviour Callbacks
		private void Start()
		{
			InitializeColliderSize();
		}
		#endregion

	}
}

