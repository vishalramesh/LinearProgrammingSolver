using System;

namespace LinearProgrammingSolver.Solution
{
    // Solver factory makes library extensible.
    internal class SolverFactory
    {
        private static readonly string def = "default";
        private static readonly string simplex = "simplex";

        // Factory method returns child class of solver based on type input argument.
        // Argument is not case sensitive for better user experience.
        public static Solver CreateSolver(string type)
        {
            string typeLower = type.ToLower();

            if (typeLower.Equals(def))
            {
                typeLower = simplex;
            }

            if (typeLower.Equals(simplex))
            {
                return new SimplexSolver();
            }
            else
            {
                string message = string.Format("{0} is not an available solver. \nTry the SimplexSolver by passing the \"simplex\" argument.", type);
                throw new ArgumentException(message);
            }
        }
    }
}