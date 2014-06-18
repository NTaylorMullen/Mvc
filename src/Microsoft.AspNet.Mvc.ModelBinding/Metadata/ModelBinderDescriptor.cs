// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    /// <summary>
    /// Encapsulates information that describes an <see cref="IModelBinder"/>.
    /// </summary>
    public class ModelBinderDescriptor
    {
        private Type _modelBinderType;
        private IModelBinder _binder;

        public ModelBinderDescriptor([NotNull] Type modelBinderType)
        {
            if (!typeof(IModelBinder).IsAssignableFrom(modelBinderType))
            {
                var message = string.Format("Specified type must be a type of {0}.", typeof(IModelBinder).FullName);
                throw new ArgumentException(message, "modelBinder");
            }

            _modelBinderType = modelBinderType;
        }

        public ModelBinderDescriptor([NotNull] IModelBinder binder)
        {
            _binder = binder;
        }

        /// <summary>
        /// Returns an instance of IModelBinder described by this descriptor.
        /// </summary>
        public IModelBinder GetInstance([NotNull] ITypeActivator typeActivator,
                                        [NotNull] IServiceProvider serviceProvider)
        {
            if (_binder == null)
            {
                _binder = (IModelBinder)typeActivator.CreateInstance(serviceProvider, _modelBinderType);
            }

            return _binder;
        }
    }
}