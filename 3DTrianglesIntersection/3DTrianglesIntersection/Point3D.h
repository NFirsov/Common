#pragma once

namespace firsovn
{
	struct Point3D
	{
		double at[3];

		Point3D();
		Point3D(const Point3D& p);
		Point3D(const double(&p)[3]);
		Point3D(const double& x, const double& y, const double& z);
		bool operator==(const Point3D& p) const;
		double length();
		bool isNull();
		double dot(const Point3D& p) const;
	};
}

