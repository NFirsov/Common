#include "stdafx.h"
#include "LinearEquations.h"

namespace firsovn
{
	template<int pos>
	double Determinant(const double(&coefMatrix)[3][3], const double(&constTerms)[3]);

	bool LinearEquations::SolveCramer(const double(&coefMatrix)[3][3], const double(&constTerms)[3], double(&result)[3])
	{
		double det = Determinant<0>(coefMatrix, constTerms);
		if (det == 0)
			return false;

		double detX1 = Determinant<1>(coefMatrix, constTerms);
		double detX2 = Determinant<2>(coefMatrix, constTerms);
		double detX3 = Determinant<3>(coefMatrix, constTerms);

		result[0] = detX1 / det; //TODO: Possible overflow
		result[1] = detX2 / det;
		result[2] = detX3 / det;

		return true;
	}

	template<int pos>
	double Determinant(const double(&coefMatrix)[3][3], const double(&constTerms)[3])
	{
		static_assert((pos >= 0 && pos <=3), "Template argument 'pos' must be in the interval from 0 to 3.");

		double a11 = pos == 1 ? constTerms[0] : coefMatrix[0][0];
		double a12 = pos == 2 ? constTerms[0] : coefMatrix[0][1];
		double a13 = pos == 3 ? constTerms[0] : coefMatrix[0][2];
		double a21 = pos == 1 ? constTerms[1] : coefMatrix[1][0];
		double a22 = pos == 2 ? constTerms[1] : coefMatrix[1][1];
		double a23 = pos == 3 ? constTerms[1] : coefMatrix[1][2];
		double a31 = pos == 1 ? constTerms[2] : coefMatrix[2][0];
		double a32 = pos == 2 ? constTerms[2] : coefMatrix[2][1];
		double a33 = pos == 3 ? constTerms[2] : coefMatrix[2][2];

		return (a11 * a22 * a33) + (a12 * a23 * a31) + (a13 * a21 * a32) -
			(a13 * a22 * a31) - (a11 * a23 * a32) - (a12 * a21 * a33);
	}
}

