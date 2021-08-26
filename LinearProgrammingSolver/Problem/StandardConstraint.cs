namespace LinearProgrammingSolver.Problem
{
    // Constraint of the form ax + by + ... + cz <= u.
    // i.e. no lowerbound
    internal class StandardConstraint : Constraint
    {
        public StandardConstraint(Solver solver, double min, double max) : base(solver, min, max)
        {
        }

        // Flips values in the coefficients dictionary field.
        public void FlipCoefficients()
        {
            foreach (Variable var in coefficients.Keys)
            {
                coefficients[var] = -coefficients[var];
            }
        }
    }
}
