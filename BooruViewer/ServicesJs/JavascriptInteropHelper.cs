using BooruViewer.Services;
using Microsoft.JSInterop;

namespace BooruViewer.ServicesJs
{
    public class JavascriptInteropHelper
    {
        private DotNetObjectReference<JavascriptInteropHelper> ObjectReference { get; set; }
        private IJSRuntime JsRuntime { get; set; }
        private GelbooruService GelbooruService { get; set; }

        public delegate void OnScrollEvent(int positionX, int positionY);
        public event OnScrollEvent OnScroll;
        public int ScrollX { get; private set; }
        public int ScrollY { get; private set; }

        public delegate void OnResizeEvent(int Width, int Height);
        public event OnResizeEvent OnResize;
        public int Height { get; private set; }
        public int Width { get; private set; }

        public delegate void OnSearchChangedEvent(string value);

        public JavascriptInteropHelper(IJSRuntime JsRuntime, GelbooruService GelbooruService)
        {
            this.JsRuntime = JsRuntime;
            this.GelbooruService = GelbooruService;
            this.ObjectReference = DotNetObjectReference.Create(this);

            // setup class reference
            JsRuntime.InvokeVoidAsync("registerJavascriptInteropHelper", ObjectReference);

            var viewSizeTask = GetViewSize();

            viewSizeTask.ContinueWith((sizeTask) =>
            {
                var size = sizeTask.Result;
                Width = size.X;
                Height = size.Y;
            });
        }

        [JSInvokable(nameof(onscroll))]
        public void onscroll(int positionX, int positionY)
        {
            this.ScrollX = positionX;
            this.ScrollY = positionY;
            //Console.WriteLine("ScrollInfoService.OnScroll " + yValue);
            OnScroll?.Invoke(positionX, positionY);
        }

        [JSInvokable(nameof(onresize))]
        public void onresize(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

            //Console.WriteLine("ScrollInfoService.OnScroll " + yValue);
            OnResize?.Invoke(Width, Height);
        }

        public async Task<Point> GetViewSize()
        {
            return await JsRuntime.InvokeAsync<Point>("getViewSize");
        }

        public async Task<int> GetMaxScrollHeight()
        {
            return await JsRuntime.InvokeAsync<int>("getMaxScrollHeight");
        }
    }

    public class AutocompleteResult
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
