#include "stdafx.h"
#include <string>
#include <iostream>
#include <sstream>
#include <fstream>
#include "Geometry.h"
#include "TrianglesIntersect.h"
#include "TrianglesIntersectTests.h"


namespace firsovn
{
	void RelativePositionTest()
	{
		std::cout << "RelativePositionTest:\n\n";
		const int count = 6;
		const Point3D(&testCases)[count][3] = { { Point3D(1, 1, 1), Point3D(3, 3, 3), Point3D(2, 2, 2) },
												{ Point3D(1, 1, 1), Point3D(3, 3, 3), Point3D(0, 0, 0) },
												{ Point3D(1, 1, 1), Point3D(3, 3, 3), Point3D(4, 4, 4) },
												{ Point3D(1, 0, 0), Point3D(3, 0, 0), Point3D(4, 0, 0) },
												{ Point3D(0, 1, 0), Point3D(0, 3, 0), Point3D(0, 4, 0) },
												{ Point3D(0, 0, 1), Point3D(0, 0, 3), Point3D(0, 0, 4) },
											 };
		const double(&testResults)[count] = { 0.5, -0.5, 1.5, 1.5, 1.5, 1.5 };

		for (int i = 0; i < count; i++)
		{
			double res = GetPointRelativePosition(testCases[i][0], testCases[i][1], testCases[i][2]);
			if (res == testResults[i])
			{
				std::cout << i << ": success\n"; 
			}
			else
			{
				std::cout << i << ": failed\n";
			}
		}
		std::cout << "\n";
	}

	void IsInsideTriangleTest()
	{
		std::cout << "IsInsideTriangleTest:\n\n";
		const int count = 10;
		const Point3D(&testCases)[count][3] = { { Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
												{ Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
												{ Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
												{ Point3D(1, 0, 1), Point3D(1, 0, 5), Point3D(5, 2, 3) },
												{ Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
												{ Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
												{ Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
												{ Point3D(3, 1, 1), Point3D(6, 1, 3), Point3D(7, 1, -1) },
												{ Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
												{ Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(5, 1, 3) },
		};
		const Point3D(&testPoints)[count] = { Point3D(5, 1, 3), Point3D(1, 1, 1), Point3D(1, 1, 5), Point3D(3, 1, 3), 
											  Point3D(4, 1, 2), Point3D(3, 1, 5), Point3D(0.5, 1, 3), Point3D(5, 1, 3),
											  Point3D(3, 1, 4.0000000000000005), Point3D(0.99999999999999994, 1, 4) };
		const bool(&testResults)[count] = { true, true, true, true, false, false, false, false, false, false };

		for (int i = 0; i < count; i++)
		{
			std::cout << i << ": ";

			bool res0 = IsInsideTriangle(testCases[i], testPoints[i]);
			if (res0 == testResults[i])
				std::cout << "Alg1: success. ";
			else
				std::cout << "Alg1: failed.  ";

			Point3D ort;
			CrossProduct(testCases[i][0], testCases[i][1], testCases[i][2], ort);
			bool res1 = IsInsideTriangle(testCases[i], ort, testPoints[i]);
			
			if (res1 == testResults[i])
				std::cout << "Alg2: success.\n";
			else
				std::cout << "Alg2: failed.\n";
		}
		std::cout << "\n";
	}

	void TrianglesIntersectTest()
	{
		std::cout << "TrianglesIntersectTest:\n\n";
		std::ifstream input("test_data.txt");
		std::string line;

		int lineNum = 0;
		while (std::getline(input, line)) {
			lineNum++;
			if (line.find("//") != std::string::npos)
			{
				std::cout << line << "\n";
				continue;
			}

			std::istringstream ss(line);
			double tr0[9];
			double tr1[9];
			bool expectedResult;
			std::string r;

			for (int i = 0; i < 9; i++)
				ss >> tr0[i];
			for (int i = 0; i < 9; i++)
				ss >> tr1[i];
			ss >> r;
			if (r == "true")
				expectedResult = true;
			else if (r == "false")
				expectedResult = false;
			else
			{
				std::cout << lineNum << ": incorrect input\n";
				continue;
			}

			if (AreTrianglesIntersect(tr0, tr1) == expectedResult && AreTrianglesIntersect(tr1, tr0) == expectedResult)
				std::cout << lineNum << ": success\n";
			else
				std::cout << lineNum << ": failed\n";
		}
	}

}