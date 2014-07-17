using System.Diagnostics;
using OrderSample.StateMachine;

namespace LuxStateMachine.YUmlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var stateMachine = new OrderStateMachine();
            var url = YUmlGenerator.GetDiagramUrl(stateMachine, YUmlGenerator.DiagramType.Activity, YUmlGenerator.OutputFormat.Svg);

            Process.Start(url);
        }
    }
}
