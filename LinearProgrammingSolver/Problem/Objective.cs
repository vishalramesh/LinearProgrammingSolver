using System.Collections.Generic;

namespace LinearProgrammingSolver.Problem
{
    /**
     * <summary>Represents a linear expression for the objective function of a linear program.</summary>
     */
    public class Objective
    {
        // Keeps track of coefficients of variables appearing in objective function.
        // Variables not in dictionary have coefficient value of zero.
        internal readonly Dictionary<Variable, double> coefficients = new Dictionary<Variable, double>();

        private bool maximize = true;

        /**
         * <summary>True if optimization goal is to maximize given objective function.</summary>
         */
        public bool Mazimize => maximize;

        // Value on evaluating objective.
        private double value;

        private readonly Solver solver;

        internal Objective(Solver solver) 
        {
            this.solver = solver;
        }

        /**
         * <summary>Sets solver to maximize objective value.</summary>
         */ 
        public void SetMaximization()
        {
            maximize = true;
        }

        /**
         * <summary>Sets solver to minimize objective value.</summary>
         */ 
        public void SetMinimization()
        {
            maximize = false;
        }

        /**
         * <summary>Sets coefficient of variables appearing in objective function.</summary>
         * <param name="var">Instance of <c>Variable</c> appearing in objective function.</param>
         * <param name="coefficient">Coefficient of corresponding variable in objective function.</param>
         */
        public void SetCoefficient(Variable var, double coefficient = 1)
        {
            coefficients[var] = coefficient;
        }

        /**
         * <summary>Objective value calculated from variable values in array x.</summary>
         * <param name="x">Array x of values of variables in order of creation.</param>
         */
        public void CalculateValue(double[] x)
        {
            value = 0;
            int count = 0;
            foreach (Variable var in solver.Variables)
            {
                if (coefficients.ContainsKey(var))
                {
                    value += coefficients[var] * x[count];
                }
            }
        }

        /**
         * <summary>Objective value calculated from variable values.</summary>
         */
        public void CalculateValue()
        {
            value = 0;
            foreach (Variable var in solver.Variables)
            {
                if (coefficients.ContainsKey(var))
                {
                    value += coefficients[var] * var.Value();
                }
            }
        }

        /**
         * <summary>Get objective value of a feasible bounded linear program after solving.</summary>
         * <returns>Optimal objective value following evaluation.</returns>
         */
        public double Value()
        {
            return value;
        }
    }
}
