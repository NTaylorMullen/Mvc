// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Mvc
{
    public class DefaultActionBindingContextProvider : IActionBindingContextProvider
    {
        private readonly MvcOptions _mvcOptions;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ITypeActivator _typeActivator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IValueProviderFactory> _valueProviderFactories;
        private readonly IInputFormatterProvider _inputFormatterProvider;
        private readonly IEnumerable<IModelValidatorProvider> _validatorProviders;

        public DefaultActionBindingContextProvider(IOptionsAccessor<MvcOptions> mvcOptions,
                                                   ITypeActivator typeActivator,
                                                   IServiceProvider serviceProvider,
                                                   IModelMetadataProvider modelMetadataProvider,
                                                   IEnumerable<IValueProviderFactory> valueProviderFactories,
                                                   IInputFormatterProvider inputFormatterProvider,
                                                   IEnumerable<IModelValidatorProvider> validatorProviders)
        {
            _mvcOptions = mvcOptions.Options;
            _typeActivator = typeActivator;
            _serviceProvider = serviceProvider;

            _modelMetadataProvider = modelMetadataProvider;
            _valueProviderFactories = valueProviderFactories;
            _inputFormatterProvider = inputFormatterProvider;
            _validatorProviders = validatorProviders;
        }

        public Task<ActionBindingContext> GetActionBindingContextAsync(ActionContext actionContext)
        {
            var factoryContext = new ValueProviderFactoryContext(
                actionContext.HttpContext,
                actionContext.RouteData.Values);

            var valueProviders = _valueProviderFactories.Select(factory => factory.GetValueProvider(factoryContext))
                                                        .Where(vp => vp != null);

            var compositeModelBinder = new CompositeModelBinder(_mvcOptions.ModelBinders,
                                                                _serviceProvider,
                                                                _typeActivator);

            var context = new ActionBindingContext(
                actionContext,
                _modelMetadataProvider,
                compositeModelBinder,
                new CompositeValueProvider(valueProviders),
                _inputFormatterProvider,
                _validatorProviders);

            return Task.FromResult(context);
        }
    }
}
