using System;

#nullable enable

namespace Nananami.Lib.CmdSys
{
    public struct CommandVariable
    {
        public enum Type
        {
            Int,
            String,
            Float,
            Boolean,
        }

        public Type CurrentType;
        private int m_int_value;
        private string m_string_value;
        private float m_float_value;
        private bool m_bool_value;

        public void SetValue<T>(T val)
        {
            switch (val)
            {
                case int i:
                    CurrentType = Type.Int;
                    m_int_value = i;
                    break;
                case string s:
                    CurrentType = Type.String;
                    m_string_value = s;
                    break;
                case float f:
                    CurrentType = Type.Float;
                    m_float_value = f;
                    break;
                case bool b:
                    CurrentType = Type.Boolean;
                    m_bool_value = b;
                    break;
                default:
                    throw new ArgumentException($"Unsupported type: {typeof(T)}");
            }
        }

        public void Match(
            Action<int>? intCase,
            Action<string>? stringCase,
            Action<float>? floatCase,
            Action<bool>? boolCase
        )
        {
            switch (CurrentType)
            {
                case Type.Int: intCase?.Invoke(m_int_value); break;
                case Type.String: stringCase?.Invoke(m_string_value); break;
                case Type.Float: floatCase?.Invoke(m_float_value); break;
                case Type.Boolean: boolCase?.Invoke(m_bool_value); break;
                default: throw new InvalidOperationException($"Invalid type: {CurrentType}");
            }
        }
    }
}