using LinearProgrammingSolver;
using LinearProgrammingSolver.Algorithms;
using LinearProgrammingSolver.Problem;
using LinearProgrammingSolver.Solution;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LinearProgrammingGraphic
{
    public class Window : Form
    {
        TextBox xText = new TextBox();
        TextBox yText = new TextBox();

        TextBox minConstr1 = new TextBox();
        TextBox xConstr1 = new TextBox();
        TextBox yConstr1 = new TextBox();
        TextBox maxConstr1 = new TextBox();

        TextBox minConstr2 = new TextBox();
        TextBox xConstr2 = new TextBox();
        TextBox yConstr2 = new TextBox();
        TextBox maxConstr2 = new TextBox();

        Button submit = new Button();

        SimplexSolver solver = (SimplexSolver)Solver.CreateSolver();
        readonly Objective objective;
        Variable x;
        Variable y;
        Constraint c1;
        Constraint c2;

        public Window()
        {
            x = solver.MakeNumVar("x");
            y = solver.MakeNumVar("y");
            objective = solver.MakeObjective();
            c1 = solver.MakeConstraint(0, 0);
            //c2 = solver.MakeConstraint(0, 0);

            Text = "Linear Programming Solver";

            xText.Location = new Point(60, 30);
            Controls.Add(xText);
            yText.Location = new Point(60, 46);
            Controls.Add(yText);

            minConstr1.Location = new Point(60, 95);
            Controls.Add(minConstr1);
            xConstr1.Location = new Point(60, 115);
            Controls.Add(xConstr1);

            yConstr1.Location = new Point(60, 130);
            Controls.Add(yConstr1);
            maxConstr1.Location = new Point(60, 145);
            Controls.Add(maxConstr1);

            submit.Location = new Point(220, 345);
            submit.Text = "Solve";
            submit.Width = 170;
            submit.Height = 40;
            submit.BackColor = Color.LightGray;
            submit.Font = new Font("Arial", 10);
            this.Controls.Add(submit);
            submit.Click += OnButtonClick;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            Graphics g = args.Graphics;
            Font drawFont = new Font("Arial", 14);
            Font textFont = new Font("Arial", 12);
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            g.DrawString("Objective", drawFont, blackBrush, 45, 10);
            g.DrawString("x", textFont, blackBrush, 45, 30);
            
            g.DrawString("y", textFont, blackBrush, 45, 45);

            g.DrawString("Constraint", drawFont, blackBrush, 45, 75);

            g.DrawString("min", textFont, blackBrush, 45, 95);
            g.DrawString("x", textFont, blackBrush, 45, 115);
            g.DrawString("y", textFont, blackBrush, 45, 130);
            g.DrawString("max", textFont, blackBrush, 45, 145);

            g.DrawString(solver.Status.ToString(), drawFont, blackBrush, 60, 190);
        }

        public void OnButtonClick(Object sender, EventArgs e)
        {
            solver.Objective.SetCoefficient(x, double.Parse(xText.Text));
            solver.Objective.SetCoefficient(y, double.Parse(yText.Text));

            double min1 = double.NegativeInfinity;
            //double min2 = double.NegativeInfinity;
            double max1 = double.PositiveInfinity;
            //double max2 = double.PositiveInfinity;

            if (!minConstr1.Text.Equals("inf"))
            {
                min1 = double.Parse(minConstr1.Text);
            }
            if (!maxConstr1.Text.Equals("inf"))
            {
                max1 = double.Parse(maxConstr1.Text);
            }

            c1.SetMinimum(min1);
            c1.SetMaximum(max1);
            c1.SetCoefficient(x, double.Parse(xConstr1.Text));
            c1.SetCoefficient(y, double.Parse(yConstr1.Text));

            
            solver.Solve(new SimplexAlgorithm());
            Invalidate();
        }

        public void DrawObjective()
        {

        }
    }
}
