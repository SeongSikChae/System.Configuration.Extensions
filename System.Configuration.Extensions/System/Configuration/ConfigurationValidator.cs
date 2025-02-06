namespace System.Configuration
{
	using Annotation;
	using Collections.Generic;
	using Reflection;

	/// <summary>
	/// Validator that validates and sets default values ​​for a Property.
	/// </summary>
	public static class ConfigurationValidator
	{
		/// <summary>
		/// Validates an arbitrary type.
		/// </summary>
		/// <typeparam name="T">Type to be verified</typeparam>
		/// <param name="config">Instance to be verified</param>
		/// <exception cref="ArgumentNullException">If the instance to be verified is Null</exception>
		public static void Validate<T>(T? config)
		{
			ArgumentNullException.ThrowIfNull(config);

			Type type = config.GetType();
			IEnumerable<PropertyInfo> properties = type.GetTypeInfo().GetProperties();

			Dictionary<string, IProperty> d = [];
			foreach (PropertyInfo propertyInfo in properties)
			{
				PropertyAttribute? propertyAttribute = propertyInfo.GetCustomAttribute<PropertyAttribute>();
				if (propertyAttribute != null)
					d.Add(propertyInfo.Name, IProperty.Of(config, propertyInfo, propertyAttribute));
			}
			foreach (PropertyInfo propertyInfo in properties)
			{
				PropertyAttribute? propertyAttribute = propertyInfo.GetCustomAttribute<PropertyAttribute>();
				if (propertyAttribute != null)
				{
					IProperty property = d[propertyInfo.Name];
					IProperty? parent = null;
					if (!string.IsNullOrWhiteSpace(propertyAttribute.Parent))
					{
						if (!d.TryGetValue(propertyAttribute.Parent, out parent))
                            throw new Exception($"parent config property '{propertyAttribute.Parent}' not found");
					}
					if (propertyAttribute.Required)
					{
						if (!property.IsValuePresent)
						{
							if (parent is null)
                                throw new Exception($"config field '{propertyInfo.Name}' must be provided");
							else
							{
								if (parent.IsValuePresent)
                                    throw new Exception($"config field '{propertyInfo.Name}' must be provided");
                            }
                        }
						continue;
					}
					if (propertyAttribute.DefaultValue is null)
                        continue;
					if (property.IsValuePresent)
                        continue;
					if (parent is not null)
					{
						if (!parent.IsValuePresent)
							continue;
					}
                    switch (propertyAttribute.Type)
                    {
                        case PropertyType.BOOL:
                            propertyInfo.SetValue(config, bool.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.BYTE:
                            propertyInfo.SetValue(config, byte.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.SBYTE:
                            propertyInfo.SetValue(config, sbyte.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.SHORT:
                            propertyInfo.SetValue(config, short.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.USHORT:
                            propertyInfo.SetValue(config, ushort.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.INT:
                            propertyInfo.SetValue(config, int.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.UINT:
                            propertyInfo.SetValue(config, uint.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.LONG:
                            propertyInfo.SetValue(config, long.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.ULONG:
                            propertyInfo.SetValue(config, ulong.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.DOUBLE:
                            propertyInfo.SetValue(config, double.Parse(propertyAttribute.DefaultValue));
                            break;
                        case PropertyType.STRING:
                            propertyInfo.SetValue(config, propertyAttribute.DefaultValue);
                            break;
						case PropertyType.ENUM:
							Type? t = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
							if (t == null)
								throw new Exception($"config field '{propertyInfo.Name}' must be (Enum?) provided");
							propertyInfo.SetValue(config, Enum.Parse(t, propertyAttribute.DefaultValue));
							break;
                    }
                }
			}

			if (config is IValidatableConfiguration configuration)
				configuration.Validate();
		}
	}

	internal interface IProperty
	{
		bool IsValuePresent { get; }

		internal sealed class BoolProperty(bool? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class ByteProperty(byte? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class SByteProperty(sbyte? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class Int16Property(short? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class UInt16Property(ushort? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class Int32Property(int? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class UInt32Property(uint? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class Int64Property(long? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class UInt64Property(ulong? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class DoubleProperty(double? value) : IProperty
		{
			public bool IsValuePresent => value.HasValue;
		}

		internal sealed class StringProperty(string? value) : IProperty
		{
			public bool IsValuePresent => value is not null;
		}

		internal sealed class EnumProperty(Enum? value) : IProperty
		{
			public bool IsValuePresent => value is not null;
		}

		public static IProperty Of<SourceType>(SourceType o, PropertyInfo p, PropertyAttribute attribute)
		{
			return attribute.Type switch
			{
				PropertyType.BOOL => new BoolProperty((bool?)p.GetValue(o)),
				PropertyType.BYTE => new ByteProperty((byte?)p.GetValue(o)),
				PropertyType.SBYTE => new SByteProperty((sbyte?)p.GetValue(o)),
				PropertyType.SHORT => new Int16Property((short?)p.GetValue(o)),
				PropertyType.USHORT => new UInt16Property((ushort?)p.GetValue(o)),
				PropertyType.INT => new Int32Property((int?)p.GetValue(o)),
				PropertyType.UINT => new UInt32Property((uint?)p.GetValue(o)),
				PropertyType.LONG => new Int64Property((long?)p.GetValue(o)),
				PropertyType.ULONG => new UInt64Property((ulong?)p.GetValue(o)),
				PropertyType.DOUBLE => new DoubleProperty((double?)p.GetValue(o)),
				PropertyType.STRING => new StringProperty(p.GetValue(o) as string),
				PropertyType.ENUM => new EnumProperty((Enum?)p.GetValue(o)),
				_ => throw new Exception($"unknown type '{attribute.Type}'"),
			};
        }
	}
}
