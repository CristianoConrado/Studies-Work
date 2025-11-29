using UnityEngine;
using StudiesWork.DataModifiers;
namespace StudiesWork.PathFinding
{
	[DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
	public sealed class Unit : MonoBehaviour
	{
		private Collider2D _collider;
		private Grid _grid;
		private Vector2[] _waypoints;
		private Vector2 _originPoint;
		private Vector2 _target;		private Vector2 _destinyDirection;
		private Vector2 _movementDirection = Vector2.up;
		private bool _hasTarget = false;
		private bool _returnToOrigin = false;
		private ushort _fixedFrameCounter = 0;
		[SerializeField, Min(0f)] private float _speed;
		[SerializeField, Range(1e-3f, 1f)] private float _turnSpeed;
		[SerializeField, Min(1f)] private float _lookDistance;
		[SerializeField] private ushort _fixedFramesToJump;
		private void Awake()
		{
			_collider = GetComponent<Collider2D>();
			_grid = new Grid(_collider, transform.position, _lookDistance);
			_target = _originPoint = transform.position;
			_fixedFrameCounter = _fixedFramesToJump;
		}
		private void FixedUpdate()
		{
			if(_fixedFrameCounter-- <= 0f)
			{
				_fixedFrameCounter = _fixedFramesToJump;
				Collider2D collider = Physics2D.OverlapCircle(_originPoint, _lookDistance, WorldBuild.TargetMask);
				if(_hasTarget = collider)
					if(!Physics2D.OverlapCircle(_target, (_collider.bounds.extents.x + _collider.bounds.extents.y) / 2f, WorldBuild.TargetMask))
					{
						_returnToOrigin = true;
						_target = collider.transform.position;
						_waypoints = new PathFinder(_grid = new Grid(_collider, transform.position, _lookDistance)).FindPath(transform.position, _target);
					}
			}
			if(_waypoints is not null && _waypoints.Length > 0)
			{
				_destinyDirection = (_waypoints[0] - (Vector2)transform.position).normalized;
				_movementDirection = Vector2.MoveTowards(_movementDirection, _destinyDirection, _turnSpeed * _speed * Time.fixedDeltaTime);
				if(Vector2.Distance(transform.position, _waypoints[0]) < _speed * Time.fixedDeltaTime)
				{
					transform.Translate(_speed * Time.fixedDeltaTime * _movementDirection);
					Vector2[] temporary = new Vector2[_waypoints.Length - 1];
					for(int i = 1; i < _waypoints.Length; i++)
						temporary[i - 1] = _waypoints[i];
					_waypoints = temporary;
				}
				else
					transform.Translate(_speed * Time.fixedDeltaTime * _movementDirection);
			}
			else if (!_hasTarget && _returnToOrigin && Vector2.Distance(transform.position, _originPoint) > _speed * Time.fixedDeltaTime)
			{
				_returnToOrigin = false;
				_waypoints = new PathFinder(_grid = new Grid(_collider, transform.position, _lookDistance)).FindPath(transform.position, _originPoint);
			}
		}
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(_originPoint, _lookDistance);
			Gizmos.color = Color.cyan;
			if (_collider)
				Gizmos.DrawWireSphere(_target, (_collider.bounds.extents.x + _collider.bounds.extents.y) / 2f);
			if(didStart)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(transform.position, new Vector2(_grid.Width, _grid.Height));
			}
			if(_waypoints is not null && _waypoints.Length > 0)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(transform.position, _waypoints[0]);
				for(int i = 1; i < _waypoints.Length; i++)
					Gizmos.DrawLine(_waypoints[i - 1], _waypoints[i]);
			}
		}
	};
};