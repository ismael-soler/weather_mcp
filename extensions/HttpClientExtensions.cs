using System.Text.Json;

internal static class HttpClientExt
{
    // extends a HttpClient object by adding the following method:
    public static async Task<JsonDocument> ReadJsonDocumentAsync(this HttpClient client, string requestUri)
    {
        // GET request on the URI, the response is disposed after
        using var response = await client.GetAsync(requestUri);
        // Checks for status code
        response.EnsureSuccessStatusCode();
        // Reads the content and returns it
        return await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
    }
}