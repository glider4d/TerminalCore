using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTerminal.Tools;


public class JsBridge
{
    private readonly IJSRuntime jsEntryPoint;

    public JsBridge(IJSRuntime js)
    {
        this.jsEntryPoint = js;
    }


    public async void Alert(string message)
    {
        await jsEntryPoint.InvokeVoidAsync("alert", message);
    }

    public async void Nav_ScrollIntoView(string className)
    {
        await jsEntryPoint.InvokeVoidAsync("ScrollIntoClass", className);
        //        Alert(className);
        //await jsEntryPoint.InvokeVoidAsync("test", className);

    }

    public async void InitialMap(string className, float xCenter, float yCenter, float zoom)
    {
        await jsEntryPoint.InvokeVoidAsync("initialMap", className, xCenter, yCenter, zoom);
    }

    public async void WinBoxTest()
    {
        await jsEntryPoint.InvokeVoidAsync("MyWinBox.OpenTestWindow");
    }

    public async void AboutBox()
    {
        await jsEntryPoint.InvokeVoidAsync("AboutBox");
    }

    public async void Camera2()
    {



        //   await jsEntryPoint.InvokeVoidAsync("particlesJS");



        await jsEntryPoint.InvokeVoidAsync("camera2");
    }

    public async void GetElementById()
    {
        ElementReference result = await jsEntryPoint.InvokeAsync<ElementReference>("document.getElementById", "charClass");
    }


}