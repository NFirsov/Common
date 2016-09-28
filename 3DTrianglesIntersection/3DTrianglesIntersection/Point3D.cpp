#include "stdafx.h"

#include <cmath>
#include "Point3D.h"

namespace firsovn
{
	Point3D::Point3D()
	{
	}

	Point3D::Point3D(const Point3D& p)
	{
		at[0] = p.at[0];
		at[1] = p.at[1];
		at[2] = p.at[2];
	}

	Point3D::Point3D(const double(&p)[3])
	{
		at[0] = p[0];
		at[1] = p[1];
		at[2] = p[2];
	}

	Point3D::Point3D(const double& x, const double& y, const double& z)
	{
		at[0] = x;
		at[1] = y;
		at[2] = z;
	}

	bool Point3D::operator==(const Point3D& p) const
	{
		return at[0] == p.at[0] && at[1] == p.at[1] && at[2] == p.at[2];
	}

	bool Point3D::isNull()
	{
		return at[0] == 0.0 && at[1] == 0.0 && at[2] == 0.0;
	}

	double Point3D::length()
	{
		double x = at[0];
		double y = at[1];
		double z = at[2];

		return std::sqrt(x*x + y*y + z*z);
	}

	// Scalar product
	double Point3D::dot(const Point3D& p) const
	{
		return at[0] * p.at[0] + at[1] * p.at[1] + at[2] * p.at[2];
	}
}

