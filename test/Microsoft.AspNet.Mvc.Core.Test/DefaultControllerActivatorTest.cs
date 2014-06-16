﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET45
using System;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Routing;
using Moq;
using Xunit;

namespace Microsoft.AspNet.Mvc.Core.Test
{
    public class DefaultControllerActivatorTest
    {
        [Fact]
        public void Activate_SetsPropertiesFromActionContextHierarchy()
        {
            // Arrange
            var httpRequest = Mock.Of<HttpRequest>();
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.Request)
                       .Returns(httpRequest);
            httpContext.SetupGet(c => c.RequestServices)
                       .Returns(Mock.Of<IServiceProvider>());
            var routeContext = new RouteContext(httpContext.Object);
            var controller = new TestController();
            var context = new ActionContext(routeContext, new ActionDescriptor())
            {
                Controller = controller
            };
            var activator = new DefaultControllerActivator();

            // Act
            activator.Activate(context);

            // Assert
            Assert.Same(context, controller.ActionContext);
            Assert.Same(httpContext.Object, controller.HttpContext);
            Assert.Same(httpRequest, controller.Request);
        }

        [Fact]
        public void Activate_SetsViewDatDictionary()
        {
            // Arrange
            var service = new Mock<IServiceProvider>();
            service.Setup(s => s.GetService(typeof(IModelMetadataProvider)))
                   .Returns(Mock.Of<IModelMetadataProvider>());

            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.RequestServices)
                       .Returns(service.Object);
            var routeContext = new RouteContext(httpContext.Object);
            var controller = new TestController();
            var context = new ActionContext(routeContext, new ActionDescriptor())
            {
                Controller = controller
            };
            var activator = new DefaultControllerActivator();

            // Act
            activator.Activate(context);

            // Assert
            Assert.NotNull(controller.ViewData);
        }

        [Fact]
        public void Activate_PopulatesServicesFromServiceContainer()
        {
            // Arrange
            var urlHelper = Mock.Of<IUrlHelper>();
            var service = new Mock<IServiceProvider>();
            service.Setup(s => s.GetService(typeof(IUrlHelper)))
                   .Returns(urlHelper);

            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.RequestServices)
                       .Returns(service.Object);
            var routeContext = new RouteContext(httpContext.Object);
            var controller = new TestController();
            var context = new ActionContext(routeContext, new ActionDescriptor())
            {
                Controller = controller
            };
            var activator = new DefaultControllerActivator();

            // Act
            activator.Activate(context);

            // Assert
            Assert.Same(urlHelper, controller.Helper);
        }

        [Fact]
        public void Activate_IgnoresPropertiesThatAreNotDecoratedWithActivateAttribute()
        {
            // Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.Response)
                       .Returns(Mock.Of<HttpResponse>());
            httpContext.SetupGet(c => c.RequestServices)
                       .Returns(Mock.Of<IServiceProvider>());
            var routeContext = new RouteContext(httpContext.Object);
            var controller = new TestController();
            var context = new ActionContext(routeContext, new ActionDescriptor())
            {
                Controller = controller
            };
            var activator = new DefaultControllerActivator();

            // Act
            activator.Activate(context);

            // Assert
            Assert.Null(controller.Response);
        }

        [Fact]
        public void Activate_SetsPropertiesForValueTypes()
        {
            // Arrange
            var response = Mock.Of<HttpResponse>();
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.Response)
                       .Returns(response);
            var routeContext = new RouteContext(httpContext.Object);
            var context = new ActionContext(routeContext, new ActionDescriptor())
            {
                Controller = new TestValueTypeController()
            };
            var activator = new DefaultControllerActivator();

            // Act
            activator.Activate(context);

            // Assert
            var controller = (TestValueTypeController)context.Controller;
            Assert.Same(response, controller.Response);
        }

        public class TestController
        {
            [Activate]
            public ActionContext ActionContext { get; set; }

            [Activate]
            public HttpContext HttpContext { get; set; }

            [Activate]
            public HttpRequest Request { get; set; }

            [Activate]
            public ViewDataDictionary ViewData { get; set; }

            [Activate]
            public IUrlHelper Helper { get; set; }

            public HttpResponse Response { get; set; }
        }

        public struct TestValueTypeController
        {
            [Activate]
            public HttpResponse Response { get; set; }
        }
    }
}
#endif