#include "stdafx.h"

#include "Geometry.h"
#include "LinearEquations.h"


namespace firsovn
{
	bool AreTrianglesIntersect_3x3_X_planes(const Point3D(&tr0)[3], const Point3D(&tr1)[3], const Point3D& ort0, const Point3D& ort1);
	bool AreTrianglesIntersect_3x3_I_planes(const Point3D(&tr0)[3], const Point3D(&tr1)[3]);
	bool AreTrianglesIntersect_3x2(const Point3D(&tr)[3], const Point3D& ort, const Point3D& p0, const Point3D& p1);
	bool AreTrianglesIntersect_3x1(const Point3D(&tr)[3], const Point3D& ort, const Point3D& p);
	bool AreSegmentsIntersect(const Point3D(&tr0)[3], const Point3D(&tr1)[3]);
	bool AreSegmentsIntersect_2x2_I_Plane(const Point3D& p0, const Point3D& p1, const Point3D& p2, const Point3D& p3);
	void ArrayToPoints(const double(&tr)[9], Point3D(&res)[3]);
	
	// Function to determine if two 3D triangles are intersect.
	bool AreTrianglesIntersect(const double(&arr0)[9], const double(&arr1)[9])
	{
		Point3D tr0[3];
		Point3D tr1[3];
		ArrayToPoints(arr0, tr0);
		ArrayToPoints(arr1, tr1);

		Point3D ort0;
		Point3D ort1;
		CrossProduct(tr0[0], tr0[1], tr0[2], ort0);
		CrossProduct(tr1[0], tr1[1], tr1[2], ort1);
		bool hasOrt0 = !ort0.isNull();
		bool hasOrt1 = !ort1.isNull();

		if (hasOrt0 && hasOrt1)
		{
			Point3D ort;
			CrossProduct(ort0, ort1, ort);
			if (ort.isNull()) //II-planes or I-planes
			{
				double d0 = ort0.dot(tr0[0]);
				double d0_1 = ort0.dot(tr1[0]);
				if (d0 != d0_1) //II-planes
					return false;

				return AreTrianglesIntersect_3x3_I_planes(tr0, tr1);
			}

			return AreTrianglesIntersect_3x3_X_planes(tr0, tr1, ort0, ort1); // X-planes
		}
		else if (hasOrt0 || hasOrt1)
		{
			Point3D(&tr)[3] = (hasOrt0 ? tr0 : tr1);
			Point3D& ort = (hasOrt0 ? ort0 : ort1);
			Point3D(&pts)[3] = (hasOrt0 ? tr1 : tr0);

			std::vector<int> uniq;
			GetUniquePoints(pts, uniq);

			if (uniq.size() == 2)
				return AreTrianglesIntersect_3x2(tr, ort, pts[uniq[0]], pts[uniq[1]]);

			return AreTrianglesIntersect_3x1(tr, ort, pts[uniq[0]]);
		}
		else // Both triangles are collapsed into line or point
		{
			return AreSegmentsIntersect(tr0, tr1);
		}

		return true; //Unexpected
	}

	// Function to determine if two 3D triangles are intersect. Triangles must lay on one plane. Triangles must be unmerged.
	bool AreTrianglesIntersect_3x3_I_planes(const Point3D(&tr0)[3], const Point3D(&tr1)[3])
	{
		bool res = IsInsideTriangle(tr0, tr1[0]) || IsInsideTriangle(tr0, tr1[1]) || IsInsideTriangle(tr0, tr1[2]) ||
				   IsInsideTriangle(tr1, tr0[0]) || IsInsideTriangle(tr1, tr0[1]) || IsInsideTriangle(tr1, tr0[2]);
		if (res)
			return true;

		for(int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
				if (AreSegmentsIntersect_2x2_I_Plane(tr0[i], tr0[(i+1)%3], tr1[j], tr1[(j + 1) % 3]))
					return true;

		return false;
	}

	// Function to determine if two 3D triangles are intersect. Triangles must lay on skew crossing planes. Triangles must be unmerged.
	bool AreTrianglesIntersect_3x3_X_planes(const Point3D(&tr0)[3], const Point3D(&tr1)[3], const Point3D& ort0, const Point3D& ort1)
	{
		double d0 = ort0.dot(tr0[0]);
		double d1 = ort1.dot(tr1[0]);

		Point3D ip0[3];
		std::vector<int> v0;

		bool res = TryGetIntersectPoint(ort0, d0, tr1[0], tr1[1], ip0[0]);
		if (res) v0.push_back(0);
		res = TryGetIntersectPoint(ort0, d0, tr1[1], tr1[2], ip0[1]);
		if (res) v0.push_back(1);
		res = TryGetIntersectPoint(ort0, d0, tr1[2], tr1[0], ip0[2]);
		if (res) v0.push_back(2);
		if (v0.size() == 2 && ip0[v0[0]] == ip0[v0[1]]) //if p0==p1 remove p1
			v0.pop_back();
		if (v0.size() == 0)
			return false;


		Point3D ip1[3];
		std::vector<int> v1;

		res = TryGetIntersectPoint(ort1, d1, tr0[0], tr0[1], ip1[0]);
		if (res) v1.push_back(0);
		res = TryGetIntersectPoint(ort1, d1, tr0[1], tr0[2], ip1[1]);
		if (res) v1.push_back(1);
		res = TryGetIntersectPoint(ort1, d1, tr0[2], tr0[0], ip1[2]);
		if (res) v1.push_back(2);
		if (v1.size() == 2 && ip1[v1[0]] == ip1[v1[1]]) //if p0==p1 remove p1
			v1.pop_back();
		if (v1.size() == 0)
			return false;

		if (v0.size() == 2 && v1.size() == 2)
		{
			double rp1 = GetPointRelativePosition(ip0[v0[0]], ip0[v0[1]], ip1[v1[0]]);
			double rp2 = GetPointRelativePosition(ip0[v0[0]], ip0[v0[1]], ip1[v1[1]]);
			if ((0 <= rp1 && rp1 <= 1) || (0 <= rp2 && rp2 <= 1) || (rp1 < 0 && rp2 > 1) || (rp2 < 0 && rp1 > 1))
				return true;
			return false;
		}
		else if (v0.size() == 2 && v1.size() == 1)
		{
			double rp = GetPointRelativePosition(ip0[v0[0]], ip0[v0[1]], ip1[v1[0]]);
			return (0 <= rp && rp <= 1);
		}
		else if (v0.size() == 1 && v1.size() == 2)
		{
			double rp = GetPointRelativePosition(ip1[v1[0]], ip1[v1[1]], ip0[v0[0]]);
			return (0 <= rp && rp <= 1);
		}
		else if (v0.size() == 1 && v1.size() == 1)
		{
			return (ip0[v0[0]] == ip1[v1[0]]); // return p1==p2
		}

		return true;
	}

	// Function to determine if 3-point triangle intersects with 2-point merged triangle.
	bool AreTrianglesIntersect_3x2(const Point3D(&tr)[3], const Point3D& ort, const Point3D& p0, const Point3D& p1)
	{
		double d = ort.dot(tr[0]);

		std::vector<const Point3D*> onPlane;
		if (ort.dot(p0) == d) onPlane.push_back(&p0);
		if (ort.dot(p1) == d) onPlane.push_back(&p1);

		auto count = onPlane.size();
		if (count == 0)
		{
			Point3D ip;
			bool res = TryGetIntersectPoint(ort, d, p0, p1, ip);
			if (res)
				return IsInsideTriangle(tr, ip);

			return false;
		}
		else if (count == 1)
		{
			return IsInsideTriangle(tr, *onPlane[0]);
		}
		else if (count == 2)
		{
			bool res = IsInsideTriangle(tr, p0) || IsInsideTriangle(tr, p1);
			if (res)
				return true;

			return AreSegmentsIntersect_2x2_I_Plane(tr[0], tr[1], p0, p1) || AreSegmentsIntersect_2x2_I_Plane(tr[1], tr[2], p0, p1) || AreSegmentsIntersect_2x2_I_Plane(tr[0], tr[2], p0, p1);
		}

		return true;
	}

	// Function to determine if 3-point triangle intersects with 1-point merged triangle.
	bool AreTrianglesIntersect_3x1(const Point3D(&tr)[3], const Point3D& ort, const Point3D& p)
	{
		double d = ort.dot(tr[0]);
		bool onPlane = (ort.dot(p) == d);
		
		if (!onPlane)
			return false;

		return IsInsideTriangle(tr, p);
	}

	// Function to determine if merged 1/2-point triangles are intersect.
	bool AreSegmentsIntersect(const Point3D(&tr0)[3], const Point3D(&tr1)[3])
	{
		std::vector<int> uniq0;
		std::vector<int> uniq1;
		GetUniquePoints(tr0, uniq0);
		GetUniquePoints(tr1, uniq1);

		int s0 = uniq0.size();
		int s1 = uniq1.size();

		if (s0 == 2 && s1 == 2) // 2x2
		{
			const Point3D& p0 = tr0[uniq0[0]];
			const Point3D& p1 = tr0[uniq0[1]];
			const Point3D& p2 = tr1[uniq1[0]];
			const Point3D& p3 = tr1[uniq1[1]];

			Point3D ort;
			CrossProduct(p0, p1, p2, ort);
			if (ort.isNull() || ort.dot(p2) == ort.dot(p3)) // I-plane
				return AreSegmentsIntersect_2x2_I_Plane(p0, p1, p2, p3);

			return false; // lines not in I-plane
		}
		else if (s0 == 1 && s1 == 1) // 1x1
		{
			return tr0[uniq0[0]] == tr1[uniq1[0]];
		}
		else if (s0 + s1 == 3) // 2x1
		{
			const Point3D& p0 = (s0 == 2 ? tr0[uniq0[0]] : tr1[uniq1[0]]);
			const Point3D& p1 = (s0 == 2 ? tr0[uniq0[1]] : tr1[uniq1[1]]);
			const Point3D& p2 = (s0 == 2 ? tr1[uniq1[0]] : tr0[uniq0[0]]);

			Point3D res;
			CrossProduct(p0, p1, p2, res);
			if (res.isNull()) // I-line
			{
				double rp = GetPointRelativePosition(p0, p1, p2);
				return (0 <= rp && rp <= 1);
			}
			return false;
		}

		return true; //Unexpected
	}

	// Function to determine if two segments are intersect. Segments must lay on one plane. Segments must be unmerged.
	bool AreSegmentsIntersect_2x2_I_Plane(const Point3D& p0, const Point3D& p1, const Point3D& p2, const Point3D& p3)
	{
		Point3D p10(p1.at[0] - p0.at[0], p1.at[1] - p0.at[1], p1.at[2] - p0.at[2]);
		Point3D p32(p3.at[0] - p2.at[0], p3.at[1] - p2.at[1], p3.at[2] - p2.at[2]);
		Point3D ort;
		CrossProduct(p10, p32, ort);
		if (ort.isNull()) // II-lines
		{
			CrossProduct(p0, p1, p2, ort);
			if (ort.isNull()) // I-lines
			{
				double rp2 = GetPointRelativePosition(p0, p1, p2);
				double rp3 = GetPointRelativePosition(p0, p1, p3);
				if ((0 <= rp2 && rp2 <= 1) || (0 <= rp3 && rp3 <= 1) || (rp2 < 0 && rp3 > 1) || (rp3 < 0 && rp2 > 1))
					return true;
			}
			return false;
		}

		// X-lines
		double coef[3][3];
		coef[0][0] = p10.at[0];
		coef[1][0] = p10.at[1];
		coef[2][0] = p10.at[2];

		coef[0][1] = -p32.at[0];
		coef[1][1] = -p32.at[1];
		coef[2][1] = -p32.at[2];

		coef[0][2] = ort.at[0];
		coef[1][2] = ort.at[1];
		coef[2][2] = ort.at[2];

		double ct[3];
		ct[0] = p2.at[0] - p0.at[0];
		ct[1] = p2.at[1] - p0.at[1];
		ct[2] = p2.at[2] - p0.at[2];

		double eqres[3];
		if (LinearEquations::SolveCramer(coef, ct, eqres))
		{
			double alpha = eqres[0];
			double beta = eqres[1];

			return (0 <= alpha && alpha <= 1 && 0 <= beta && beta <= 1);
		}

		return true; //Unexpected
	}
	
	void ArrayToPoints(const double(&tr)[9], Point3D(&res)[3])
	{
		for (int i = 0; i < 3; i++)
		{
			res[i].at[0] = tr[3 * i];
			res[i].at[1] = tr[3 * i + 1];
			res[i].at[2] = tr[3 * i + 2];
		}
	}

}
