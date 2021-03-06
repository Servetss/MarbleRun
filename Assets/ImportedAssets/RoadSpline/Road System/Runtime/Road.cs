using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Barmetler.RoadSystem
{
	public class Road : MonoBehaviour
	{
		public RoadAnchor start;
		public RoadAnchor end;

		[SerializeField, HideInInspector]
		bool autoSetControlPoints = false;

		[SerializeField, HideInInspector]
		List<Vector3> points = new List<Vector3>();
		[SerializeField, HideInInspector]
		List<float> angles = new List<float>();
		[SerializeField, HideInInspector]
		Bounds bounds = new Bounds();
		[SerializeField, HideInInspector]
		List<Bounds> boundingBoxes = new List<Bounds>();

		[Space]
		[SerializeField] private Transform _playerPosition;

		public Bounds BoundingBox { get => bounds; }
		public List<Bounds> BoundingBoxes { get => boundingBoxes; }

		public bool AutoSetControlPoints
		{
			get => autoSetControlPoints;
			set
			{
				if (autoSetControlPoints != value)
				{
					autoSetControlPoints = value;
					if (autoSetControlPoints)
					{
						AutoSetAllControlPoints();
					}
				}
			}
		}

        public struct EvenlySpacedPointsContext
		{
			public EvenlySpacedPointsContext(float _spacing, float _resolution) { spacing = _spacing; resolution = _resolution; }

			public float spacing;
			public float resolution;

			public override bool Equals(object obj)
			{
				return obj is EvenlySpacedPointsContext other && other.spacing == spacing && other.resolution == resolution;
			}

			public override int GetHashCode()
			{
				return string.Format("{0}-{1}", spacing, resolution).GetHashCode();
			}
		}

		private readonly ContextDataCache<Bezier.OrientedPoint[], EvenlySpacedPointsContext> evenlySpacedPointsCache =
			new ContextDataCache<Bezier.OrientedPoint[], EvenlySpacedPointsContext>();
		private readonly ContextDataCache<float, EvenlySpacedPointsContext> lengthCache = new ContextDataCache<float, EvenlySpacedPointsContext>();

		public Bezier.OrientedPoint[] OrientedPoints => GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(transform)).ToArray();

		public Vector3 this[int i]
		{
			get => points[LoopIndex(i)];
			private set => points[LoopIndex(i)] = value;
		}

		public int NumPoints { get => points.Count; }

		public int NumSegments { get => points.Count / 3; }

		public Road()
		{
			evenlySpacedPointsCache.children.Add(lengthCache);
		}

        public int GetIndexOnPlineByTransform(Transform itemTransform)
		{
			Vector3 savePosition = itemTransform.position;

			itemTransform.localPosition = new Vector3(0, itemTransform.localPosition.y, itemTransform.localPosition.z);

			Bezier.OrientedPoint[] orientedPoints = GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(transform)).ToArray();

			float distanceBetweenDots = Vector3.Distance(orientedPoints[1].position, orientedPoints[0].position);

			int splineIndex = (int)(Vector3.Distance(itemTransform.position, orientedPoints[0].position) / distanceBetweenDots);

			itemTransform.position = savePosition;

			return splineIndex;
		}

		public void Clear()
		{
			points.Clear();
			angles.Clear();
		}

		public void RefreshEndPoints(bool updatemesh = true)
		{
			if (start != null) start.SetRoad(this, true);
			if (end != null) end.SetRoad(this, false);

			if (points.Count == 0 || angles.Count == 0)
			{
				points = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 3), new Vector3(0, 0, 4) };
				angles = new List<float> { 0, 0 };

				if (start != null)
					points[0] = transform.InverseTransformPoint(start.transform.position);
				if (end != null)
					points[3] = transform.InverseTransformPoint(end.transform.position);

				if (start != null)
				{
					angles[0] = Bezier.AngleFromNormal(
						transform.InverseTransformDirection(start.transform.forward),
						transform.InverseTransformDirection(start.transform.up));
					points[1] = this[0] + transform.InverseTransformDirection(start.transform.forward) * (this[3] - this[0]).magnitude / 2;
				}
				if (end != null)
				{
					angles[1] = -Bezier.AngleFromNormal(
						transform.InverseTransformDirection(end.transform.forward),
						transform.InverseTransformDirection(end.transform.up));
					points[2] = this[3] + transform.InverseTransformDirection(end.transform.forward) * (this[0] - this[3]).magnitude / 2;
				}

				if (autoSetControlPoints)
					AutoSetAllControlPoints();

				OnCurveChanged(updatemesh);
			}
			else if (NumPoints > 1)
			{
				if (start != null)
				{
					Vector3 a = this[0];
					Vector3 b = this[1];
					float c = angles[0];
					float startLength = (this[1] - this[0]).magnitude;
					this[0] = transform.InverseTransformPoint(start.transform.position);
					this[1] = this[0] + transform.InverseTransformDirection(start.transform.forward) * startLength;
					angles[0] = Bezier.AngleFromNormal(
						transform.InverseTransformDirection(start.transform.forward),
						transform.InverseTransformDirection(start.transform.up));
					if (a != this[0] || b != this[1] || c != angles[0])
						OnCurveChanged(updatemesh);
				}
				if (end != null)
				{
					Vector3 a = this[-1];
					Vector3 b = this[-2];
					float c = angles[angles.Count - 1];
					float endLength = (this[-2] - this[-1]).magnitude;
					this[-1] = transform.InverseTransformPoint(end.transform.position);
					this[-2] = this[-1] + transform.InverseTransformDirection(end.transform.forward) * endLength;
					angles[angles.Count - 1] = -Bezier.AngleFromNormal(
						transform.InverseTransformDirection(end.transform.forward),
						transform.InverseTransformDirection(end.transform.up));
					if (a != this[-1] || b != this[-2] || c != angles[angles.Count - 1])
						OnCurveChanged(updatemesh);
				}
			}
		}

		public Vector3[] GetPointsInSegment(int i)
		{
			return new Vector3[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };
		}

		public void AppendSegment(Vector3 pos, bool isStart, Vector3 normal = default(Vector3))
		{
			if (isStart && start != null) return;
			if (!isStart && end != null) return;

			if (normal == default(Vector3)) normal = Vector3.up;

			if (isStart)
			{
				points.InsertRange(0, new Vector3[] { pos, Vector3.zero, Vector3.zero });
				this[2] = this[3] - (this[4] - this[3]).normalized * 0.5f * (this[0] - this[3]).magnitude;
				this[1] = pos + 0.85f * (this[2] - this[0]);
				this[1] -= Vector3.Dot(normal, (this[1] - this[0])) * normal;
				var angle = Bezier.AngleFromNormal(this[0] - this[1], normal);
				angles.Insert(0, angle);
			}
			else
			{
				points.AddRange(new Vector3[] { Vector3.zero, Vector3.zero, pos });
				this[-3] = this[-4] - (this[-5] - this[-4]).normalized * 0.5f * (this[-1] - this[-4]).magnitude;
				this[-2] = pos + 0.85f * (this[-3] - this[-1]);
				this[-2] -= Vector3.Dot(normal, (this[-2] - this[-1])) * normal;
				var angle = Bezier.AngleFromNormal(this[-1] - this[-2], normal);
				angles.Add(angle);
			}

			if (autoSetControlPoints)
			{
				AutoSetAllAffectedControlPoints(isStart ? 0 : NumPoints - 1);
			}
			OnCurveChanged();
		}

		public void InsertSegment(Vector3 pos, int segmentIndex)
		{
			points.InsertRange(segmentIndex * 3 + 2, new Vector3[] { Vector3.zero, pos, Vector3.zero });
			angles.Insert(segmentIndex + 1, 0);

			if (autoSetControlPoints)
			{
				AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
			}
			else
			{
				AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
			}
			OnCurveChanged();
		}

		public void DeleteAnchor(int anchorIndex)
		{
			if (NumSegments > 1)
			{
				if (anchorIndex == 0 && start == null)
				{
					points.RemoveRange(0, 3);
					angles.RemoveRange(0, 1);
				}
				else if (anchorIndex == NumPoints - 1 && end == null)
				{
					points.RemoveRange(anchorIndex - 2, 3);
					angles.RemoveRange(anchorIndex / 3, 1);
				}
				else if (anchorIndex > 0 && anchorIndex < NumPoints - 1)
				{
					points.RemoveRange(anchorIndex - 1, 3);
					angles.RemoveRange(anchorIndex / 3, 1);
				}
			}

			OnCurveChanged();
		}

		public void MovePoint(int i, Vector3 pos)
		{
			Vector3 oldPos = this[i];

			if (i % 3 == 0)
			{
				if ((start != null && i == 0) || (end != null && i == NumPoints - 1)) return;
				if (i > 0) this[i - 1] += pos - oldPos;
				if (i < NumPoints - 1) this[i + 1] += pos - oldPos;
				this[i] = pos;

				if (autoSetControlPoints)
					AutoSetAllAffectedControlPoints(i);
			}
			else
			{
				if (autoSetControlPoints) return;

				bool nextIsAnchor = (i + 1) % 3 == 0;
				int correspondingIndex = nextIsAnchor ? i + 2 : i - 2;
				Vector3 anchor = this[nextIsAnchor ? i + 1 : i - 1];

				if ((start != null && i == 1) || (end != null && i == NumPoints - 2))
				{
					bool isStart = i == 1;
					Vector3 forward = transform.InverseTransformDirection((isStart ? start : end).transform.forward);
					this[i] = this[isStart ? 0 : (NumPoints - 1)] +
						Mathf.Max(1e-1f, Vector3.Dot(pos - this[isStart ? 0 : (NumPoints - 1)], forward))
						* forward;
				}
				else if (i > 1 && i < NumPoints - 2)
				{
					float correspondingDistance = (this[correspondingIndex] - anchor).magnitude;
					this[i] = pos;
					Vector3 direction = (pos - anchor).normalized;
					this[correspondingIndex] = anchor - direction * correspondingDistance;
				}
				else
				{
					this[i] = pos;
				}
			}

			OnCurveChanged();
		}

		public void MoveAngle(int i, float angle)
		{
			if ((i == 0 && start != null) || (i == angles.Count - 1 && end != null)) return;
			angles[i] = angle;

			OnCurveChanged();
		}

		public float GetAngle(int i)
		{
			return angles[i];
		}

		public void OnValidate()
		{
			RefreshEndPoints(false);
			if (start != null) start.SetRoad(this, true);
			if (end != null) end.SetRoad(this, false);
		}

		#region Automatic Control Point Functions

		void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
		{
			for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
			{
				if (i >= 0 && i < NumPoints)
					AutoSetAnchorControlPoints(i);
			}

			AutoSetStartAndEndControls();

			OnCurveChanged();
		}

		public void AutoSetAllControlPoints()
		{
			for (int i = 0; i < NumPoints; i += 3)
			{
				AutoSetAnchorControlPoints(i);
			}

			AutoSetStartAndEndControls();

			OnCurveChanged();
		}

		void AutoSetAnchorControlPoints(int anchorIndex)
		{
			Vector3 anchorPos = this[anchorIndex];
			Vector3 direction = Vector3.zero;
			float[] neighborDistances = new float[2];
			if (anchorIndex - 3 >= 0)
			{
				Vector3 offset = this[anchorIndex - 3] - anchorPos;
				direction += offset.normalized;
				neighborDistances[0] = offset.magnitude;
			}
			if (anchorIndex + 3 <= NumPoints - 1)
			{
				Vector3 offset = this[anchorIndex + 3] - anchorPos;
				direction -= offset.normalized;
				neighborDistances[1] = -offset.magnitude;
			}

			direction.Normalize();

			for (int i = 0; i < 2; ++i)
			{
				int controlIndex = anchorIndex + i * 2 - 1;
				if (controlIndex >= 0 && controlIndex < NumPoints)
				{
					this[controlIndex] = anchorPos + direction * neighborDistances[i] * 0.5f;
				}
			}

			OnCurveChanged();
		}

		void AutoSetStartAndEndControls()
		{
			if (start == null)
				this[1] = (this[0] + this[2]) * 0.5f;
			else
				this[1] = this[0] + transform.InverseTransformDirection(start.transform.forward) * ((this[0] - this[3]) * 0.5f).magnitude;

			if (end == null)
				this[-2] = (this[-1] + this[-3]) * 0.5f;
			else
				this[-2] = this[-1] + transform.InverseTransformDirection(end.transform.forward) * ((this[-1] - this[-4]) * 0.5f).magnitude;

			OnCurveChanged();
		}

		// end Auto Control Points
		#endregion

		public Bezier.OrientedPoint[] GetEvenlySpacedPoints(float spacing, float resolution = 1)
		{
			CalculateEvenlySpacedPoints(spacing, resolution);
			return evenlySpacedPointsCache.GetData(new EvenlySpacedPointsContext(spacing, resolution));
		}

		void CalculateEvenlySpacedPoints(float spacing, float resolution = 1, bool calculateBoundingBoxes = false)
		{
			EvenlySpacedPointsContext context = new EvenlySpacedPointsContext(spacing, resolution);
			if (evenlySpacedPointsCache.IsValid(context) && !calculateBoundingBoxes) return;

			Bezier.OrientedPoint[] result;

			if (calculateBoundingBoxes)
			{
				result = Bezier.GetEvenlySpacedPoints(points, angles, ref bounds, boundingBoxes, spacing, resolution);
			}
			else
			{
				result = Bezier.GetEvenlySpacedPoints(points, angles, spacing, resolution);
			}

			evenlySpacedPointsCache.SetData(result, context);
		}

		public float GetLength(float spacing = 1, float resolution = 1)
		{
			EvenlySpacedPointsContext context = new EvenlySpacedPointsContext(spacing, resolution);
			if (lengthCache.IsValid(context))
				return lengthCache.GetData(context);

			Bezier.OrientedPoint[] points = GetEvenlySpacedPoints(spacing, resolution);

			float length = 0;
			Vector3 lastPoint = Vector3.zero;
			for (int i = 0; i < points.Length; ++i)
			{
				if (i > 0) length += (points[i].position - lastPoint).magnitude;
				lastPoint = points[i].position;
			}

			lengthCache.SetData(length, context);
			return length;
		}

		// to be used before GetMinDistance
		public bool IsMaybeCloser(Vector3 worldPosition, float minDistance, float yScale)
		{
			float sqrDst;
			Vector3 localPos = transform.InverseTransformPoint(worldPosition);

			// Check overall bounding box
			sqrDst = bounds.SqrDistance(Vector3.Scale(localPos, new Vector3(1, yScale, 1)));
			if (!bounds.Contains(Vector3.Scale(localPos, new Vector3(1, yScale, 1))) && sqrDst >= minDistance * minDistance) return false;

			// calculate min distance to all bounding boxes
			sqrDst = float.PositiveInfinity;
			foreach (var bounds in boundingBoxes)
			{
				if (bounds.Contains(new Vector3(localPos.x, bounds.center.y + yScale * (localPos.y - bounds.center.y), localPos.z)))
					return true;
				sqrDst = Mathf.Min(sqrDst, bounds.SqrDistance(new Vector3(localPos.x, bounds.center.y + yScale * (localPos.y - bounds.center.y), localPos.z)));
			}

			// check min distance to all bounding boxes
			if (sqrDst >= minDistance * minDistance) return false;

			return true;
		}

		public float GetMinDistance(Vector3 worldPosition, float stepSize, float yScale, out Vector3 closestPoint, out float distanceAlongRoad)
		{
			float currDistAlongRoad = 0;
			Vector3 localPos = transform.InverseTransformPoint(worldPosition);
			Vector3 closestPointLocal = Vector3.zero;

			distanceAlongRoad = 0;
			float minDst = float.PositiveInfinity;

			var points = GetEvenlySpacedPoints(stepSize);

			for (int i = 0; i < points.Length; ++i)
			{
				var point = points[i];

				float dst = Vector3.Scale(point.position - localPos, new Vector3(1, yScale, 1)).magnitude;
				if (dst < minDst)
				{
					Vector3 a = point.position;
					Vector3 b;
					float l;
					Vector3 n = Vector3.zero;
					bool correct = false;
					bool backwards = false;
					float along = 0;
					if (i < points.Length - 1)
					{
						b = points[i + 1].position;
						l = (b - a).magnitude;
						n = (b - a).normalized;
						float f;
						if ((f = Vector3.Dot(localPos - a, n)) > 0 && f < l)
						{
							along = f;
							correct = true;
						}
					}
					if (i > 0 && !correct)
					{
						b = points[i - 1].position;
						l = (b - a).magnitude;
						n = (b - a).normalized;
						float f;
						if ((f = Vector3.Dot(localPos - a, n)) > 0 && f < l)
						{
							along = f;
							correct = true;
							backwards = true;
						}
					}

					Vector3 pt = point.position;

					if (correct)
					{
						pt = a + along * n;
					}

					dst = Vector3.Scale(pt - localPos, new Vector3(1, yScale, 1)).magnitude;
					if (dst < minDst)
					{
						minDst = dst;
						distanceAlongRoad = currDistAlongRoad + (backwards ? -along : along);
						closestPointLocal = pt;
					}
				}
				currDistAlongRoad += stepSize;
			}

			closestPoint = transform.TransformPoint(closestPointLocal);
			return minDst;
		}

		public void OnCurveChanged(bool updateMesh = true)
		{
			evenlySpacedPointsCache.Invalidate();
			CalculateEvenlySpacedPoints(1, 1, true);

			if (GetComponent<RoadMeshGenerator>())
			{
				GetComponent<RoadMeshGenerator>().Invalidate(updateMesh);
			}
		}

		public int LoopIndex(int i) { return ((i % NumPoints) + NumPoints) % NumPoints; }
	}
}
