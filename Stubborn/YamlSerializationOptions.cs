namespace Stubborn
{
    public enum YamlNamingStyle
    {
        AsIs,
        UpperCamelCase,
        LowerCamelCase
    }

    public enum YamlCommentStyle
    {
        None,
        SameLine,
        SmallAbove,
        MediumAbove,
        LargeAbove
    }

    public class YamlSerializationOptions
    {
        /// <summary>
        /// Number of spaces per indentation step
        /// </summary>
        public int IndentStep { get; set; } = 2;

        /// <summary>
        /// Preferred width of the output in characters
        /// </summary>
        public int Width { get; set; } = 80;

        /// <summary>
        /// Maximum depth of the generated document
        /// </summary>
        /// <remarks>
        /// Every enumerable sequence (list), mapping (dictionary) or
        /// an object with properties is counted as one level of depth.
        /// </remarks>
        public int MaxDepth { get; set; } = 10;

        /// <summary>
        /// Style of the property names when serialized
        /// </summary>
        /// <remarks>
        /// The initial character and any underscores are processed to match
        /// the chosen uppercase/lowercase style.
        /// </remarks>
        public YamlNamingStyle PropertyNamingStyle { get; set; } = YamlNamingStyle.LowerCamelCase;

        /// <summary>
        /// Set to generate YAML document boundaries around the output
        /// </summary>
        public bool DocumentBoundaries { get; set; } = false;

        /// <summary>
        /// Comment to include at the beginning of the output
        /// </summary>
        public string PrologueComment { get; set; } = null;

        /// <summary>
        /// Style of the prologue comment, if specified
        /// </summary>
        public YamlCommentStyle PrologueCommentStyle { get; set; } = YamlCommentStyle.SmallAbove;

        /// <summary>
        /// Number of blank lines following the prologue comment
        /// </summary>
        /// <remarks>
        /// Default: 1 if prologue is set, otherwise 0.
        /// </remarks>
        public int? PrologueMargin { get; set; } = null;

        /// <summary>
        /// Comment to include at the end of the output
        /// </summary>
        public string EpilogueComment { get; set; } = null;

        /// <summary>
        /// Style of the epilogue comment, if specified
        /// </summary>
        public YamlCommentStyle EpilogueCommentStyle { get; set; } = YamlCommentStyle.SmallAbove;

        /// <summary>
        /// Number of blank lines preceeding the epilogue comment
        /// </summary>
        /// <remarks>
        /// Default: 1 if epilogue is set, otherwise 0.
        /// </remarks>
        public int? EpilogueMargin { get; set; } = null;

        /// <summary>
        /// Disables handling of all extra options and attributes
        /// </summary>
        /// <remarks>
        /// Unconditional <c>YamlIgnore</c> attributes are handled at all times
        /// because they often avoid complete serialization failures.</remarks>
        public bool DisableOverrides { get; set; } = false;
    }
}
