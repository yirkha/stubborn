using System;

namespace Stubborn
{
    /// <summary>
    /// Customize the name of this property when serialized to YAML
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class YamlNameAttribute : Attribute
    {
        public YamlNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Custom name of this property as it should appear in YAML
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Customize the position of this property within the serialized object
    /// </summary>
    /// <remarks>
    /// Property order can be trivially changed within one object, but not as
    /// easily across the inheritance chain. This attribute makes that doable.
    /// </remarks>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class YamlOrderAttribute : Attribute
    {
        /// <summary>
        /// Customize the position of this property within the serialized object
        /// </summary>
        /// <param name="order">
        /// Position index of this property within the object
        /// </param>
        public YamlOrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Position index of this property within the object
        /// </summary>
        /// <remarks>
        /// By default all properties are assigned sequential numbers,
        /// starting at zero.
        /// </remarks>
        public int Order { get; set; }

        /// <summary>
        /// Determines whether to affect also the ordering of all subsequent
        /// properties after this one
        /// </summary>
        /// <remarks>
        /// By default any subsequent properties (without this attribute) that
        /// will continue using the specified order, increased by one per every
        /// property. Set this option to set the relative order of this
        /// property only without affecting the following ones.
        /// </remarks>
        public bool ThisOnly { get; set; }
    }

    /// <summary>
    /// Avoid serializing this property to YAML
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class YamlIgnoreAttribute : Attribute
    {
        public YamlIgnoreAttribute()
        {
        }

        /// <summary>
        /// Do not write property to YAML if its value equals this parameter.
        /// </summary>
        public object IfEquals { get; set; }

        /// <summary>
        /// Do not write property to YAML if its stringified value equals this
        /// parameter.
        /// </summary>
        /// <remarks>
        /// Can be used to work around the limitation of .NET attributes to be
        /// initialized only with constant expressions.
        /// </remarks>
        public string IfEqualsStr { get; set; }

        /// <summary>
        /// Do not write property to YAML if it is empty.
        /// </summary>
        /// <remarks>
        /// Valid only for <c>IEnumerable</c> properties (includes string).
        /// </remarks>
        public bool IfEmpty { get; set; }
    }

    /// <summary>
    /// Customize formatting of this property when serialized to YAML
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class YamlFormatAttribute : Attribute
    {
        /// <summary>
        /// Customize formatting of this property when serialized to YAML
        /// </summary>
        /// <param name="format">Optional format string for ToString()</param>
        public YamlFormatAttribute(string format = null)
        {
            Format = format;
        }

        /// <summary>
        /// Set to serialize by ToString() instead of traversing public
        /// properties
        /// </summary>
        /// <remarks>
        /// Affects object instances only.
        /// </remarks>
        public new bool ToString { get => throw new NotImplementedException(); set => Format = value ? "" : null; }

        // Some properties need to be defined twice to allow differentiating
        // between e.g. 0 or false and no value set at all. Attribute params
        // themselves cannot be nullable because of CLR restrictions.

        /// <summary>
        /// Number of blank lines written before this property/item
        /// </summary>
        /// <remarks>
        /// The number of actual blank lines used might be higher based on the
        /// number of blank lines configured after the preceding property/item.
        /// </remarks>
        public int BlankLinesBefore { get => throw new NotImplementedException(); set => MaybeBlankLinesBefore = value; }

        /// <summary>
        /// Number of blank lines written after this property/item
        /// </summary>
        /// <remarks>
        /// The number of actual blank lines used might be higher based on the
        /// number of blank lines configured before the next property/item.
        /// </remarks>
        public int BlankLinesAfter { get => throw new NotImplementedException(); set => MaybeBlankLinesAfter = value; }

        /// <summary>
        /// Set to serialize as a nested list even if it contains just 1 item
        /// </summary>
        /// <remarks>
        /// This option has any effect only on enumerable properties/items.
        /// </remarks>
        public bool AlwaysNested { get => throw new NotImplementedException(); set => MaybeAlwaysNested = value; }

        /// <summary>
        /// Set to always serialize the value as a quoted flow string
        /// </summary>
        /// <remarks>
        /// Single quotes are usually prefered for readability (less escaping).
        /// This option has effect only on values serialized as a string.
        /// </remarks>
        public bool Quoted { get => throw new NotImplementedException(); set => MaybeQuoted = value; }

        /// <summary>
        /// Set to always serialize the value as a double quoted flow string
        /// </summary>
        /// <remarks>
        /// This option has effect only on values serialized as a string.
        /// </remarks>
        public bool DoubleQuoted { get => throw new NotImplementedException(); set => MaybeDoubleQuoted = value; }

        /// <summary>
        /// Set to always serialize the value as an indented block
        /// </summary>
        /// <remarks>
        /// This option has effect only on values serialized as a string.
        /// </remarks>
        public bool Block { get => throw new NotImplementedException(); set => MaybeBlock = value; }

        /// <summary>
        /// Number of spaces per indentation step for this property/class
        /// </summary>
        public int IndentStep { get => throw new NotImplementedException(); set => MaybeIndentStep = value; }

        internal string Format { get; set; }
        internal int? MaybeBlankLinesBefore { get; set; }
        internal int? MaybeBlankLinesAfter { get; set; }
        internal bool? MaybeAlwaysNested { get; set; }
        internal bool? MaybeQuoted { get; set; }
        internal bool? MaybeDoubleQuoted { get; set; }
        internal bool? MaybeBlock { get; set; }
        internal int? MaybeIndentStep { get; set; }
    }

    /// <summary>
    /// Add a YAML comment associated with this property/item
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class YamlCommentAttribute : Attribute
    {
        /// <param name="text">Comment text</param>
        /// <param name="style">Comment appearance style</param>
        public YamlCommentAttribute(string text = null, YamlCommentStyle style = YamlCommentStyle.SmallAbove)
        {
            Style = style;
            Text = text;
        }

        public YamlCommentStyle Style { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// Read the actual comment contents from another, possibly dynamically
        /// generated string property of the object
        /// </summary>
        /// <remarks>
        /// Return <c>null</c> to avoid writting the comment formatting at all.
        /// </remarks>
        public string Property { get; set; } = null;

        internal bool TryGetText(object obj, out string text)
        {
            text = null;

            if (Property != null)
            {
                var textProp = obj.GetType().GetProperty(Property);
                if (textProp != null)
                {
                    text = textProp.GetValue(obj)?.ToString();
                }
            }

            if (text == null)
            {
                text = Text;
            }

            return (text != null);
        }
    }
}
