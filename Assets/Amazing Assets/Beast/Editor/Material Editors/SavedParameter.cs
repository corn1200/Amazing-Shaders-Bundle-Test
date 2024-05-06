using System;

using UnityEngine.Assertions;


namespace UnityEditor.Rendering.Universal
{
    class BeastSavedParameter<T>
        where T : IEquatable<T>
    {
        internal delegate void SetParameter(string key, T value);
        internal delegate T GetParameter(string key, T defaultValue);

        readonly string m_Key;
        bool m_Loaded;
        T m_Value;

        readonly SetParameter m_Setter;
        readonly GetParameter m_Getter;

        public BeastSavedParameter(string key, T value, GetParameter getter, SetParameter setter)
        {
            Assert.IsNotNull(setter);
            Assert.IsNotNull(getter);

            m_Key = key;
            m_Loaded = false;
            m_Value = value;
            m_Setter = setter;
            m_Getter = getter;
        }

        void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;
            m_Value = m_Getter(m_Key, m_Value);
        }

        public T value
        {
            get
            {
                Load();
                return m_Value;
            }
            set
            {
                Load();

                if (m_Value.Equals(value))
                    return;

                m_Value = value;
                m_Setter(m_Key, value);
            }
        }
    }

    // Pre-specialized class for easier use and compatibility with existing code
    sealed class BeastSavedBool : BeastSavedParameter<bool>
    {
        public BeastSavedBool(string key, bool value)
            : base(key, value, EditorPrefs.GetBool, EditorPrefs.SetBool) { }
    }

    sealed class BeastSavedInt : BeastSavedParameter<int>
    {
        public BeastSavedInt(string key, int value)
            : base(key, value, EditorPrefs.GetInt, EditorPrefs.SetInt) { }
    }

    sealed class BeastSavedFloat : BeastSavedParameter<float>
    {
        public BeastSavedFloat(string key, float value)
            : base(key, value, EditorPrefs.GetFloat, EditorPrefs.SetFloat) { }
    }

    sealed class BeastSavedString : BeastSavedParameter<string>
    {
        public BeastSavedString(string key, string value)
            : base(key, value, EditorPrefs.GetString, EditorPrefs.SetString) { }
    }
}