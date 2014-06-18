// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Microsoft.AspNet.Mvc
{
    public class MvcOptions
    {
        private readonly ModelBinderDescriptorCollection _binderDescriptors = new ModelBinderDescriptorCollection
        {
            new ModelBinderDescriptor(typeof(TypeConverterModelBinder)),
            new ModelBinderDescriptor(typeof(TypeMatchModelBinder)),
            new ModelBinderDescriptor(typeof(GenericModelBinder)),
            new ModelBinderDescriptor(typeof(MutableObjectModelBinder)),
            new ModelBinderDescriptor(typeof(ComplexModelDtoModelBinder)),
        };

        private AntiForgeryOptions _antiForgeryOptions = new AntiForgeryOptions();

        public AntiForgeryOptions AntiForgeryOptions
        {
            get
            {
                return _antiForgeryOptions;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", 
                                                    Resources.FormatPropertyOfTypeCannotBeNull("AntiForgeryOptions",
                                                                                               typeof(MvcOptions)));
                }

                _antiForgeryOptions = value;
            }
        }

        public ModelBinderDescriptorCollection ModelBinders
        {
            get { return _binderDescriptors; }
        }
    }
}