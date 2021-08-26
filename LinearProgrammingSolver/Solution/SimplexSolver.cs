using LinearProgrammingSolver.Algorithms;
using LinearProgrammingSolver.Problem;
using System.Collections.Generic;

namespace LinearProgrammingSolver.Solution
{
    /**
     * <summary>Implementation of Solver for running simplex algorithm.</summary>
     */
    public class SimplexSolver : Solver
    {
        private bool logging = true;

        public bool Logging => logging;

        internal readonly List<StandardConstraint> standardConstraints = new();

        public enum Solution { Infeasible, Unbounded, Feasible }

        internal Solution status;

        internal double constant = 0;

        /**
         * <summary>Possible values are: Infeasible, Unbounded and Feasible.</summary>
         */
        public Solution Status => status;

        internal SimplexSolver() { }

        /**
         * <summary>Sets solver to output logging info.</summary>
         * <param name="val">True if logging must be turned on.</param>
         */ 
        public void SetLogging(bool val = true)
        {
            logging = val;
        }

        /**
         * <summary>Sets constant term in the objective function of linear program.</summary>
         * <param name="constant">Constant term in objective function.</param>
         */
        public void SetObjectiveConstant(double constant)
        {
            this.constant = constant;
        }

        /**
         * <summary>Takes visitor class as parameter representing algorithm and runs it on solver.</summary>
         * <param name="visitor">Algorithm subclass as visitor</param>
         */
        public void Solve(SimplexAlgorithm visitor)
        {
            visitor.Evaluate(this);
        }

        /**
         * <summary>Sets values of all created variables associated with solver.</summary>
         * <param name="x">Array of variable values ordered by creation order.</param>
         */
        public void SetValues(double[] x)
        {
            for (int i = 0; i < Variables.Count; ++i)
            {
                Variables[i].SetValue(x[i]);
            }
        }
    }
}
