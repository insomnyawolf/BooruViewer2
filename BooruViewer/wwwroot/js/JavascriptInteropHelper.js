//registerMethod(reference, name)
//{
//    window[name] = window.reference.invokeMethodAsync(name, window.pageXOffset, window.pageYOffset);
//}

// stores a reference to the c# class so you can call methods on it
function registerJavascriptInteropHelper(JavascriptInteropHelper) {
    window.JavascriptInteropHelper = JavascriptInteropHelper;
}
// actuall calling
window.onscroll = function () {
    if (window.JavascriptInteropHelper != null)
        window.JavascriptInteropHelper.invokeMethodAsync('onscroll', window.pageXOffset, window.pageYOffset);
};

// actuall calling
window.onresize = function () {
    if (window.JavascriptInteropHelper != null) {

        let size = getViewSize();
        window.JavascriptInteropHelper.invokeMethodAsync('onresize', size.x, size.y);
    }
};

function getViewSize() {

    const doc = this.document.documentElement;

    const size = {
        x: doc.clientWidth,
        y: doc.clientHeight
    };

    return size;
}

function getMaxScrollHeight() {
    return document.documentElement.scrollHeight;
}