using System;
using System.Collections.Generic;
using System.Text;

namespace LuxStateMachine.YUmlGenerator
{
    public static class YUmlGenerator
    {
        public enum DiagramType
        {
            Class,
            Activity
        }

        public enum OutputFormat
        {
            Pdf,
            Svg,
            Png,
            Jpeg,
            Json
        }
        
        public static string GetDiagramUrl<TState>(StateMachine<TState> stateMachine, DiagramType diagramType, OutputFormat outputFormat)
        {
            return GetDiagramUrl(stateMachine, EnumTools.GetValues<TState>(), diagramType, outputFormat);
        }

        public static string GetDiagramUrl<TState>(StateMachine<TState> stateMachine, IEnumerable<TState> possibleStates, DiagramType diagramType, OutputFormat outputFormat)
        {
            var output = new StringBuilder();

            foreach (var state in possibleStates)
            {
                var stateConfiguration = stateMachine.GetStateConfiguration(state);
                foreach (var actionConfiguration in stateConfiguration.Actions)
                {
                    if (actionConfiguration.PossibleStatesAfter.Count == 0)
                    {
                        output.Append("," + GetChunk(diagramType, state, actionConfiguration.ActionType.Name, state));
                    }
                    else
                    {
                        foreach (var possibleState in actionConfiguration.PossibleStatesAfter)
                        {
                            output.Append("," + GetChunk(diagramType, state, actionConfiguration.ActionType.Name, possibleState));
                        }
                    }
                }
            }

            return GetPrefix(diagramType) + output.ToString().TrimStart(',') + ".svg";
        }

        private static string GetChunk<TState>(DiagramType diagramType, TState initialState, string action, TState targetState)
        {
            switch (diagramType)
            {
                case DiagramType.Class: return string.Format("[{0}]-{1}>[{2}]", initialState, action, targetState);
                case DiagramType.Activity: return string.Format("({0}){1}->({2})", initialState, action, targetState);
                default: throw new ArgumentOutOfRangeException("diagramType");
            }
        }

        private static string GetPrefix(DiagramType diagramType)
        {
            switch (diagramType)
            {
                case DiagramType.Class: return "http://yuml.me/diagram/scruffy;dir:LR;scale:120;/class/";
                case DiagramType.Activity: return "http://yuml.me/diagram/nofunky/activity/";
                default: throw new ArgumentOutOfRangeException("diagramType");
            }
        }
    }
}