// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;

namespace Microsoft.AspNet.Mvc
{
    public class Controller : IActionFilter, IAsyncActionFilter
    {
        private DynamicViewData _viewBag;

        public HttpContext Context
        {
            get
            {
                return ActionContext.HttpContext;
            }
        }

        public ModelStateDictionary ModelState
        {
            get
            {
                return ViewData.ModelState;
            }
        }

        [Activate]
        public ActionContext ActionContext { get; set; }

        [Activate]
        public IServiceProvider ServiceProvider { get; set; }

        [Activate]
        public IViewEngine ViewEngine { get; set; }

        [Activate]
        public IUrlHelper Url { get; set; }

        public IPrincipal User
        {
            get
            {
                if (Context == null)
                {
                    return null;
                }

                return Context.User;
            }
        }

        [Activate]
        public ViewDataDictionary ViewData { get; set; }

        public dynamic ViewBag
        {
            get
            {
                if (_viewBag == null)
                {
                    _viewBag = new DynamicViewData(() => ViewData);
                }

                return _viewBag;
            }
        }

        [NonAction]
        public ViewResult View()
        {
            return View(view: null);
        }

        [NonAction]
        public ViewResult View(string view)
        {
            return View(view, model: null);
        }

        [NonAction]
        // TODO #110: May need <TModel> here and in the overload below.
        public ViewResult View(object model)
        {
            return View(view: null, model: model);
        }

        [NonAction]
        public ViewResult View(string view, object model)
        {
            // Do not override ViewData.Model unless passed a non-null value.
            if (model != null)
            {
                ViewData.Model = model;
            }

            return new ViewResult(ServiceProvider, ViewEngine)
            {
                ViewName = view,
                ViewData = ViewData,
            };
        }

        [NonAction]
        public ContentResult Content(string content)
        {
            return Content(content, contentType: null);
        }

        [NonAction]
        public ContentResult Content(string content, string contentType)
        {
            return Content(content, contentType, contentEncoding: null);
        }

        [NonAction]
        public ContentResult Content(string content, string contentType, Encoding contentEncoding)
        {
            var result = new ContentResult
            {
                Content = content,
            };

            if (contentType != null)
            {
                result.ContentType = contentType;
            }

            if (contentEncoding != null)
            {
                result.ContentEncoding = contentEncoding;
            }

            return result;
        }

        [NonAction]
        public JsonResult Json(object value)
        {
            return new JsonResult(value);
        }

        [NonAction]
        public virtual RedirectResult Redirect(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new RedirectResult(url);
        }

        [NonAction]
        public virtual RedirectResult RedirectPermanent(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "url");
            }

            return new RedirectResult(url, permanent: true);
        }

        [NonAction]
        public RedirectToActionResult RedirectToAction(string actionName)
        {
            return RedirectToAction(actionName, routeValues: null);
        }

        [NonAction]
        public RedirectToActionResult RedirectToAction(string actionName, object routeValues)
        {
            return RedirectToAction(actionName, controllerName: null, routeValues: routeValues);
        }

        [NonAction]
        public RedirectToActionResult RedirectToAction(string actionName, string controllerName)
        {
            return RedirectToAction(actionName, controllerName, routeValues: null);
        }

        [NonAction]
        public RedirectToActionResult RedirectToAction(string actionName, string controllerName,
                                        object routeValues)
        {
            return new RedirectToActionResult(Url, actionName, controllerName,
                                                TypeHelper.ObjectToDictionary(routeValues));
        }

        [NonAction]
        public RedirectToActionResult RedirectToActionPermanent(string actionName)
        {
            return RedirectToActionPermanent(actionName, routeValues: null);
        }

        [NonAction]
        public RedirectToActionResult RedirectToActionPermanent(string actionName, object routeValues)
        {
            return RedirectToActionPermanent(actionName, controllerName: null, routeValues: routeValues);
        }

        [NonAction]
        public RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName)
        {
            return RedirectToActionPermanent(actionName, controllerName, routeValues: null);
        }

        [NonAction]
        public RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName,
                                        object routeValues)
        {
            return new RedirectToActionResult(Url, actionName, controllerName,
                                                TypeHelper.ObjectToDictionary(routeValues), permanent: true);
        }

        [NonAction]
        public RedirectToRouteResult RedirectToRoute(string routeName)
        {
            return RedirectToRoute(routeName, routeValues: null);
        }

        [NonAction]
        public RedirectToRouteResult RedirectToRoute(object routeValues)
        {
            return RedirectToRoute(routeName: null, routeValues: routeValues);
        }

        [NonAction]
        public RedirectToRouteResult RedirectToRoute(string routeName, object routeValues)
        {
            return new RedirectToRouteResult(Url, routeName, routeValues);
        }

        [NonAction]
        public RedirectToRouteResult RedirectToRoutePermanent(string routeName)
        {
            return RedirectToRoutePermanent(routeName, routeValues: null);
        }

        [NonAction]
        public RedirectToRouteResult RedirectToRoutePermanent(object routeValues)
        {
            return RedirectToRoutePermanent(routeName: null, routeValues: routeValues);
        }

        [NonAction]
        public RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues)
        {
            return new RedirectToRouteResult(Url, routeName, routeValues, permanent: true);
        }

        [NonAction]
        public virtual void OnActionExecuting([NotNull] ActionExecutingContext context)
        {
        }

        [NonAction]
        public virtual void OnActionExecuted([NotNull] ActionExecutedContext context)
        {
        }

        [NonAction]
        public virtual async Task OnActionExecutionAsync(
            [NotNull] ActionExecutingContext context,
            [NotNull] ActionExecutionDelegate next)
        {
            OnActionExecuting(context);
            if (context.Result == null)
            {
                OnActionExecuted(await next());
            }
        }
    }
}
