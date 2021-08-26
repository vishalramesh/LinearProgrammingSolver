using LinearProgrammingSolver.Solution;
using System.Collections.Generic;

namespace LinearProgrammingSolver.Problem
{
    /**
     * <summary>Represents a linear constraint on an instance of a linear program solver.</summary>
     */
    public class Constraint
    {
        private double min;
        private double max;

        internal readonly Dictionary<Variable, double> coefficients = new Dictionary<Variable, double>();

        /**
         * <summary>Lowerbound of constraint, i.e.  min <= ax + ... + by <= ...</summary>
         */
        public double Min => min;

        /**
         * <summary>Upperbound of constraint, i.e.  ... <= ax + ... + by <= max.</summary>
         */
        public double Max => max;

        internal Constraint(Solver solver, double min, double max)
        {
            this.min = min;
            this.max = max;

            foreach (Variable v in solver.Variables)
            {
                coefficients.Add(v, 0);
            }
        }

        /**
         * <summary>Set min bound on constraint: min <= ax + ... + by <= ... </summary>
         * <param name="min">Minimum value on constraint.</param>
         */
        public void SetMinimum(double min)
        {
            this.min = min;
        }

        /**
         * <summary>Set max bound on constraint: ... <= ax + ... + by <= max </summary>
         * <param name="max">Maximum value on constraint.</param>
         */
        public void SetMaximum(double max)
        {
            this.max = max;
        }

        /**
         * <summary>Sets coefficient of vars appearing in constraint in following form: <c>min <= x + y <= max.</c></summary>
         * <param name="var">Instance of <c>Variable</c> appearing in inequality.</param>
         * <param name="coefficient">Coefficient of corresponding variable in inequality.</param>
         */
        public void SetCoefficient(Variable var, double coefficient = 1)
        {
            coefficients[var] = coefficient;
        }

        internal void Standardize(SimplexSolver solver)
        {
            if (max != double.PositiveInfinity)
            {
                StandardConstraint standard = new StandardConstraint(solver, double.NegativeInfinity, max);
                foreach (Variable v in coefficients.Keys)
                {
                    standard.SetCoefficient(v, coefficients[v]);
                }
                solver.standardConstraints.Add(standard);
                
            }
            if (min != double.NegativeInfinity)
            {
                StandardConstraint standardConstraint = new StandardConstraint(solver, double.NegativeInfinity, -min);
                foreach (Variable v in coefficients.Keys)
                {
                    standardConstraint.SetCoefficient(v, coefficients[v]);
                }
                standardConstraint.FlipCoefficients();
                solver.standardConstraints.Add(standardConstraint);
            }
        }
    }
}