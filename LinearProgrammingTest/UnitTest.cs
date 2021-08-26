using LinearProgrammingSolver;
using LinearProgrammingSolver.Algorithms;
using LinearProgrammingSolver.Problem;
using LinearProgrammingSolver.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinearProgrammingTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test1()
        {
            SimplexSolver solver = (SimplexSolver)Solver.CreateSolver();

            Variable x = solver.MakeNumVar("x");
            Variable y = solver.MakeNumVar("y");
            Variable z = solver.MakeNumVar("y");
            Objective objective = solver.MakeObjective();
            objective.SetCoefficient(x, 3);
            objective.SetCoefficient(y, 1);
            objective.SetCoefficient(z, 2);
            objective.SetMaximization();
            Constraint c1 = solver.MakeConstraint(double.NegativeInfinity, 30);
            c1.SetCoefficient(x);
            c1.SetCoefficient(y);
            c1.SetCoefficient(z, 3);
            Constraint c2 = solver.MakeConstraint(double.NegativeInfinity, 24);
            c2.SetCoefficient(x, 2);
            c2.SetCoefficient(y, 2);
            c2.SetCoefficient(z, 5);
            Constraint c3 = solver.MakeConstraint(double.NegativeInfinity, 36);
            c3.SetCoefficient(x, 4);
            c3.SetCoefficient(y, 1);
            c3.SetCoefficient(z, 2);
            solver.Solve(new SimplexAlgorithm());

            Assert.AreEqual(8, x.Value());
            Assert.AreEqual(4, y.Value());
            Assert.AreEqual(0, z.Value());
        }

        [TestMethod]
        public void Test2()
        {
            SimplexSolver solver = (SimplexSolver)Solver.CreateSolver();

            Variable x = solver.MakeNumVar("x");
            Variable y = solver.MakeNumVar("y");
            Objective objective = solver.MakeObjective();
            objective.SetCoefficient(x, 3);
            objective.SetCoefficient(y, 4);
            objective.SetMaximization();
            Constraint c1 = solver.MakeConstraint(double.NegativeInfinity, 14);
            c1.SetCoefficient(x);
            c1.SetCoefficient(y, 2);
            Constraint c2 = solver.MakeConstraint(0, double.PositiveInfinity);
            c2.SetCoefficient(x, 3);
            c2.SetCoefficient(y, -1);
            Constraint c3 = solver.MakeConstraint(double.NegativeInfinity, 2);
            c3.SetCoefficient(x, 1);
            c3.SetCoefficient(y, -1);
            solver.Solve(new SimplexAlgorithm());

            Assert.AreEqual(34, objective.Value());
        }

        [TestMethod]
        public void Test3()
        {
            SimplexSolver solver = (SimplexSolver)Solver.CreateSolver();

            Variable x = solver.MakeNumVar("x");
            Variable y = solver.MakeNumVar("y");

            Objective objective = solver.MakeObjective();
            objective.SetCoefficient(x);
            objective.SetCoefficient(y);
            objective.SetMaximization();

            Constraint c = solver.MakeConstraint(double.MinValue, 8);
            c.SetCoefficient(x);
            c.SetCoefficient(y);

            solver.Solve(new SimplexAlgorithm());

            Assert.AreEqual(8, objective.Value());
        }
    }
}
