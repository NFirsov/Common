#pragma once

#include <vector>
#include "Point3D.h"

namespace firsovn
{
	void CrossProduct(const Point3D& p0, const Point3D& p1, const Point3D& p2, Point3D& result);
	void CrossProduct(const Point3D& a, const Point3D& b, Point3D& result);

	bool TryGetIntersectPoint(const Point3D& ort, const double& d, const Point3D& p0, const Point3D& p1, Point3D& res);
	void GetMiddlePoint(const Point3D& x0, const Point3D& x1, double a, Point3D& res);

	void GetUniquePoints(const Point3D(&pts)[3], std::vector<int>& uniq);

	double GetPointRelativePosition(const Point3D& p0, const Point3D& p1, const Point3D& p);

	bool IsInsideTriangle(const Point3D(&t)[3], const Point3D& p);
}

