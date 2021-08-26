using System;
using LinearProgrammingSolver.Problem;
using LinearProgrammingSolver.Solution;

namespace LinearProgrammingSolver.Algorithms
{
    public class SimplexAlgorithm : IAlgorithm
    {
        private SimplexSolver solver;

        public void Evaluate(Solver solver)
        {
            throw new NotImplementedException();
        }

        /**
         * <summary>Algorithm visitor takes visited solver and runs on the data instance.</summary>
         * <param name="solver">Associated solver instance to the algorithm.</param>
         */
        public void Evaluate(SimplexSolver solver)
        {
            this.solver = solver;

            // Convert constraints to standard form, i.e. no lowerbounded and a <= upper bound.
            foreach (Constraint c in solver.Constraints)
            {
                c.Standardize(solver);
            }

            Slack slack = new(solver);
            if (slack.infeasible)
            {
                return;
            }
            if (solver.Logging)
            {
                slack.Print();
            }

            double[] x = Simplex(slack);

            // Set solution values and status
            if (x != null)
            {
                solver.SetValues(x);
                solver.Objective.CalculateValue();
                solver.status = SimplexSolver.Solution.Feasible;
            }
            
        }

        // Helper method for main Simplex procedure
        private static int ChooseEntering(double[] c)
        {
            // Any variable which appears in objective function with positive coefficient is chosen
            // as the entering variable.
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] > 0)
                {
                    return i;
                }
            }
            return -1;
        }

        // Simplex procedure as described in 2nd edition of Introduction to Algorithms by CLRS.
        // Returns array with an optimal assignment of variable values (including slack variables).
        internal double[] Simplex(Slack initial)
        {
            int e;
            while ((e = ChooseEntering(initial.c)) != -1)
            {
                int del_index = 0;
                double del = double.PositiveInfinity;

                // Choose an index (del_index) that minimizes del as the leaving variable.
                foreach (int i in initial.Basic)
                {
                   if (initial.A[i, e] > 0 && (initial.b[i] / initial.A[i, e]) < del)
                   {
                        del = initial.b[i] / initial.A[i, e];
                        del_index = i;
                   }
                   else if (initial.A[i, e] > 0 && !initial.Basic.Contains(del_index))
                   {
                        del_index = i;
                   }
                }

                if (del == double.PositiveInfinity)
                {
                    solver.status = SimplexSolver.Solution.Unbounded;
                    return null;
                }
                else
                {
                    // If not unbounded, then perform pivot.
                    initial = Pivot(initial, del_index, e);
                }

                if (solver.Logging)
                {
                    initial.Print();
                }
            }

            double[] x = new double[initial.c.Length];

            for (int i = 0; i < initial.c.Length; ++i)
            {
                if (initial.Basic.Contains(i))
                {
                    x[i] = initial.b[i];
                }
                else
                {
                    x[i] = 0;
                }
            }

            return x;
        }

        // Pivot is a basic operation of the simplex algorithm
        // Takes the linear program as slack form, plus leaving and entering variable index
        // Returns new slack form with higher (better) objective value
        internal Slack Pivot(Slack initial, int leaving, int entering)
        {
            double[,] A_new = new double[initial.A.GetLength(0), initial.A.GetLength(1)];
            double[] b_new = new double[initial.b.Length];
            double[] c_new = new double[initial.c.Length];

            initial.Nonbasic.Remove(entering);
            initial.Basic.Remove(leaving);

            // Compute the coefficients of the equation for new basic variable x_e.
            b_new[entering] = initial.b[leaving] / initial.A[leaving, entering];

            foreach (int j in initial.Nonbasic)
            {
                A_new[entering, j] = initial.A[leaving, j] / initial.A[leaving, entering];
            }
            A_new[entering, leaving] = 1 / initial.A[leaving, entering];

            // Compute the coefficients of the remaining constraints.
            foreach (int i in initial.Basic)
            {
                b_new[i] = initial.b[i] - initial.A[i, entering] * b_new[entering];
                foreach (int j in initial.Nonbasic)
                {
                    A_new[i, j] = initial.A[i, j] - (initial.A[i, entering] * A_new[entering, j]);
                }
                A_new[i, leaving] = -initial.A[i, entering] * A_new[entering, leaving];
            }

            initial.v += initial.c[entering] * b_new[entering];

            // Compute the coefficients of the objective function.
            foreach (int j in initial.Nonbasic)
            {
                c_new[j] = initial.c[j] - (initial.c[entering] * A_new[entering, j]);
            }
            c_new[leaving] = -initial.c[entering] * A_new[entering, leaving];

            // New sets of basic and nonbasic variables.
            // Also previously removed entering and leaving variable from
            // nonbasic and basic respectively.
            initial.Nonbasic.Add(leaving);
            initial.Basic.Add(entering);

            initial.A = A_new;
            initial.b = b_new;
            initial.c = c_new;

            return initial;
        }
    }
}
