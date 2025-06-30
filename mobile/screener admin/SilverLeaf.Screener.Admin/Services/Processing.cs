using Plugin.Connectivity;
using SilverLeaf.Screener.Admin.Objects;
using SilverLeaf.Screener.Admin.Objects.Exceptions;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services
{
    public class Processing : IProcessing
    {
        private readonly INavigator _navigator;
        private readonly IReporting _reporting;

        public Processing(INavigator navigator, IReporting reporting)
        {
            _navigator = navigator;
            _reporting = reporting;

        }

        public Response Process(Action action)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    _navigator.ShowLogin();
                    return new Response() { Succeed = true };
                }

                action();
                return Response.Success();
            }
            catch (Exception ex)
            {
                return CatchException<Response>(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="loadingMessage">If you supply a loading message, it should come from resources, and then a busy message will appear while http executes.</param>
        /// <returns></returns>
        public async Task<Response> Process(Task task, string loadingMessage = null)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    _navigator.ShowLogin();
                    return new Response() { Succeed = true };
                }

                if (!string.IsNullOrEmpty(loadingMessage))
                {


                    await task;

                }
                else
                {
                    await task;
                }
                return Response.Success();
            }
            catch (Exception ex)
            {
                return CatchException<Response>(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="loadingMessage">If you supply a loading message, it should come from resources, and then a busy message will appear while http executes.</param>
        public async Task<Response<T>> Process<T>(Task<T> task, string loadingMessage = null)
        {
            try
            {
                T data;
                if (!CrossConnectivity.Current.IsConnected)
                {
                    _navigator.ShowLogin();
                    return new Response<T>() { Succeed = true };
                }

                if (!string.IsNullOrEmpty(loadingMessage))
                {

                    data = await task;

                }
                else
                {
                    data = await task;
                }

                return Response<T>.Success(data);

            }
            catch (Exception ex)
            {
                return CatchException<Response<T>>(ex);
            }
        }

        private T CatchException<T>(Exception exception) where T : Response, new()
        {
            if (exception is AppError)
            {
                var error = (AppError)exception;
                return Response.Failure<T>(error.Errors);
            }

            _reporting.Report(exception);
            return Response.Cancellation<T>();
        }
    }
}
