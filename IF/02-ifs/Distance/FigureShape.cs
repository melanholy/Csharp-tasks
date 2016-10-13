using TestingRoom;

namespace Distance
{
	public static class FigureShape
	{
		public static void DrawFigure(TestCaseUI ui)
		{
			ui.Line(-50, -10, -50, 10, TestCase.neutralPen);
			ui.Line(50, -10, 50, 10, TestCase.neutralPen);
			ui.Line(-50, 10, 0, 60, TestCase.neutralPen);
			ui.Line(50, 10, 0, 60, TestCase.neutralPen);
			ui.Arc(0, -10, 50, 180, 180, TestCase.neutralPen);
		}
	}
}