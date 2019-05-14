using Castle.MicroKernel.Registration;
using Castle.Windsor;
using HttpRestApiServer.Attributes;
using Lab6.Attributes;
using Lab6.FakeDatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpRestApiServer
{
    public class HttpServerHandler       
    {
        readonly HttpListener _httpListener;
        

        readonly WindsorContainer _container;

        public HttpServerHandler(string ip, int port)
        {
            string prefix = $"http://{ip}:{port}/";
            _httpListener = new HttpListener();            
            _httpListener.Prefixes.Add(prefix);

            _container = new WindsorContainer();

            _container.Register(Component.For<CarsFakeTable>());
            _container.Register(Component.For<MutantsFakeTable>());

            Console.WriteLine("Server started:");
            Console.WriteLine($"ip:\t{ip}");
            Console.WriteLine($"port:\t{port}");
        }

        public int MaxThreadsCount { get; private set; }
        public int MinThreadsCount { get; private set; }


        public void StartListen()
        {
            Console.WriteLine("Start listening");
            _httpListener.Start();
            Console.WriteLine("Listening started ");
            while (true)
            {
                HttpListenerContext httpListenerContext = _httpListener.GetContext();

                Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
                Thread.Start(httpListenerContext);
            }
        }

        private void ClientThread(object contextObj)
        {
            HttpListenerContext context = contextObj as HttpListenerContext;

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string controllerName = request.Url.Segments[1].Replace("/", "");

            string[] urlParams = request.Url.Segments.Skip(2).Select(s => s.Replace("/", "")).ToArray();

            var controllerType = GetTypeController(controllerName);


            if (controllerType == null)
            {
                Stream o = response.OutputStream;
                var resp = Encoding.UTF8.GetBytes("<html><head><meta charset='utf8'></head><body>not found</body></html>");
                o.Write(resp, 0, resp.Length);
                o.Close();
                return;
            }
                

            object[] controllerCreationsParams = controllerType.GetConstructors().FirstOrDefault()?.GetParameters().Select(t => _container.Resolve(t.ParameterType)).ToArray();

            var controller = Activator.CreateInstance(controllerType, controllerCreationsParams);

            var method = controllerType.GetMethods().FirstOrDefault(t => t.GetCustomAttribute<PageAttribute>()?.ValidationUrl(urlParams) ?? false);

            object[] @params = GenerateParams(urlParams, method, GetRequestPostData(request));


            if (@params == null)
            {
                Stream o = response.OutputStream;
                var resp = Encoding.UTF8.GetBytes("<html><head><meta charset='utf8'></head><body>not found</body></html>");
                o.Write(resp, 0, resp.Length);
                o.Close();
                return;
            }


            object ret = default;
            try
            {
                ret = method.Invoke(controller, @params);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is HttpStatusCodeException)
                {
                    var errormethod = controllerType.GetMethods().FirstOrDefault(t => t.GetCustomAttribute<ErrorAttribute>()?.HttpCode == (ex.InnerException as HttpStatusCodeException).StatusCode);
                    ret = errormethod.Invoke(controller, null);
                }
            }

            byte[] buffer = Encoding.UTF8.GetBytes((ret as IHttpResponse)?.Data ?? string.Empty);


            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
        }

        private static object[] GenerateParams(string[] urlParams, MethodInfo method, string postdata = "")
        {
            try
            {
                var retparams = method.GetParameters().Select((p, i) =>
                {
                    if (p.GetCustomAttribute(typeof(PostDataAttribute)) != null)
                        return postdata;

                    return Convert.ChangeType(urlParams[i], p.ParameterType);
                });

                return retparams.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private Type GetTypeController(string controllerName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllerType = assembly.GetTypes().FirstOrDefault(t => string.Equals(t.GetCustomAttribute<ControllerAttribute>()?.ControllerName, controllerName, StringComparison.OrdinalIgnoreCase));

            return controllerType;
        }

        public static string GetRequestPostData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return null;
            }
            using (System.IO.Stream body = request.InputStream)
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

// request -> if url == controllerName -> invoke controller with controllerName