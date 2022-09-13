using Microsoft.JSInterop;

namespace BooruViewer.ServicesJs
{
    public class JavascriptInteropHelper
    {
        private DotNetObjectReference<JavascriptInteropHelper> ObjectReference { get; set; }
        private IJSRuntime JsRuntime { get; set; }

        public Point Scroll { get; set; } = new Point();
        public delegate void OnScrollEvent(int positionX, int positionY);
        public event OnScrollEvent OnScroll;

        public Point Size { get; set; } = new Point();
        public delegate void OnResizeEvent(int Width, int Height);
        public event OnResizeEvent OnResize;

        public JavascriptInteropHelper(IJSRuntime JsRuntime)
        {
            this.JsRuntime = JsRuntime;
            this.ObjectReference = DotNetObjectReference.Create(this);

            // setup class reference
            JsRuntime.InvokeVoidAsync("registerJavascriptInteropHelper", ObjectReference);

            var viewSizeTask = GetViewSize();

            viewSizeTask.ContinueWith((sizeTask) =>
            {
                var size = sizeTask.Result;
                Size = size;
            });
        }

        [JSInvokable(nameof(onscroll))]
        public void onscroll(int positionX, int positionY)
        {
            Scroll.X = positionX;
            Scroll.Y = positionY;
            //Console.WriteLine("ScrollInfoService.OnScroll " + yValue);
            OnScroll?.Invoke(positionX, positionY);
        }

        public async Task SetScroll(Point scroll)
        {
            await JsRuntime.InvokeVoidAsync("setScroll", scroll.X, scroll.Y);
        }

        [JSInvokable(nameof(onresize))]
        public void onresize(int Width, int Height)
        {
            Size.X = Width;
            Size.Y = Height;
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

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
