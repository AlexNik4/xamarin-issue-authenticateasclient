# xamarin-issue-authenticateasclient

I made this as a repo to prove that there is an issue with self signed certificates using .NET6-Android that does not exist in Xamarin.Android.
See this issue for details: <https://github.com/xamarin/xamarin-android/issues/7119>
While there is a difference in implementation, there is a work around for .NET6-Android.
See here for fix: <https://stackoverflow.com/questions/72276421/trust-additional-cas-and-make-use-of-the-android-certificate-store-in-a-net6-mau>
Basically, you have to specifically indicated that User trusted CA are accepted by the app.
