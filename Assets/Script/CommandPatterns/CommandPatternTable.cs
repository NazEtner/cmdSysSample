using System.Collections.Generic;
using System.Diagnostics;

namespace Nananami.CommandPatterns
{
    public class CommandPatternTable
    {
        public void AddCommandPattern(string name, CommandPattern cmdPattern)
        {
            m_patterns[name] = cmdPattern;
        }

        public CommandPattern GetCommandPattern(string name)
        {
            if (m_patterns.ContainsKey(name))
            {
                return m_patterns[name];
            }

            throw new KeyNotFoundException($"Pattern '{name}' was not found.");
        }

        private Dictionary<string, CommandPattern> m_patterns = new Dictionary<string, CommandPattern>();
    }
}