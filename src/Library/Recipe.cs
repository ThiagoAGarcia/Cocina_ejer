//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full_GRASP_And_SOLID
{
    // La clase Recipe implementa la interfaz IRecipeContent, cumpliendo con el Principio de Inversión de Dependencias (DIP)
    public class Recipe : IRecipeContent
    {
        // Lista de pasos de la receta, utiliza el tipo BaseStep para cumplir con el Principio de Abierto/Cerrado (OCP)
        private IList<BaseStep> steps = new List<BaseStep>();
        // Variables para controlar el estado de la cocción
        public bool enProceso; // Indica si la receta ya está en proceso de cocción
        public bool Cocinadito { get; private set; } // Indica si la receta ya ha terminado de cocinarse

        // Producto final que se obtiene al completar la receta
        public Product FinalProduct { get; set; }
        // Agregado por Creator
        // Método para agregar un paso de tipo Step (que involucra ingredientes y equipo) a la receta
        public void AddStep(Product input, double quantity, Equipment equipment, int time)        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        // Método sobrecargado para agregar un paso de espera (sin equipo o ingredientes)
        public void AddStep(string description, int time)        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        // Método para eliminar un paso específico de la receta
        public void RemoveStep(BaseStep step)        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        // Método que genera el texto a imprimir para la receta completa

        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            // Agrega el costo de producción total al final del texto
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        // Calcula el costo de producción de la receta sumando el costo de cada paso

        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result =+ step.GetStepCost();// Suma el costo de cada paso
            }

            return result;
        }

        // Método para calcular el tiempo total de cocción de todos los pasos de la receta
        public int SaberTiempoCoccion()
        {
            int counter = 0;
            foreach (var step in steps)
            {
                counter += step.Time; // Suma el tiempo de cada paso
            }
            return counter;
        }
        
        // Método para marcar la receta como cocinada al completar el proceso de cocción
        public void CoccionTrue()
        {
            Cocinadito = true;
        }
        
        // Método asíncrono para iniciar el proceso de cocción de la receta
        public async void Coccion()
        {
            int tiempo_de_coccion = SaberTiempoCoccion(); // Obtiene el tiempo total de cocción
            if (enProceso)
            {
                // Si la receta ya está en proceso de cocción, lanza una excepción
                throw new InvalidOperationException("Ya se encuentra esa receta en produccion");
            }
            await Task.Delay(tiempo_de_coccion); // Espera el tiempo de cocción en milisegundos
            enProceso = true; // Marca la receta como en proceso
            TemporizadorDeCoccion tempo = new TemporizadorDeCoccion(this); // Crea un temporizador para manejar el tiempo de cocción
            tempo.TimeOut(); // Llama al método TimeOut para completar la cocción
        }
    }
}