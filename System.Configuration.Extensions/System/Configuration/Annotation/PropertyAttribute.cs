namespace System.Configuration.Annotation
{
	/// <summary>
	/// Specifies rules for validation checks on other properties.
	/// </summary>
	/// <param name="type">Property Data Type for Validation</param>
	/// <param name="required">Whether it is a required value</param>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class PropertyAttribute(PropertyType type, bool required = false) : Attribute
	{
		/// <summary>
		/// Property Data Type for Validation
		/// </summary>
		public PropertyType Type => type;

		/// <summary>
		/// Whether it is a required value
		/// </summary>
		public bool Required => required;

		/// <summary>
		/// Parent Property Name
		/// If there is a parent property, it will not be watched if the parent property is invalid.
		/// </summary>
		public string? Parent { get; set; }

		/// <summary>
		/// Default value to be set when the property is not required
		/// </summary>
		public string? DefaultValue { get; set; }
	}

	/// <summary>
	/// Property Data Type for Validation
	/// </summary>
	public enum PropertyType
    {
		/// <summary>
		/// <see cref="bool"/>
		/// </summary>
		BOOL,
		/// <summary>
		/// <see cref="byte"/>
		/// </summary>
		BYTE,
		/// <summary>
		/// <see cref="sbyte"/>
		/// </summary>
		SBYTE,
		/// <summary>
		/// <see cref="short"/>
		/// </summary>
		SHORT,
		/// <summary>
		/// <see cref="ushort"/>
		/// </summary>
		USHORT,
		/// <summary>
		/// <see cref="int"/>
		/// </summary>
		INT,
		/// <summary>
		/// <see cref="uint"/>
		/// </summary>
		UINT,
		/// <summary>
		/// <see cref="long"/>
		/// </summary>
		LONG,
		/// <summary>
		/// <see cref="ulong"/>
		/// </summary>
		ULONG,
		/// <summary>
		/// <see cref="double"/>
		/// </summary>
		DOUBLE,
		/// <summary>
		/// <see cref="string"/>
		/// </summary>
        STRING,
		/// <summary>
		/// <see cref="Enum"/>
		/// </summary>
		ENUM
    }
}
