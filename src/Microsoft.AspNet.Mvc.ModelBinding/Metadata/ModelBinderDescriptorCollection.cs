// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    /// <summary>
    /// Summary description for ModelBinderDescriptorCollection
    /// </summary>
    public class ModelBinderDescriptorCollection : Collection<ModelBinderDescriptor>
    {
        private readonly List<ModelBinderDescriptor> _descriptors = new List<ModelBinderDescriptor>();

        public ModelBinderDescriptor Add([NotNull] Type modelBinderType)
        {
            return Insert(_descriptors.Count, modelBinderType);
        }

        public ModelBinderDescriptor Insert(int index, [NotNull] Type modelBinderType)
        {
            var descriptor = new ModelBinderDescriptor(modelBinderType);
            _descriptors.Insert(index, descriptor);

            return descriptor;
        }

        public ModelBinderDescriptor Add([NotNull] IModelBinder modelBinder)
        {
            return Insert(_descriptors.Count, modelBinder);
        }

        public ModelBinderDescriptor Insert(int index, [NotNull] IModelBinder modelBinder)
        {
            var descriptor = new ModelBinderDescriptor(modelBinder);
            _descriptors.Insert(index, descriptor);

            return descriptor;
        }
    }
}