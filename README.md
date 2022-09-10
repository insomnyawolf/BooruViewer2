# BooruViewer2

Remake of [BooruViewer](https://github.com/insomnyawolf/BooruViewer) which may actually turn into a functional app.

## Todo

* Style the search menu
* Bigger menu toggle button
* Rating search control
* Size search control
* Scroll to top when search changes
* Local favourites
* Saved searchs

## Done

* Basic search
* Tokenized search input
* Principal view with responsive layout
* Base search menu
* Infinite scroll


## Issues

Unknown cause => probably on JavascriptInteropHelper.cs y JavascriptInteropHelper.js
```
[chromium] [INFO:CONSOLE(1)] "Uncaught (in promise) Error: System.Text.Json.JsonException: The JSON value could not be converted to System.Int32. Path: $ | LineNumber: 0 | BytePositionInLine: 18.
[chromium]  ---> System.FormatException: Either the JSON value is not in a supported format, or is out of bounds for an Int32.
[chromium]    at System.Text.Json.Utf8JsonReader.GetInt32()
[chromium]    at System.Text.Json.Serialization.Converters.Int32Converter.Read(Utf8JsonReader& reader, Type typeToConvert, JsonSerializerOptions options)
[chromium]    at System.Text.Json.Serialization.JsonConverter`1[[System.Int32, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]].TryRead(Utf8JsonReader& reader, Type typeToConvert, JsonSerializerOptions options, ReadStack& state, Int32& value)
[chromium]    at System.Text.Json.Serialization.JsonConverter`1[[System.Int32, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]].ReadCore(Utf8JsonReader& reader, JsonSerializerOptions options, ReadStack& state)
[chromium]    --- End of inner exception stack trace ---
```