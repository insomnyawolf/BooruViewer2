dotnet publish -f:net6.0-android -c:Release

rem dotnet publish -f:net6.0-android -c:Release /p:AndroidSigningKeyPass=mypassword /p:AndroidSigningStorePass=mypassword

rem sign --ks "C:\Users\inso\AppData\Local\Xamarin\Mono for Android\debug.keystore" --ks-pass pass:android --ks-key-alias androiddebugkey --key-pass pass:android --min-sdk-version 24 --max-sdk-version 31  bin\Debug