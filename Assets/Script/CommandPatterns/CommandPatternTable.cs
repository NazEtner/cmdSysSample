using System.Collections.Generic;

namespace Nananami.CommandPatterns
{
    public class CommandPatternTable
    {
        public void AddCommandPattern(string name, CommandPattern cmdPattern)
        {
            m_patterns.Add(name, cmdPattern);
        }

        public CommandPattern GetCommandPattern(string name)
        {
            if (m_patterns.ContainsKey(name))
            {
                return m_patterns[name];
            }

            throw new KeyNotFoundException($"Pattern '{name}' was not found.");
        }

        private Dictionary<string, CommandPattern> m_patterns;
    }
}