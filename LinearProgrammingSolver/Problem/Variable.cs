namespace LinearProgrammingSolver.Problem
{
    /**
     * <summary>Represents an instance of a variable in the linear program solver.</summary>
     */ 
    public class Variable
    {
        private readonly string name;
        private readonly Constraint constraint;

        public double Min => constraint.Min;
        public double Max => constraint.Max;

        public string Name => name;

        private readonly Solver solver;

        private double value = 0;

        internal Variable(Solver solver, string name, double min, double max)
        {
            this.name = name;
            this.solver = solver;
            constraint = new(solver, min, max);
            constraint.SetCoefficient(this, 1);
            //solver.Constraints.Add(constraint);
        }

        /**
         * <summary>Set value for variable.</summary>
         * <param name="value">Value to be set.</param>
         */ 
        public void SetValue(double value)
        {
            this.value = value;
        }

        /**
         * <summary>Get value for variable after solving a feasible bounded linear program.</summary>
         * <returns>Optimal value for variable.</returns>
         */
        public double Value()
        {
            return value;
        }
    }
}