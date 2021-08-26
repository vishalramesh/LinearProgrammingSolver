using LinearProgrammingSolver.Problem;
using LinearProgrammingSolver.Solution;
using System.Collections.Generic;
using System;

namespace LinearProgrammingSolver.Algorithms
{
    // Problem is transformed into slack representation for running pivot operations.
    internal class Slack
    {
        // Nonbasic variables appear in the objective function.
        private HashSet<int> nonbasic = new HashSet<int>();
        private HashSet<int> basic = new HashSet<int>();

        public double[,] A; // Represents coefficients in constraint inequalities.
        public double[] b; // Represents constant terms in constraint inequalities.
        public double[] c; // Represents coefficients in objective function.
        public double v; // Represents constant term in objective function.

        public HashSet<int> Nonbasic => nonbasic;
        public HashSet<int> Basic => basic;

        internal bool infeasible = false;

        /*
         * Prints out representation of matrix A of slack form.
         * For example, A = [[1, 2, 3], [2, 1, 1], [1, 11, 4]] would output:
         * 1  2  3
         * 2  1  1
         * 1  11  4
         */
        public void Print()
        {
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    Console.Write(A[i, j] + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Initialize Slack for running Simplex algorithm.
        internal Slack(SimplexSolver solver)
        {
            if (!InitialFeasible(solver))
            {
                // If the initial basic solution is not feasible,
                // initiate checking procedure.
                CheckFeasible(solver);
            }
            
            int n = solver.Variables.Count;
            int m = solver.standardConstraints.Count;

            for (int i = 0; i < n; ++i)
            {
                nonbasic.Add(i);
            }
            for (int i = n; i < n + m; ++i)
            {
                basic.Add(i);
            }

            A = new double[n + m, n + m]; // Dimensions n+m because essentially creates m new slack variables.

            for (int i = n; i < n + m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Variable v = solver.Variables[j];
                    if (solver.standardConstraints[i - n].coefficients.ContainsKey(v))
                    {
                        A[i, j] = solver.standardConstraints[i - n].coefficients[v];
                    }
                }
            }

            // m new slack variables are created to measure the slack and optimize the objective function.
            b = new double[n + m];
            c = new double[n + m];

            for (int i = 0; i < m; ++i)
            {
                b[n + i] = solver.standardConstraints[i].Max;
            }

            for (int i = 0; i < n; ++i)
            {
                double x = 0;
                if (solver.Objective.coefficients.ContainsKey(solver.Variables[i]))
                {
                    x =  solver.Objective.coefficients[solver.Variables[i]];
                }

                if (!solver.Objective.Mazimize)
                {
                    x = -x;
                }

                c[i] = x;
            }
            
            v = solver.constant;
        }

        // Procedure as described in 2nd edition of Introduction to Algorithms by CLRS.
        private void CheckFeasible(SimplexSolver solver)
        {
            // Form L_aux by adding −x0 to the left-hand side of each equation
            // and setting the objective function to −x0.
            SimplexSolver s2 = (SimplexSolver)Solver.CreateSolver();
            Variable x0 = s2.MakeNumVar("x000");
            foreach (Variable v in solver.Variables)
            {
                Variable _ = s2.MakeNumVar(v.Name);
            }
            Objective obj = s2.MakeObjective();
            obj.SetCoefficient(x0, -1);

            foreach (Constraint c in solver.Constraints)
            {
                Constraint c2 = s2.MakeConstraint(c.Min, c.Max);
                int i = 0;
                foreach (Variable v in c.coefficients.Keys)
                {
                    c2.SetCoefficient(s2.Variables[i], c.coefficients[v]);
                    i++;
                }
                c2.SetCoefficient(x0, -1);
            }
            // Then convert L_aux to slack form.
            Slack slack = new Slack(s2);
            int l = 0;
            double xx = double.PositiveInfinity;
            for (int i = 0; i < solver.standardConstraints.Count; ++i)
            {
                if (solver.standardConstraints[i].Max < xx)
                {
                    xx = solver.standardConstraints[i].Max;
                    l = i;
                }
            }
            slack = new SimplexAlgorithm().Pivot(slack, l, 0); // The basic solution is now feasible for L_aux.

            double[] d = new SimplexAlgorithm().Simplex(slack);

            // If the basic solution sets x0 to 0
            // then return the final slack form with x0 removed and
            // with the original objective function restored.
            if (d[0] != 0)
            {
                slack.infeasible = true;
                solver.status = SimplexSolver.Solution.Infeasible;
                return;
            }

            nonbasic = slack.Nonbasic;
            nonbasic.Remove(slack.Nonbasic.Count - 1);
            basic = slack.Basic;
            basic.Remove(slack.Nonbasic.Count - 1);
            v = slack.v;

            // ab and c
            b = slack.b;
            A = new double[slack.A.GetLength(0), slack.A.GetLength(1) - 1];
            for (int i = 0; i < A.GetLength(0); ++i)
            {
                for (int j = 0; j < A.GetLength(1); ++j)
                {
                    A[i, j] = slack.A[i, j + 1];
                }
            }

            c = new double[slack.c.Length - 1];
            for (int i = 0; i < c.Length; ++i)
            {
                double x = 0;
                if (solver.Objective.coefficients.ContainsKey(solver.Variables[i]))
                {
                    x = solver.Objective.coefficients[solver.Variables[i]];
                }

                if (!solver.Objective.Mazimize)
                {
                    x = -x;
                }

                c[i] = x;
            }
        }

        // Checks with initial basic solution of SimplexSolver is feasible.
        private static bool InitialFeasible(SimplexSolver solver)
        {
            foreach (StandardConstraint s in solver.standardConstraints)
            {
                if (s.Max >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
