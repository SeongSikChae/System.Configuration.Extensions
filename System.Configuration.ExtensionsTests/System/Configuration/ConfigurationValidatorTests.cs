using System.Configuration.Annotation;

namespace System.Configuration.Tests
{
    [TestClass]
    public class ConfigurationValidatorTests
    {
        [TestMethod]
        public void NullValidateTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                ConfigurationValidator.Validate<ParentConfig>(null);
            });
        }

        [TestMethod]
        public void UnknownTypeValidateTest()
        {
            Assert.ThrowsException<Exception>(() =>
            {
                UnkownTypeConfig config = new UnkownTypeConfig();
                ConfigurationValidator.Validate(config);
            });
        }

        private class UnkownTypeConfig
        {
            [Property((PropertyType)11)]
            public string? StringValue { get; set; }
        }

        [TestMethod]
        public void DefaultValidateTest()
        {
            ChildConfig config = new ChildConfig();
            ConfigurationValidator.Validate(config);

            Assert.IsTrue(config.BoolValue.HasValue);
            Assert.IsTrue(config.BoolValue.Value);

            Assert.IsTrue(config.ByteValue.HasValue);
            Assert.AreEqual((byte)0, config.ByteValue.Value);

            Assert.IsTrue(config.SByteValue.HasValue);
            Assert.AreEqual((sbyte)-1, config.SByteValue.Value);

            Assert.IsTrue(config.UShortValue.HasValue);
            Assert.AreEqual((ushort)1, config.UShortValue.Value);

            Assert.IsTrue(config.IntValue.HasValue);
            Assert.AreEqual(-1, config.IntValue.Value);

            Assert.IsTrue(config.UIntValue.HasValue);
            Assert.AreEqual((uint)1, config.UIntValue.Value);

            Assert.IsTrue(config.LongValue.HasValue);
            Assert.AreEqual(-1, config.LongValue.Value);

            Assert.IsTrue(config.ULongValue.HasValue);
            Assert.AreEqual((ulong)1, config.ULongValue.Value);

            Assert.IsTrue(config.DoubleValue.HasValue);
            Assert.AreEqual(0.0, config.DoubleValue.Value);

            Assert.IsNotNull(config.StringValue);
            Assert.AreEqual("Hello, World!", config.StringValue);
        }

        private class ParentConfig
        {
            [Property(PropertyType.BOOL, DefaultValue = "true")]
            public bool? BoolValue { get; set; }

            [Property(PropertyType.BYTE, DefaultValue = "0")]
            public byte? ByteValue { get; set; }

            [Property(PropertyType.SBYTE, DefaultValue = "-1")]
            public sbyte? SByteValue { get; set; }

            [Property(PropertyType.SHORT, DefaultValue = "-1")]
            public short? ShortValue { get; set; }

            [Property(PropertyType.USHORT, DefaultValue = "1")]
            public ushort? UShortValue { get; set; }

            [Property(PropertyType.INT, DefaultValue = "-1")]
            public int? IntValue { get; set; }

            [Property(PropertyType.UINT, DefaultValue = "1")]
            public uint? UIntValue { get; set; }

            [Property(PropertyType.LONG, DefaultValue = "-1")]
            public long? LongValue { get; set; }

            [Property(PropertyType.ULONG, DefaultValue = "1")]
            public ulong? ULongValue { get; set; }

            [Property(PropertyType.DOUBLE, DefaultValue = "0.0")]
            public double? DoubleValue { get; set; }

            [Property(PropertyType.STRING, DefaultValue = "Hello, World!")]
            public string? StringValue { get; set; }
        }

        private class ChildConfig : ParentConfig
        {

        }

        [TestMethod]
        public void ParentPropertyTest()
        {
            {
                ParentPropertyNotFoundConfig config = new ParentPropertyNotFoundConfig();
                Assert.ThrowsException<Exception>(() =>
                {
                    ConfigurationValidator.Validate(config);
                });
            }

            {
                ParentPropertyConfig config = new ParentPropertyConfig();
                config.BoolValue = true;
                config.BoolValue4 = true;
                Assert.ThrowsException<Exception>(() =>
                {
                    ConfigurationValidator.Validate(config);
                });
            }
        }

        private class ParentPropertyNotFoundConfig
        {
            [Property(PropertyType.INT, required: true, Parent = "Value")]
            public int? IntValue { get; set; }
        }

        private class ParentPropertyConfig
        {
            [Property(PropertyType.BOOL)]
            public bool? BoolValue4 { get; set; }

            [Property(PropertyType.INT, Parent = "BoolValue4", DefaultValue = "1")]
            public int? IntValue4 { get; set; }

            [Property(PropertyType.BOOL)]
            public bool? BoolValue3 { get; set; }

            [Property(PropertyType.INT, Parent = "BoolValue3", DefaultValue = "1")]
            public int? IntValue3 { get; set; }

            [Property(PropertyType.BOOL)]
            public bool? BoolValue2 { get; set; }

            [Property(PropertyType.INT, required: true, Parent = "BoolValue2")]
            public int? IntValue2 { get; set; }

            [Property(PropertyType.BOOL)]
            public bool? BoolValue { get; set; }

            [Property(PropertyType.INT, required: true, Parent = "BoolValue")]
            public int? IntValue { get; set; }
        }

        [TestMethod]
        public void RequiredPropertyTest()
        {
            RequiredPropertyConfig config = new RequiredPropertyConfig();
            config.StringValue2 = "Hello";
            config.StringValue3 = "Hello";
            config.BoolValue2 = true;
            Assert.ThrowsException<Exception>(() =>
            {
                ConfigurationValidator.Validate(config);
            });
        }

        private class RequiredPropertyConfig
        {
            [Property(PropertyType.STRING, DefaultValue = "Hello")]
            public string? StringValue3 { get; set; }

            [Property(PropertyType.STRING, required: true)]
            public string? StringValue2 { get; set; }

            [Property(PropertyType.STRING, required: true, DefaultValue = "Hello")]
            public string? StringValue { get; set; }

            [Property(PropertyType.STRING, required: true, DefaultValue = "Hello", Parent = "BoolValue2")]
            public string? StringValue4 { get; set; }

            [Property(PropertyType.BOOL, required: true)]
            public bool? BoolValue2 { get; set; }

            [Property(PropertyType.BOOL, required: true)]
            public bool? BoolValue { get; set; }
        }

        [TestMethod]
        public void ValidatableConfigValidateTest()
        {
            ValidatableConfig config = new ValidatableConfig();
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                ConfigurationValidator.Validate(config);
            });
        }

        private class ValidatableConfig : IValidatableConfiguration
        {
            public void Validate()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void EnumPropertyTest()
        {
            EnumPropertyConfig config = new EnumPropertyConfig();
            ConfigurationValidator.Validate(config);
        }

        private class EnumPropertyConfig
        {
			[Property(PropertyType.ENUM, DefaultValue = "A")]
			public TestEnum? Enum1 { get; set; }
		}

		private enum TestEnum
        {
            None, A, B
        }
    }
}