﻿using System;
#if NET45
using System.ComponentModel;
#endif
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    /// <summary>
    /// Test the <see cref="CachedDataAnnotationsMetadataAttributes" /> class.
    /// </summary>
    public class CachedDataAnnotationsMetadataAttributesTest
    {
        [Fact]
        public void Constructor_DefaultsAllPropertiesToNull()
        {
            // Arrange
            var attributes = Enumerable.Empty<Attribute>();

            // Act
            var cache = new CachedDataAnnotationsMetadataAttributes(attributes);

            // Assert
            Assert.Null(cache.Display);
            Assert.Null(cache.DisplayColumn);
            Assert.Null(cache.DisplayFormat);
#if NET45
            Assert.Null(cache.DisplayName);
#endif
            Assert.Null(cache.Editable);
            Assert.Null(cache.Required);
        }

        public static TheoryData<Attribute, Func<CachedDataAnnotationsMetadataAttributes, Attribute>> ExpectedAttributeData
        {
            get
            {
                return new TheoryData<Attribute, Func<CachedDataAnnotationsMetadataAttributes, Attribute>>
                {
                    {
                        new DisplayAttribute(),
                        (CachedDataAnnotationsMetadataAttributes cache) => cache.Display
                    },
                    {
                        new DisplayColumnAttribute("Property"),
                        (CachedDataAnnotationsMetadataAttributes cache) => cache.DisplayColumn
                    },
                    {
                        new DisplayFormatAttribute(),
                        (CachedDataAnnotationsMetadataAttributes cache) => cache.DisplayFormat
                    },
#if NET45
                    {
                        new DisplayNameAttribute(),
                        (CachedDataAnnotationsMetadataAttributes cache) => cache.DisplayName
                    },
#endif
                    {
                        new EditableAttribute(allowEdit: false),
                        (CachedDataAnnotationsMetadataAttributes cache) => cache.Editable
                    },
                    {
                        new RequiredAttribute(),
                        (CachedDataAnnotationsMetadataAttributes cache) => cache.Required
                    },
                };
            }
        }

        [Theory]
        [MemberData("ExpectedAttributeData")]
        public void Constructor_FindsExpectedAttribute(
            Attribute attribute,
            Func<CachedDataAnnotationsMetadataAttributes, Attribute> accessor)
        {
            // Arrange
            var attributes = new[] { attribute };

            // Act
            var cache = new CachedDataAnnotationsMetadataAttributes(attributes);
            var result = accessor(cache);

            // Assert
            Assert.Same(attribute, result);
        }
    }
}