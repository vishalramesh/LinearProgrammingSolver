using System.Collections.Generic;

using LinearProgrammingSolver.Algorithms;
using LinearProgrammingSolver.Problem;
using LinearProgrammingSolver.Solution;

namespace LinearProgrammingSolver
{
    /**
     * <summary>Parent class to all Solvers in library. Provides interface for basic procedures of library.</summary>
     */
    public abstract class Solver
    {
        private Objective objective = null;

        private readonly List<Variable> variables = new();
        private readonly List<Constraint> constraints = new();

        /**
         * <summary>Returns Objective instance associated with solver.</summary>
         */
        public Objective Objective => objective;

        /**
         * <summary>Interface for Solver factory.</summary>
         * <param name="type">String representing solver type. Not case sensitive.</param>
         * <returns>Instance of Solver created by factory.</returns>
         */
        public static Solver CreateSolver(string type = "default")
        {
            return SolverFactory.CreateSolver(type);
        }

        /**
         * <summary>Solve the described linear programming problem.</summary>
         * <param name="visitor">Accepts an algorithm (implementation of IAlgorithm) as visitor.</param>
         */
        public void Solve(IAlgorithm visitor)
        {
            visitor.Evaluate(this);
        }

        /**
         * <summary>Creates an empty Objective associated with the Solver.</summary>
         * <returns>Empty instance of Objective associated with the Solver.</returns>
         */
        public Objective MakeObjective()
        {
            objective = new Objective(this);
            return objective;
        }

        // Helper method for MakeNumVar(string name) for extensibility. 
        internal Variable MakeNumVar(string name, double min, double max)
        {
            Variable v = new(this, name, min, max);
            variables.Add(v);
            return v;
        }

        /**
         * <summary>Creates new Variable instance associated with solver that is >= 0.</summary>
         * <param name="name">Name of variable as string.</param>
         */
        public Variable MakeNumVar(string name)
        {
            return MakeNumVar(name, 0, double.PositiveInfinity);
        }

        /**
         * <summary>Creates a Constraint assoiated with Solver: min <= ax + ... + by <= max.</summary>
         * <param name="min">Lower bound on constraint with above format.</param>
         * <param name="max">Upper bound on constraint with above format.</param>
         * <returns>Instance of Constraint with given min and max value.</returns>
         */
        public Constraint MakeConstraint(double min, double max)
        {
            Constraint c = new(this, min, max);
            constraints.Add(c);
            return c;
        }

        internal List<Variable> Variables => variables;
        internal List<Constraint> Constraints => constraints;
    }
}
