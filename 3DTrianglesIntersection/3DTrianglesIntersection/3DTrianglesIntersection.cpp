
#include "stdafx.h"
#include <iostream>
#include "TrianglesIntersectTests.h"

#include "TrianglesIntersect.h"

int main()
{
	firsovn::RelativePositionTest();
	firsovn::IsInsideTriangleTest();
	firsovn::TrianglesIntersectTest();

	char c;
	std::cin >> c;
	return 0;
}

