#pragma once

namespace firsovn
{

	class LinearEquations
	{
	public:
		static bool SolveCramer(const double(&coefMatrix)[3][3], const double(&constTerms)[3], double(&result)[3]);
	};
}
