using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent
    {
        private IList<BaseStep> steps = new List<BaseStep>();
        public bool enProceso;
        public bool Cocinadito { get; private set; }
        public Product FinalProduct { get; set; }

        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }
            result = result + $"Costo de producción: {this.GetProductionCost()}";
            return result;
        }

        public double GetProductionCost()
        {
            double result = 0;
            foreach (BaseStep step in this.steps)
            {
                result += step.GetStepCost();
            }
            return result;
        }

        public int SaberTiempoCoccion()
        {
            int counter = 0;
            foreach (var step in steps)
            {
                counter += step.Time;
            }
            return counter;
        }

        public void CoccionTrue()
        {
            Cocinadito = true;
        }

        public async void Coccion()
        {
            int tiempo_de_coccion = SaberTiempoCoccion();
            if (enProceso)
            {
                throw new InvalidOperationException("Ya se encuentra esa receta en produccion");
            }
            await Task.Delay(tiempo_de_coccion);
            enProceso = true;
            Adaptador tempo = new Adaptador(this);
            tempo.TimeOut();
        }
    }
}
