#include "stdafx.h"
#include "LinearEquations.h"

namespace firsovn
{
	double determinant(const double(&coefMatrix)[3][3]);
	double determinantX1(const double(&coefMatrix)[3][3], const double(&constTerms)[3]);
	double determinantX2(const double(&coefMatrix)[3][3], const double(&constTerms)[3]);
	double determinantX3(const double(&coefMatrix)[3][3], const double(&constTerms)[3]);

	bool LinearEquations::SolveCramer(const double(&coefMatrix)[3][3], const double(&constTerms)[3], double(&result)[3])
	{
		double det = determinant(coefMatrix);
		if (det == 0)
			return false;

		double detX1 = determinantX1(coefMatrix, constTerms);
		double detX2 = determinantX2(coefMatrix, constTerms);
		double detX3 = determinantX3(coefMatrix, constTerms);

		result[0] = detX1 / det;
		result[1] = detX2 / det;
		result[2] = detX3 / det;

		return true;
	}

	double determinant(const double(&coefMatrix)[3][3])
	{
		double a11 = coefMatrix[0][0];
		double a12 = coefMatrix[0][1];
		double a13 = coefMatrix[0][2];
		double a21 = coefMatrix[1][0];
		double a22 = coefMatrix[1][1];
		double a23 = coefMatrix[1][2];
		double a31 = coefMatrix[2][0];
		double a32 = coefMatrix[2][1];
		double a33 = coefMatrix[2][2];

		return (a11 * a22 * a33) + (a12 * a23 * a31) + (a13 * a21 * a32) -
			(a13 * a22 * a31) - (a11 * a23 * a32) - (a12 * a21 * a33);
	}

	double determinantX1(const double(&coefMatrix)[3][3], const double(&constTerms)[3])
	{
		double a12 = coefMatrix[0][1];
		double a13 = coefMatrix[0][2];
		double a22 = coefMatrix[1][1];
		double a23 = coefMatrix[1][2];
		double a32 = coefMatrix[2][1];
		double a33 = coefMatrix[2][2];
		double c1 = constTerms[0];
		double c2 = constTerms[1];
		double c3 = constTerms[2];

		return (c1 * a22 * a33) + (a12 * a23 * c3) + (a13 * c2 * a32) -
			(a13 * a22 * c3) - (c1 * a23 * a32) - (a12 * c2 * a33);
	}

	double determinantX2(const double(&coefMatrix)[3][3], const double(&constTerms)[3])
	{
		double a11 = coefMatrix[0][0];
		double a13 = coefMatrix[0][2];
		double a21 = coefMatrix[1][0];
		double a23 = coefMatrix[1][2];
		double a31 = coefMatrix[2][0];
		double a33 = coefMatrix[2][2];
		double c1 = constTerms[0];
		double c2 = constTerms[1];
		double c3 = constTerms[2];

		return (a11 * c2 * a33) + (c1 * a23 * a31) + (a13 * a21 * c3) -
			(a13 * c2 * a31) - (a11 * a23 * c3) - (c1 * a21 * a33);
	}

	double determinantX3(const double(&coefMatrix)[3][3], const double(&constTerms)[3])
	{
		double a11 = coefMatrix[0][0];
		double a12 = coefMatrix[0][1];
		double a21 = coefMatrix[1][0];
		double a22 = coefMatrix[1][1];
		double a31 = coefMatrix[2][0];
		double a32 = coefMatrix[2][1];
		double c1 = constTerms[0];
		double c2 = constTerms[1];
		double c3 = constTerms[2];

		return (a11 * a22 * c3) + (a12 * c2 * a31) + (c1 * a21 * a32) -
			(c1 * a22 * a31) - (a11 * c2 * a32) - (a12 * a21 * c3);
	}
}

