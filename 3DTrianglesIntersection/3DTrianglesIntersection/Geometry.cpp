#include "stdafx.h"


#include <cmath>
#include "Geometry.h"

namespace firsovn
{
	void CrossProduct(const Point3D& p0, const Point3D& p1, const Point3D& p2, Point3D& result)
	{
		double x21 = p1.at[0] - p0.at[0];
		double x31 = p2.at[0] - p0.at[0];
		double y21 = p1.at[1] - p0.at[1];
		double y31 = p2.at[1] - p0.at[1];
		double z21 = p1.at[2] - p0.at[2];
		double z31 = p2.at[2] - p0.at[2];

		double x = y21*z31 - z21*y31;
		double y = z21*x31 - x21*z31;
		double z = x21*y31 - y21*x31;

		result.at[0] = x;
		result.at[1] = y;
		result.at[2] = z;
	}

	void CrossProduct(const Point3D& a, const Point3D& b, Point3D& result)
	{
		double x21 = a.at[0];
		double x31 = b.at[0];
		double y21 = a.at[1];
		double y31 = b.at[1];
		double z21 = a.at[2];
		double z31 = b.at[2];

		double x = y21*z31 - z21*y31;
		double y = z21*x31 - x21*z31;
		double z = x21*y31 - y21*x31;

		result.at[0] = x;
		result.at[1] = y;
		result.at[2] = z;
	}

	void GetUniquePoints(const Point3D(&pts)[3], std::vector<int>& uniq)
	{
		uniq.push_back(0);
		if (!(pts[1] == pts[0]))
			uniq.push_back(1);
		if (!(pts[2] == pts[0]) && !(pts[2] == pts[1]))
			uniq.push_back(2);
	}

	// Suppose 'p' lies on the line formed by 'p0' and 'p1'
	double GetPointRelativePosition(const Point3D& p0, const Point3D& p1, const Point3D& p)
	{
		double x10 = p1.at[0] - p0.at[0];
		if (x10 != 0)
			return (p.at[0] - p0.at[0]) / x10;

		double y10 = p1.at[1] - p0.at[1];
		if (y10 != 0)
			return (p.at[1] - p0.at[1]) / y10;

		double z10 = p1.at[2] - p0.at[2];
		if (z10 != 0)
			return (p.at[2] - p0.at[2]) / z10;

		return 0;
	}

	void GetMiddlePoint(const Point3D& x0, const Point3D& x1, double a, Point3D& res)
	{
		res.at[0] = (x1.at[0] - x0.at[0]) * a + x0.at[0];
		res.at[1] = (x1.at[1] - x0.at[1]) * a + x0.at[1];
		res.at[2] = (x1.at[2] - x0.at[2]) * a + x0.at[2];
	}

	bool TryGetIntersectPoint(const Point3D& ort, const double& d, const Point3D& p0, const Point3D& p1, Point3D& res)
	{
		double scp1 = ort.dot(p0);
		double div = ort.dot(p1) - scp1;
		if (div == 0)
			return false;

		double alpha = (d - scp1) / div;

		if (0 <= alpha && alpha <= 1)
		{
			GetMiddlePoint(p0, p1, alpha, res);
			return true;
		}
		return false;
	}

	// Suppose 'p' lies on the plane formed by triangle, and triangle is not merged.
	bool IsInsideTriangle(const Point3D(&t)[3], const Point3D& p)
	{
		Point3D pr0;
		CrossProduct(t[0], t[1], p, pr0);
		Point3D pr1;
		CrossProduct(t[0], t[2], p, pr1);
		Point3D pr2;
		CrossProduct(p, t[1], t[2], pr2);
		Point3D pr;
		CrossProduct(t[0], t[1], t[2], pr);

		double ls = pr0.length() + pr1.length() + pr2.length();
		double lt = pr.length();

		if (ls > lt)
			return false;

		return true;
	}
}



